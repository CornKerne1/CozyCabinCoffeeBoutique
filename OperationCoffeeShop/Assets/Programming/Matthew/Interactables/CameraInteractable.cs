using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInteractable : Interactable
{
    private Camera _camera;
    public GameObject cameraUi;
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

    public override void Save(int gameNumber)
    {
        var position = new Vector3(.65f, -5.42f, -6f);
        var rotation = new Quaternion(0, -76.9f, 0, 0);
        if(delivered)
            gameMode.saveGameData.respawnables.Add(new RespawbableData(objTypeShop,position,rotation,0));
    }
}