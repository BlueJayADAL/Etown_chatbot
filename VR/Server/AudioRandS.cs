using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
public class AudioRandS : MonoBehaviour
{
    private const int SampleRate = 44100;
    private const string OutputFileName = "recorded_audio.wav";

    private AudioClip recordedClip;
    private bool isRecording;

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

    private void StartRecording()
    {
        recordedClip = Microphone.Start(null, false, 10, SampleRate);
        isRecording = true;
        Debug.Log("Recording started...");
    }

private void StopRecording()
{
    if (!isRecording)
        return;

    Microphone.End(null);
    isRecording = false;

    if (recordedClip == null)
        return;

    // Convert the audio clip to a float array
    float[] samples = new float[recordedClip.samples];
    recordedClip.GetData(samples, 0);

    // Create a WAV file from the audio clip data
    byte[] wavData = ConvertToWav(samples);

    // Send the WAV data over a socket
    SendAudioData(wavData);
    Thread.Sleep(5000);
    // Receive the WAV data from the socket
    byte[] receivedData = ReceiveAudioData();

    // Save the received WAV data as a file
    SaveAsWavFile(receivedData, OutputFileName);
}

private void SendAudioData(byte[] data)
{
    // Socket configuration
    string serverAddress = "127.0.0.1";
    int serverPort = 12345;

    // Create a TCP client socket
    TcpClient client = new TcpClient();

    try
    {
        // Connect to the server
        client.Connect(serverAddress, serverPort);

        // Get a network stream for sending data
        NetworkStream stream = client.GetStream();

        // Send the audio data
        stream.Write(data, 0, data.Length);

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
private byte[] ReceiveAudioData()
{
    // Socket configuration
    string serverAddress = "127.0.0.1";
    int serverPort = 12345;

    // Create a TCP client socket
    TcpClient client = new TcpClient();

    byte[] receivedData = null;

    try
    {
        // Connect to the server
        client.Connect(serverAddress, serverPort);

        // Get a network stream for receiving data
        NetworkStream stream = client.GetStream();

        // Read the audio data into a memory stream
        MemoryStream memoryStream = new MemoryStream();
        byte[] buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            memoryStream.Write(buffer, 0, bytesRead);
        }

        // Get the received audio data as bytes
        receivedData = memoryStream.ToArray();

        // Close the memory stream and client socket
        memoryStream.Close();
        client.Close();

        Debug.Log("Audio data received successfully.");
    }
    catch (Exception e)
    {
        Debug.LogError($"Failed to receive audio data: {e.Message}");
    }

    return receivedData;
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
        // Write the byte array to a file
        File.WriteAllBytes(fileName, wavData);

        Debug.Log($"Saved audio data to {fileName}");
    }

















}