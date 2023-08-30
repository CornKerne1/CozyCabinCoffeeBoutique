using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class KillZone : MonoBehaviour
{
    private GameMode _gM;
    [SerializeField]private Transform resetTrans;
    private Image _fade;
    private void Start()
    {
        _gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
    }

    private async void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 7) return;
        _gM.playerData.canMove = false;
        await HandleFade();
        _gM.player.GetComponent<PlayerMovement>().TeleportPlayer(resetTrans.position);
        await Task.Delay(50);
        await HandleFade();
        _gM.playerData.canMove = true;
    }
    private async Task CreateFadeObject()
    {
        GameObject newObject = new GameObject("MyObject");
        RectTransform rectTransform = newObject.AddComponent<RectTransform>();//
        newObject.AddComponent<CanvasRenderer>();
        newObject.AddComponent<Canvas>().renderMode=RenderMode.ScreenSpaceOverlay;
        newObject.AddComponent<CanvasScaler>().referenceResolution=new Vector2(1920,1080);
        _fade= newObject.AddComponent<Image>();
        _fade.color = new Color(Color.black.r,Color.black.g,Color.black.b,0);
        rectTransform.anchoredPosition = new Vector2(.5f, .5f);
        rectTransform.sizeDelta = new Vector2(10000, 10000);
    }

    private async Task HandleFade()
    {
        float currentTime = 0;
        if (_fade)
        {
            var clear = new Color(0, 0, 0, 0);
            while (currentTime<1)
            {
                var time = Time.deltaTime;
                currentTime += time;
                _fade.color=Color.Lerp(_fade.color, clear, time * 1.2f);
                await Task.Yield();
            }
            _fade.color = clear;
            Destroy(_fade.gameObject);
        }
        else
        {
            await CreateFadeObject();
            while (currentTime<1)
            {
                var time = Time.deltaTime;
                currentTime += time;
                _fade.color=Color.Lerp(_fade.color, Color.black, time * 1.2f);
                await Task.Yield();
            }

            _fade.color = Color.black;
        }
    }
}
