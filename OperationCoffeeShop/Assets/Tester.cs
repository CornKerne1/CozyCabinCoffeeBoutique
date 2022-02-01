using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    // Start is called before the first frame update
    public int number;
    public int number2;

    public int ChangeNumber()
    {
        return number = 1 + number;
    }

    public void ChangeNumber2()
    {
        number2 = 1 + number2;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(ChangeNumber());
    }
}
