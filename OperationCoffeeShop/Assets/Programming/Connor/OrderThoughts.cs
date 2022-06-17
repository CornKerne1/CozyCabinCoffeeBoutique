using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OrderThoughts : MonoBehaviour
{
    [FormerlySerializedAs("Sugar")] public Sprite sugar;
    [FormerlySerializedAs("BrewedCoffee")] public Sprite brewedCoffee;
    [FormerlySerializedAs("Milk")] public Sprite milk;
    [FormerlySerializedAs("Esspresso")] public Sprite esspresso;


    private Customer _customer;
    private DrinkData _drink;
    private List<IngredientNode> _ingredients;

    private Image[] _images;

    private GameMode _gameMode;
    private Transform _player;

    public Transform pivot;

    private void Start()
    {
        _customer = GetComponentInParent<Customer>();
        _images = GetComponentsInChildren<Image>();
        StartCoroutine(C0_GetOrderIngredients());
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        _player = _gameMode.player;
    }

    private void Update()
    {
        var t = pivot.gameObject.transform;
        t.LookAt(_player);
        var rotation = _player.rotation;
        t.rotation = new Quaternion(0, rotation.y, rotation.z, rotation.w);
        this.gameObject.transform.rotation = t.rotation;
    }

    private IEnumerator C0_GetOrderIngredients()
    {
        yield return new WaitForSeconds(.4f);
        _drink = _customer.GetDrinkOrder();
        _ingredients = _drink.ingredients;
        var i = 1;
        foreach (var ingredientNode in _ingredients)
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
                _images[position].sprite = sugar;
                _images[position].color = new Color(_images[position].color.r, _images[position].color.g,
                    _images[position].color.b, 1);
                break;
            case Ingredients.BrewedCoffee:
                _images[position].sprite = brewedCoffee;
                _images[position].color = new Color(_images[position].color.r, _images[position].color.g,
                    _images[position].color.b, 1);
                break;
            case Ingredients.Milk:
                _images[position].sprite = milk;
                _images[position].color = new Color(_images[position].color.r, _images[position].color.g,
                    _images[position].color.b, 1);
                break;
            case Ingredients.Espresso:
                _images[position].sprite = esspresso;
                _images[position].color = new Color(_images[position].color.r, _images[position].color.g,
                    _images[position].color.b, 1);
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
                _images[position].sprite = null;
                _images[position].color = new Color(_images[position].color.r, _images[position].color.g,
                    _images[position].color.b, 0);
                break;
        }
    }
}