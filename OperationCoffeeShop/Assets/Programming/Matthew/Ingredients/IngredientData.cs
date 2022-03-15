using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientData", menuName = "Ingredient/Generic")]
public class IngredientData : ScriptableObject
{
    [SerializeField] public GameObject milk;
    [SerializeField] public GameObject sMilk;
    [SerializeField] public GameObject fMilk;
    [SerializeField] public GameObject Sugar;
    [SerializeField] public GameObject wCream;
    [SerializeField] public GameObject espressoShot;
    [SerializeField] public GameObject salt;
    [SerializeField] public GameObject lCoffee;
    [SerializeField] public GameObject mCoffee;
    [SerializeField] public GameObject dCoffee;
    [SerializeField] public GameObject brewedCoffee;
    [SerializeField] public GameObject glCoffee;
    [SerializeField] public GameObject gmCoffee;
    [SerializeField] public GameObject gdCoffee;
}
