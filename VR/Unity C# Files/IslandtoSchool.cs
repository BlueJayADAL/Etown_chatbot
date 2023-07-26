using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IslandtoSchool : MonoBehaviour
{
    public GameObject island; 
   // public GameObject School;
    private bool switched = true;
    // Start is called before the first frame update
    public void OnPress()
    {
        if(switched){
            School();
            switched=false;
        }
        else{
            Island();
            switched=true;
        }
    }

    void Island()
    {
        SceneManager.UnloadSceneAsync("School");
        island.SetActive(true);
    }
    void School()
    {
        SceneManager.LoadScene("School", LoadSceneMode.Additive);

        island.SetActive(false);

    }


}


