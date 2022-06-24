using UnityEngine;
using UnityEngine.Serialization;

public class PhysicalIngredient : Interactable
{
    private Objectives1 _objectives1;

    private readonly Vector3 _rejectionForce = new Vector3(55, 55, 55);
    [SerializeField] public Ingredients thisIngredient;

    [FormerlySerializedAs("_inHand")] [SerializeField]
    private bool inHand;

    public PlayerInteraction pI;

    public override void Start()
    {
        base.Start();
        gameObject.tag = "PickUp";
    }


    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        pI = playerInteraction;
        playerInteraction.Carry(gameObject);
        inHand = true;
    }

    public override void OnDrop()
    {
        inHand = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        var rb = GetComponent<Rigidbody>();
        if (other.gameObject.layer != 3) return;
        try
        {
            other.GetComponent<Machine>().IngredientInteract(gameObject);
            rb.AddForce(_rejectionForce);
            pI.DropCurrentObj();
        }
        catch
        {
            try
            {
                other.GetComponent<BrewerBowl>().IngredientInteract(gameObject);
                rb.AddForce(_rejectionForce);
                pI.DropCurrentObj();
            }
            catch
            {
                try
                {
                    other.GetComponent<TutorialBrewerBowl>().IngredientInteract(gameObject);
                    rb.AddForce(_rejectionForce);
                    pI.DropCurrentObj();
                }
                catch
                {
                    // ignored
                }            }
        }
    }
}