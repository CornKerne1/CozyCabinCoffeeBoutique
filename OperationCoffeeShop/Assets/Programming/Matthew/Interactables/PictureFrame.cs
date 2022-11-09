using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureFrame : Interactable
{
    // Start is called before the first frame update
    [SerializeField] private GameObject picture;
    [SerializeField] private GameObject uiPref;
    private int currentPic;

    public void ChangePicture(int i)
    {
        MeshRenderer meshRenderer = picture.GetComponent<MeshRenderer>();
        picture.GetComponent<MeshRenderer>().material.mainTexture = gameMode.playerData.screenShots[i];
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
            StartCoroutine(CO_LookAt(0,interaction.transform));
            gameMode.playerData.canMove = false;
            gameMode.playerData.inUI = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            var ui = Instantiate(uiPref);
            ui.GetComponent<PicturePickerUI>().physicalRef = this;
        }
    }

    IEnumerator CO_LookAt(int i, Transform t)
    {
        transform.LookAt(t);
        StartCoroutine(CO_DualTwo(i,t));
        yield return null;
    }

    private IEnumerator CO_DualTwo(int i, Transform t)
    {
        if (i > 50) yield return null;
        yield return new WaitForSeconds(.02f);
        StartCoroutine(CO_LookAt(0,t));
    }
}