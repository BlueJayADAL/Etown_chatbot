using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadSample : MonoBehaviour
{
    [SerializeField] private int sceneIndexToUnload = 2;
    public GameObject stuff;

    private void Start()
    {
        stuff.SetActive(false);

         SceneManager.UnloadSceneAsync("Island");
    }
}
