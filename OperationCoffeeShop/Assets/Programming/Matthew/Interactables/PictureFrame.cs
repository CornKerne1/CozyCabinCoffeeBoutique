using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PictureFrame : Interactable
{
    // Start is called before the first frame update
    [SerializeField] private GameObject picture;
    [SerializeField] private GameObject uiPref;
    private GameObject ui;
    private int currentPic;
    private static readonly int EmissionMap = Shader.PropertyToID("_EmissionMap");

    public void ChangePicture(int i)
    {
        MeshRenderer meshRenderer = picture.GetComponent<MeshRenderer>();
        meshRenderer.material.mainTexture = gameMode.gameModeData.screenShots[i];
        meshRenderer.material.SetTexture(EmissionMap, gameMode.gameModeData.screenShots[i]);
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        base.playerInteraction = interaction;
        interaction.Carry(gameObject);
    }

    public override void OnAltInteract(PlayerInteraction interaction)
    {
        if (interaction.carriedObj == this.gameObject)
        {
            if (!ui)
            {
                gameMode.playerData.canMove = false;
                gameMode.playerData.inUI = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                ui = Instantiate(uiPref);
                ui.GetComponent<PicturePickerUI>().physicalRef = this;
                interaction.playerInput.ToggleHud();
                for (int i = 0; i <= 10; i++)
                {
                    transform.LookAt(interaction.transform);
                    //Thread.Sleep(1);
                }
            }
            else
            {
                DestroyUI();
                interaction.playerInput.ToggleHud();
            }
        }
    }

    private void DestroyUI()
    {
        gameMode.playerData.canMove = true;
        gameMode.playerData.inUI = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Destroy(ui);
    }
    
}