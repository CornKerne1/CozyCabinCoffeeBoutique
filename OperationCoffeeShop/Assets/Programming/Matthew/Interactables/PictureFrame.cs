using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureFrame : Interactable
{
    // Start is called before the first frame update
    [SerializeField] private GameObject picture;
    public override void Start()
    {
        base.Start();
        StartCoroutine(WaitForStart());
    }

    private IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(.1f);
        MeshRenderer meshRenderer = picture.GetComponent<MeshRenderer>();
        picture.GetComponent<MeshRenderer>().material.mainTexture = gameMode.playerData.screenShots[Random.Range(0,gameMode.playerData.screenShots.Count)];
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        base.playerInteraction = interaction;
        interaction.Carry(gameObject);
    }

    public override void OnAltInteract(PlayerInteraction interaction)
    {
    }
}