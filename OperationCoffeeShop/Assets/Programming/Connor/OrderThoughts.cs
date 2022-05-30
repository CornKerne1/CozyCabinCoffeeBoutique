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

    // Start is called before the first frame update
    void Start()
    {
        customer = GetComponentInParent<Customer>();
        images = GetComponentsInChildren<Image>();
        drink = customer.GetDrinkOrder();
        ingredients = drink.Ingredients;
        int i = 1;
        foreach (IngredientNode IN in ingredients)
        {
            Add(IN.ingredient, i);
            i++;
        }
        GM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        player = GM.player;
    }


    private void Add(Ingredients ingredient, int position)
    {
        switch (ingredient)
        {
            case Ingredients.Sugar:
                images[position].sprite = Sugar;
                break;
            case Ingredients.BrewedCoffee:
                images[position].sprite = BrewedCoffee;
                break;
            case Ingredients.Milk:
                images[position].sprite = Milk;
                break;
            case Ingredients.Espresso:
                images[position].sprite = Esspresso;
                break;
            default:
                images[position].sprite = null;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Transform t = this.gameObject.transform;
        t.LookAt(player);
        t.rotation = new Quaternion(0, player.rotation.y, player.rotation.z, player.rotation.w);
        this.gameObject.transform.rotation = t.rotation;
    }
}
