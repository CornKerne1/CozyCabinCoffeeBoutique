using UnityEngine;


public class DeliveryZone : MonoBehaviour
{
    public CustomerLine line;


    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out IngredientContainer drinkContainer) && drinkContainer.inHand == false)
        {
            line.DeliverDrink(other.gameObject);
        }
    }
}