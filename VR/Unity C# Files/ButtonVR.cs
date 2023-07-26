using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;
    AudioSource sound;
    bool isPressed;


    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        isPressed=false;
    }
    private void OnTriggerEnter(Collider other){
        if (!isPressed){
            button.transform.localPosition = new Vector3(0,0.003f,0);
            presser = other.gameObject;
            onPress.Invoke();
            sound.Play();
            isPressed = true;
        }


    }
    private void OnTriggerExit(Collider other){
        if(other.gameObject==presser){
            button.transform.localPosition = new Vector3(0,0.015f,0);
            onRelease.Invoke();
            isPressed = false;


        }

    }
    public void SpawnSphere(){
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(.5f,.5f,.5f);
        sphere.transform.localPosition = new Vector3(0,1,2); 
        sphere.AddComponent<Rigidbody>();



    }





}
