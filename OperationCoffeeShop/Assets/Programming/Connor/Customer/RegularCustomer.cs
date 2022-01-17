using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RegularCustomer : Customer
{
    public abstract DrinkData FavoriteDrink();
    public abstract List<DrinkData> DesirableDrinks();
}
