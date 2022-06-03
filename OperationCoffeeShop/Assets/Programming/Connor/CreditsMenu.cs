using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : MonoBehaviour
{
    public GameObject creditsScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseCredits()
    {
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
        Destroy(creditsScreen);
    }
}
