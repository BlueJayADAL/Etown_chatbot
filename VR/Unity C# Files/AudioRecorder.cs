
using System.Collections;
using System.Net.Sockets;
using UnityEngine;
using System;
using TMPro;
using System.Net;
using System.Text;
using System.IO;

public class AudioRecorder : MonoBehaviour
{
    private const int SampleRate = 44100;
    private const int ClipLength = 10; // Length of audio clip to record (in seconds)
    private AudioClip recordedClip;
    public AudioSource audioSource;
    private const string savePath = "received.wav";
    private bool isntRecording = false;

    public void MicStart()
    {
        // Start recording audio from the microphone
        recordedClip = Microphone.Start(null, false, ClipLength, SampleRate);
        isntRecording = false;
    }

    public void MicStop()
    {
        // Stop recording audio from the microphone
           Microphone.End(null);
            isntRecording = true;
    }

    private void Update()
    {
        if (isntRecording)
        {
            // Check if the microphone stopped recording
            if (!Microphone.IsRecording(null) && recordedClip != null)
            {
                isntRecording = false;
                // Convert the audio clip to a byte array
                float[] samples = new float[recordedClip.samples];
                recordedClip.GetData(samples, 0);
                byte[] audioData = ConvertAudioClipToByteArray(samples);

                // Send the audio data over the socket connection
                //SendAudioDataOverSocket(audioData);
        string serverIP = "127.0.0.1";
        int serverPort = 12345;
        string audioFilePath = "audio.wav";

        // Connect to the server
        TcpClient client = new TcpClient(serverIP, serverPort);
        Console.WriteLine($"Connected to {serverIP}:{serverPort}");

        // Send audio data to the server
        using (NetworkStream stream = client.GetStream())
        {
            // Read the audio file
            

            // Send the audio data to the server
            stream.Write(audioData, 0, audioData.Length);
            Console.WriteLine($"Sent {audioData.Length} bytes of audio data");

            // Receive the response audio file from the server
            byte[] buffer = new byte[4096];
            int bytesRead;
            using (FileStream fileStream = File.Create("response.wav"))
            {
                // Read the file size
                int fileSize = 0;
                bytesRead = stream.Read(buffer, 0, 4);
                if (bytesRead == 4)
                {
                    fileSize = BitConverter.ToInt32(buffer, 0);
                    Console.WriteLine($"Received file size: {fileSize}");
                }

                // Read the file data
                int totalBytesRead = 0;
                while (totalBytesRead < fileSize)
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    fileStream.Write(buffer, 0, bytesRead);
                    totalBytesRead += bytesRead;
                }

                Console.WriteLine($"Received {totalBytesRead} bytes of response audio data");
            }
        }

        // Close the connection
        client.Close();
    
                // Destroy the AudioClip
                Destroy(recordedClip);
                recordedClip = null;
                isntRecording=true;
            }
        }
    }

    private byte[] ConvertAudioClipToByteArray(float[] samples)
    {
        // Convert float samples to 16-bit PCM format
        short[] pcmSamples = new short[samples.Length];
        for (int i = 0; i < samples.Length; i++)
        {
            pcmSamples[i] = (short)(samples[i] * 32767);
        }

        // Convert PCM samples to byte array
        byte[] byteArray = new byte[pcmSamples.Length * 2];
        Buffer.BlockCopy(pcmSamples, 0, byteArray, 0, byteArray.Length);

        return byteArray;
    }

    private void SendAudioDataOverSocket(byte[] audioData)
    {
        // Establish a socket connection and send the audio data
        using (TcpClient client = new TcpClient())
        {
            client.Connect("127.0.0.1", 12345); // Replace with your server IP and port

            using (NetworkStream networkStream = client.GetStream())
            {
                // Send the audio data
                networkStream.Write(audioData, 0, audioData.Length);

                // Receive the response data
                byte[] buffer = new byte[4194304];
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    int bytesRead;
                    while ((bytesRead = networkStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memoryStream.Write(buffer, 0, bytesRead);
                    }

                    // Save the received audio data as a .wav file
                    string outputPath = Path.Combine(Application.dataPath, "Output", "received.wav");
                    File.WriteAllBytes(outputPath, memoryStream.ToArray());
                    Debug.Log("Data received and saved as a .wav file.");
                }
            }
        }
    }
}