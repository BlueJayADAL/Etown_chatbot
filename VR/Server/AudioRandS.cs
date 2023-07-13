using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI;
using System.Net;
using System.Threading.Tasks;


public class AudioRandS : MonoBehaviour
{
    private const int SampleRate = 44100;
    private string wavFilePath; // Path to the .wav file
  //  public AudioClip Clip;
  //  public AudioSource audioSource;
    private const string OutputFileName = "recorded_audio.wav";
    public Text textField;

    private AudioClip recordedClip;
    private bool isRecording;
    public AudioPlayerSTT player;
    public GameObject light;
    public GameObject mic;
    public AudioSource wait;

    private byte[] receivedData = null;
    private byte[] Datamm = null;

    private void Update()
    {
        // Start or stop recording on a key press (e.g., spacebar)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isRecording)
            {
                StartRecording();
            }
            else
            {
                StopRecording();
            }
        }
    }

    public void StartRecording()
    {
        if (isRecording)
             return;
        recordedClip = Microphone.Start(null, false, 10, SampleRate);
        isRecording = true;
        Debug.Log("Recording started...");
        receivedData = null;
        mic.SetActive(true);
    }

public async void StopRecording()
{
    if (!isRecording)
        return;
    mic.SetActive(false);
    Microphone.End(null);
    isRecording = false;
    light.SetActive(true);
    if (recordedClip == null)
        return;
    
    wait.Play();
    // Convert the audio clip to a float array
    float[] samples = new float[recordedClip.samples];
    recordedClip.GetData(samples, 0);

    // Create a WAV file from the audio clip data
    byte[] wavData = ConvertToWav(samples);

    // Create separate threads for sending and receiving audio data
     // Call the SendAudioData method asynchronously and await its completion
    Task sendTask = Task.Run(() => SendAudioData(wavData));

    // Wait for SendAudioData to complete
    sendTask.Wait();

    // Call the ReceiveAudioData method asynchronously
    Task receiveTask = Task.Run(() => ReceiveAudioData());

    // Wait for ReceiveAudioData to complete
    receiveTask.Wait();

   // SaveAsWavFile(Datamm, OutputFileName);
    //Thread.Sleep(15000);

    
}

    private async void SendAudioData(byte[] data)
    {
        // Socket configuration
        string serverAddress = "35.245.154.81";
        int serverPort = 45250;

        try
        {
            // Create a TCP client socket
            TcpClient client = new TcpClient();

            // Connect to the server asynchronously
            await client.ConnectAsync(IPAddress.Parse(serverAddress), serverPort);
 
            // Get a network stream for sending data
            NetworkStream stream = client.GetStream();

            // Send the audio data asynchronously
            await stream.WriteAsync(data, 0, data.Length);

            // Close the stream and client socket
            stream.Close();
            client.Close();

            Debug.Log("Audio data sent successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to send audio data: {e.Message}");
        }
    }
    private async void ReceiveAudioData()
    {
        // Socket configuration
        string serverAddress = "35.245.154.81";
        int serverPort = 45251;

        
        while(receivedData==null){
        try
        {
            // Create a TCP client socket
            TcpClient client = new TcpClient();

            // Connect to the server asynchronously
            await client.ConnectAsync(IPAddress.Parse(serverAddress), serverPort);

            // Get a network stream for receiving data
            NetworkStream stream = client.GetStream();

            // Read the audio data into a memory stream
            MemoryStream memoryStream = new MemoryStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                memoryStream.Write(buffer, 0, bytesRead);
            }

            // Get the received audio data as bytes
            receivedData = memoryStream.ToArray();

            // Close the memory stream and client socket
            memoryStream.Close();
            client.Close();

            Debug.Log("Audio data received successfully.");
            Datamm = receivedData;
            SaveAsWavFile(Datamm, OutputFileName);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to receive audio data: {e.Message}");
        }

        
        }
        return;
    }
    private byte[] ConvertToWav(float[] samples)
    {
        MemoryStream stream = new MemoryStream();

        // WAV file header
        WriteString(stream, "RIFF"); // Chunk ID
        WriteInt(stream, 36 + samples.Length * 2); // Chunk Size
        WriteString(stream, "WAVE"); // Format

        // Subchunk 1: Format
        WriteString(stream, "fmt "); // Subchunk 1 ID
        WriteInt(stream, 16); // Subchunk 1 Size
        WriteShort(stream, 1); // Audio Format (1 = PCM)
        WriteShort(stream, 1); // Number of Channels
        WriteInt(stream, SampleRate); // Sample Rate
        WriteInt(stream, SampleRate * 2); // Byte Rate
        WriteShort(stream, 2); // Block Align
        WriteShort(stream, 16); // Bits per Sample

        // Subchunk 2: Data
        WriteString(stream, "data"); // Subchunk 2 ID
        WriteInt(stream, samples.Length * 2); // Subchunk 2 Size

        // Convert float samples to short (16-bit) samples
        short[] shortSamples = new short[samples.Length];
        for (int i = 0; i < samples.Length; i++)
        {
            shortSamples[i] = (short)(samples[i] * 32767);
        }

        // Write the audio data
        byte[] sampleData = new byte[shortSamples.Length * 2];
        Buffer.BlockCopy(shortSamples, 0, sampleData, 0, sampleData.Length);
        stream.Write(sampleData, 0, sampleData.Length);

        // Set the stream position to the beginning
        stream.Position = 0;

        // Convert the stream to a byte array
        byte[] wavData = stream.ToArray();

        // Clean up the stream
        stream.Dispose();

        return wavData;
    }

    private void WriteString(Stream stream, string s)
    {
        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(s);
        stream.Write(bytes, 0, bytes.Length);
    }

    private void WriteInt(Stream stream, int value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
    }

    private void WriteShort(Stream stream, short value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
    }
    private void SaveAsWavFile(byte[] wavData, string fileName)
    {
        
       
        // Get the program's directory path
        string programDirectory = Application.persistentDataPath;
        Debug.Log(programDirectory);
        Debug.Log(fileName);
        // Combine the program's directory path with the desired file name
        string filePath = programDirectory+"/"+ fileName;

        // Write the byte array to a file
        File.WriteAllBytes(filePath, receivedData);
        wavFilePath=filePath;
        Debug.Log($"Saved audio data to {fileName} @ {wavFilePath}");
       // Thread.Sleep(5000);
       textField.text = "File Saved!";
        light.SetActive(false);

        player.Play();
           
    }
 private void PlayAudio()
    {
        //audioSource = GetComponent<AudioSource>();

        // Load the .wav file as an AudioClip
       // AudioClip audioClip = LoadAudioClip(wavFilePath);

        // Assign the AudioClip to the AudioSource component
       // audioSource.clip = Clip;

        // Play the audio
        //audioSource.Play();
    }

    private AudioClip LoadAudioClip(string filePath)
    {
        // Load the .wav file as an AudioClip
        AudioClip audioClip = Resources.Load<AudioClip>(filePath);

        if (audioClip == null)
        {
            Debug.LogError($"Failed to load audio clip at path: {filePath}");
        }

        return audioClip;
    }

    private IEnumerator DelayedAction()
    {
        Debug.Log("Coroutine started");
        

        yield return new WaitForSeconds(10f);
        
        
        Debug.Log("Coroutine ended");

    }
}














