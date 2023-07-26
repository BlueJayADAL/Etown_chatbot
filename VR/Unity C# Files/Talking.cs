using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi.TTS.Utilities;
using TMPro;


public class UnityChanTalks : MonoBehaviour
{

    public Animator anim;

    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
            {
                print ("I pressed H");
                anim.Play("WAIT03",-1,0f);
            }
    }
}
