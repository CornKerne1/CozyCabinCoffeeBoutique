using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class CupDispenser : Dispenser
{
    private ObjectPool<IngredientContainer> _pool;

    public override void Start()
    {
        base.Start();
        ComputerShop.DepositItems += AddItems;
        _pool = new ObjectPool<IngredientContainer>(
            () => Instantiate(objType.gameObject.GetComponentInChildren<IngredientContainer>()),
            ingredientContainer =>
            {
                ingredientContainer.ResetCup();
                ingredientContainer.gameObject.SetActive(true);
            },
            ingredientContainer => { ingredientContainer.gameObject.SetActive(false); }, Destroy, true, 100, 100);
        if (bottomless) return;
        try
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
            if (!bottomless)
                text.text = quantity + message;
            else text.text = "Bottomless";
        }
        catch
        {
            text = null;
        }
    }

    public void ReleasePoolObject(IngredientContainer ingredientContainer)
    {
        _pool.Release(ingredientContainer);
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        if (playerInteraction.playerData.busyHands || (!bottomless && quantity <= 0)) return;
        quantity--;
        UpdateQuantity();
        var ingredientContainer = _pool.Get();
        var transform1 = ingredientContainer.transform;
        transform1.position = spawnTrans.position;
        transform1.rotation = spawnTrans.rotation;

        ingredientContainer.pI = playerInteraction;
        ingredientContainer.inHand = true;
        ingredientContainer.dispenser = this;

        playerInteraction.Carry(ingredientContainer.gameObject);
        IfTutorial();
    }
}