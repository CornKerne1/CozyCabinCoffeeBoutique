using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInteractable : Interactable
{
    private Camera _camera;
    [SerializeField] private GameObject cameraUi;
    [SerializeField] private GameObject apertureUi;
    private GameObject uiRef;
    private MeshRenderer _meshRenderer;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    public override void OnInteract(PlayerInteraction interaction)
    {
        base.playerInteraction = interaction;
        interaction.Carry(gameObject);
        lookAtPlayer = true;
        LookAtPlayer();
    }
    public override void OnAltInteract(PlayerInteraction interaction)
    {
        _meshRenderer.enabled = ! _meshRenderer.enabled;
        interaction.playerInput.FreeCam();
        if (interaction.playerData.camMode)
        {
            uiRef = Instantiate(cameraUi);
        }
        else
            Destroy(uiRef);
    }
    
    public override void OnDrop()
    {
        base.OnDrop();
        lookAtPlayer = false;
    }

    public void TakePictureUI()
    {
        var apertureRef = Instantiate(apertureUi);
        StartCoroutine(CO_DestroyUI(apertureRef));
    }

    public void HideUI()//
    {
        if(playerInteraction)
            StartCoroutine(CO_HideUI());
    }

    private IEnumerator CO_DestroyUI(GameObject apertureRef)
    {
        yield return new WaitForSeconds(.375f);
        Destroy(apertureRef);
        apertureRef = null;
    }
    private IEnumerator CO_HideUI()
    {
        uiRef.SetActive(false);
        yield return new WaitForSeconds(.04f);
        uiRef.SetActive(true);
    }

    public override void Save(int gameNumber)
    {
        var position = new Vector3(.65f, -5.42f, -6f);
        var rotation = new Quaternion(0, -76.9f, 0, 0);
        if(delivered)
            gameMode.saveGameData.respawnables.Add(new RespawbableData(objTypeShop,position,rotation,0));
    }
}