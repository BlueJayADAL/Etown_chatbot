using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadP : MonoBehaviour
{
    public void Start()
    {
        SceneManager.LoadScene("Starter", LoadSceneMode.Additive);
    }
}

