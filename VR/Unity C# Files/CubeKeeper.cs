using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi.TTS.Utilities;
using TMPro;


public class CubeKeeper : MonoBehaviour
{
    [SerializeField] private TTSSpeaker _speaker;
    [SerializeField]             public TMP_InputField user_inputField;
  
   // public void OnEnable(){

   //     _speaker.Events.OnFinishedSpeaking.AddListener(OnFinishedSpeaking);
    
    
 //   }
  //  public void OnDisable(){

    //    _speaker.Events.OnFinishedSpeaking.RemoveListener(OnFinishedSpeaking);




   // }
//public void OnFinishedSpeaking(TTSSpeaker arg0, string arg1){





//}
    public void SpeakPlease(){
        string text = user_inputField.text;
        _speaker.Speak(text);








    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
