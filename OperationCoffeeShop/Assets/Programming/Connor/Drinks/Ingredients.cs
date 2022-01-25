using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum  Ingredients 
{
    Milk = FlavorProfile.Sweet, Sugar = FlavorProfile.Sweet,
    EspressoShot = FlavorProfile.Bitter, CofeeBean = FlavorProfile.Bitter, GroundCoffeeBean = FlavorProfile.Bitter,
    Salt = FlavorProfile.Salty

}
public enum FlavorProfile
{
    Bitter,Sweet,Salty
}
