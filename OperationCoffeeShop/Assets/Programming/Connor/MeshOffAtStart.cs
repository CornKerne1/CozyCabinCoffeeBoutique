using UnityEngine;

public class MeshOffAtStart : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}