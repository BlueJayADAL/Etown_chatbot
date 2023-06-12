using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerSTT : MonoBehaviour
{
    public string fileName = "recorded_audio.wav";
    public AudioSource audioSource;
    public SaveLoadWav saveLoadWav = new SaveLoadWav();
    // Start is called before the first frame update
    public void Play()
    {
        /*
        string programDirectory = Application.persistentDataPath;
        Debug.Log(programDirectory);
        Debug.Log(fileName);
        // Combine the program's directory path with the desired file name
        string filePath = programDirectory+"/"+ fileName;

        AudioClip audioClip = LoadAudioClip(filePath);
        audioSource.clip = audioClip;

        // Play the audio
        audioSource.Play();
    
        */


        StartCoroutine(saveLoadWav.Load(fileName, audioSource));


    // Update is called once per frame
  
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
}
