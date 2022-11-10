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
    private GameObject _ui;
    private int _currentPic;
    public int currentPic;
    private static readonly int EmissionMap = Shader.PropertyToID("_EmissionMap");

    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (_ui)
        {
            transform.position = Vector3.Lerp(transform.position,
                Camera.main.transform.position + Camera.main.transform.forward * .1f, Time.deltaTime);
        }
    }

    public void ChangePicture(Texture2D t)
    {
        MeshRenderer meshRenderer = picture.GetComponent<MeshRenderer>();
        meshRenderer.material.mainTexture = t;
        meshRenderer.material.SetTexture(EmissionMap, t);
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        base.playerInteraction = interaction;
        interaction.Carry(gameObject);
    }

    public override void OnAltInteract(PlayerInteraction interaction)
    {
        if (!playerInteraction) playerInteraction = interaction;
        if (interaction.carriedObj != this.gameObject) return;
        if (_ui) return;
        gameMode.playerData.canMove = false;
        gameMode.playerData.inUI = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _ui = Instantiate(uiPref);
        _ui.GetComponent<PicturePickerUI>().physicalRef = this;
        interaction.playerInput.ToggleHud();

        for (int i = 0; i <= 10; i++)
            transform.LookAt(interaction.transform);
    }
    public void DestroyUI()
    {
        if (!_ui) return;
        playerInteraction.playerInput.ToggleHud();
        gameMode.playerData.canMove = true;
        gameMode.playerData.inUI = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Destroy(_ui);
    }
    
}