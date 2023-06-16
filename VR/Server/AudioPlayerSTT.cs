using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AudioPlayerSTT : MonoBehaviour
{
    public string filename = "recorded_audio.wav";
    public AudioSource audioSource;
    public Text textField;
    public Text textField1;
    public Text textField2;
    public Text textField3;
    public Text textField4;

    public void Play()
    {
        textField1.text = "Got to here";

        if (!string.IsNullOrEmpty(filename) && audioSource != null)
        {
            textField4.text = "Attempted this path string";
            string path = GetPath(filename);
            Debug.Log(path);

            if (File.Exists(path))
            {
                textField3.text = "Attempted this path exists";
                StartCoroutine(LoadAudioClip(path));
            }
        }

        textField2.text = "Got to here 2";
    }

    private System.Collections.IEnumerator LoadAudioClip(string path)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = audioClip;
                audioSource.Play();
                textField.text = "Audio Played";
            }
            else
            {
                Debug.LogError($"Failed to load audio clip: {www.error}");
            }
        }
    }

    private string GetPath(string filename) 
    {
        string programDirectory = Application.persistentDataPath;
        return programDirectory+"/"+ filename;
    }
}

