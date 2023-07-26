using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using UnityEngine.XR;
using TMPro;
public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen; // Reference to the loading screen UI
    public GameObject button;
    public GameObject button2;
    public GameObject ipinfo;
    public GameObject player;
    public GameObject plane;
     public GameObject cube;
    public Text textField;
    private string sceneName = "Island";
    private Vector3 targetPosition = new Vector3(-19.71f, 0f, 5.02f);
    private Vector3 targetPosition1 = new Vector3(-19.71f, 5f, 5.02f);
    public OVRPlayerController characterController;
    public Transform teleportTarget;
    public TMP_InputField ip;
    public TMP_InputField port1;
    public TMP_InputField port2;
    public void LoadSceneWithLoadingScreen()
    
    {

        if(ip.text != null && ip.text != "" && ip.text != " "){
        PlayerPrefs.SetString("ip", ip.text);}
        else{PlayerPrefs.SetString("ip", "0");}
        if(port1.text != null && port1.text != "" && port1.text != " "){
        PlayerPrefs.SetInt("port1", int.Parse(port1.text));}
        else{PlayerPrefs.SetInt("port1", 0);}
        if(port2.text != null && port2.text != "" && port2.text != " "){
        PlayerPrefs.SetInt("port2", int.Parse(port2.text));}
        else{PlayerPrefs.SetInt("port2", 0);}
        // characterController = GetComponent<OVRPlayerController>();
        //characterController = GetComponent<OVRCharacterController>();
        // Show the loading screen
        loadingScreen.SetActive(true);
        button.SetActive(false);
        button2.SetActive(false);
        ipinfo.SetActive(false);
        plane.SetActive(false);
        string Starter = "Starter";
        UnloadScene(Starter);
        // Start loading the target scene asynchronously
        SceneManager.LoadScene("Island", LoadSceneMode.Additive);;
        float number = 100.0f;
        // Don't allow the scene to automatically activate upon loading
        UpdateLoadingScreen(number);

        // While the scene is not yet loaded completely
        
            // Calculate the loading progress (0 to 1)
         
        loadingScreen.SetActive(false);

            // Update the loading screen progress (e.g., update progress bar, loading text)
       // cube.transform.position = targetPosition1;
            // Check if the loading progress reaches a desired point before activating the scene
     //   characterController.enabled = false;
     //   characterController.enabled = true;
      //  playerController.Teleported = true;
     //   player.transform.position = teleportTarget.position;
         
        player.transform.Rotate(0f, 90f, 0f);
    //    characterController.enabled = true;
                // Hide the loading screen

                // Allow the scene to activate
      //    player.transform.position = teleportTarget.position;
        }
 
    

    private void UpdateLoadingScreen(float progress)
    {
        // Update the loading screen UI based on the progress value
        // You can update a progress bar, loading text, or any other relevant UI elements
        textField.text = Convert.ToInt32(progress).ToString()+"%";
    }

    public void UnloadScene(string sceneName1)
    {
        SceneManager.UnloadSceneAsync(sceneName1);
    }
}