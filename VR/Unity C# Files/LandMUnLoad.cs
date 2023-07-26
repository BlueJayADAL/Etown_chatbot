using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LandMUnLoad : MonoBehaviour
{
    public GameObject stuff;
    public GameObject stuff1;

    // Start is called before the first frame update
    void Start()
    {
        stuff.SetActive(false);
        stuff1.SetActive(false);
    }

    // Update is called once per frame
}
