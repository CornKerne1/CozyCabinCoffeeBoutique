using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderThoughts : MonoBehaviour
{
    public Sprite Sugar;
    public Sprite BrewedCoffee;
    public Sprite Milk;
    public Sprite Esspresso;


    Customer customer;
    DrinkData drink;
    List<IngredientNode> ingredients;

    Image[] images;

    GameMode GM;
    Transform player;

    public Transform pivot;

    // Start is called before the first frame update
    private void Start()
    {
        customer = GetComponentInParent<Customer>();
        images = GetComponentsInChildren<Image>();
        StartCoroutine(C0_GetOrderIngredients());
        GM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        player = GM.player;
    }

    private void Update()
    {
        Transform t = pivot.gameObject.transform;
        t.LookAt(player);
        t.rotation = new Quaternion(0, player.rotation.y, player.rotation.z, player.rotation.w);
        this.gameObject.transform.rotation = t.rotation;
    }

    private IEnumerator C0_GetOrderIngredients()
    {
        yield return new WaitForSeconds(.4f);
        drink = customer.GetDrinkOrder();
        ingredients = drink.Ingredients;
        var i = 1;
        foreach (var ingredientNode in ingredients)
        {
            Add(ingredientNode.ingredient, i);
            i++;
        }
    }


    private void Add(Ingredients ingredient, int position)
    {
        switch (ingredient)
        {
            case Ingredients.Sugar:
                images[position].sprite = Sugar;
                images[position].color = new Color(images[position].color.r, images[position].color.g,
                    images[position].color.b, 1);
                break;
            case Ingredients.BrewedCoffee:
                images[position].sprite = BrewedCoffee;
                images[position].color = new Color(images[position].color.r, images[position].color.g,
                    images[position].color.b, 1);
                break;
            case Ingredients.Milk:
                images[position].sprite = Milk;
                images[position].color = new Color(images[position].color.r, images[position].color.g,
                    images[position].color.b, 1);
                break;
            case Ingredients.Espresso:
                images[position].sprite = Esspresso;
                images[position].color = new Color(images[position].color.r, images[position].color.g,
                    images[position].color.b, 1);
                break;
            case Ingredients.SteamedMilk:
            case Ingredients.FoamedMilk:
            case Ingredients.WhippedCream:
            case Ingredients.UngroundCoffee:
            case Ingredients.GroundCoffee:
            case Ingredients.Salt:
            case Ingredients.EspressoBeans:
            case Ingredients.CoffeeFilter:
            case Ingredients.TeaBag:
            default:
                images[position].sprite = null;
                images[position].color = new Color(images[position].color.r, images[position].color.g,
                    images[position].color.b, 0);
                break;
        }
    }
}