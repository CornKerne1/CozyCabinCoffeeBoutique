using UnityEngine;
using UnityEngine.Serialization;

public class PhysicalIngredient : Interactable
{
    private Objectives _objectives;

    private readonly Vector3 _rejectionForce = new Vector3(55, 55, 55);
    [SerializeField] public Ingredients thisIngredient;

    [SerializeField] private bool inHand;
    public Machine machine;

    public override void Start()
    {
        base.Start();
        gameObject.tag = "PickUp";
        playerInteraction = gameMode.player.GetComponent<PlayerInteraction>();
    }


    public override void OnInteract(PlayerInteraction interaction)
    {
        IfTutorial();
        base.playerInteraction = interaction;
        base.playerInteraction.Carry(gameObject);
        inHand = true;
    }

    public override void OnDrop()
    {
        inHand = false;
    }

    private void IfTutorial()
    {
        if (gameMode.gameModeData.inTutorial)
        {
            gameMode.Tutorial.NextObjective(gameMode.Tutorial.Objectives.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var rb = GetComponent<Rigidbody>();
        if (other.gameObject.layer != 3) return;
        try
        {
            other.GetComponent<Machine>().IngredientInteract(gameObject);
            rb.AddForce(_rejectionForce);
            playerInteraction.DropCurrentObj();
        }
        catch
        {
            try
            {
                other.GetComponent<BrewerBowl>().IngredientInteract(gameObject);
                rb.AddForce(_rejectionForce);
                playerInteraction.DropCurrentObj();
            }
            catch
            {
                // ignored
            }
        }
    }
}