using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] soundtrack;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (!GetComponent<AudioSource>().playOnAwake)
        {
            GetComponent<AudioSource>().clip = soundtrack[Random.Range(0, soundtrack.Length)];
            GetComponent<AudioSource>().Play();
        }
        else
        {
         
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().clip = soundtrack[Random.Range(0, soundtrack.Length)];
            GetComponent<AudioSource>().Play();
        }
    }
}
