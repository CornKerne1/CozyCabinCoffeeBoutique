using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDeleteGarbage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("Play_Radio_RoastBlend", this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
