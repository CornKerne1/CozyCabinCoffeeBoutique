using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRotation : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed;
    [SerializeField]
    Vector3 rotationDirection = new Vector3();

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateSpeed * rotationDirection * Time.deltaTime);
    }
}
