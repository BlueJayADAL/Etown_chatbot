using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AudioPlayerSTT : MonoBehaviour
{
    public string filename = "recorded_audio.wav";
    public string waitfile = "wait_audio.wav";
    public string welcomefile = "welcome_audio.wav";
    public AudioSource audioSource;
    public AudioSource Starter;
    public Text textField;
    public Text textField1;
    public Text textField2;
    public Text textField3;
    public Text textField4;
    public Animator animator;

    public void Start(){

        Starter.Play(); 

    }
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
                animator = GetComponent<Animator>();
                animator.SetBool("landing", true);
            }
        }

        textField2.text = "Got to here 2";
    }
    public void Waiter()
    {

        if (!string.IsNullOrEmpty(waitfile) && audioSource != null)
        {
            string path = GetPath(waitfile);
            Debug.Log(path);

            if (File.Exists(path))
            {
                StartCoroutine(LoadAudioClip(path));
                animator = GetComponent<Animator>();
                animator.SetBool("landing", true);

            }
        }

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
                animator.SetBool("landing", false);
            }
            else
            {
                Debug.LogError($"Failed to load audio clip: {www.error}");
                animator.SetBool("landing", false);

            }
        }
    }

    private string GetPath(string filename) 
    {
        string programDirectory = Application.persistentDataPath;
        return programDirectory+"/"+ filename;
    }
}

