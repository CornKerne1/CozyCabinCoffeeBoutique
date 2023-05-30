using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SplashScreen : MonoBehaviour
{
    [SerializeField]private GameObject WwiseBank;

    private async void Start()
    {
        await Timer();
    }

    private async Task Timer()
    {
        await Task.Delay(500);
        AkSoundEngine.PostEvent("Play_PolyBlossom", gameObject);
        await Task.Delay(6500);
        SceneManager.LoadScene(1);
        Destroy(WwiseBank);
    }
}

