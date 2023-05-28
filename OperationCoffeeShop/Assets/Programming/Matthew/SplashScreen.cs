using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SplashScreen : MonoBehaviour
{
    [SerializeField]private GameObject WwiseBank;

    private void Start()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene(1);
        Destroy(WwiseBank);
    }
}

