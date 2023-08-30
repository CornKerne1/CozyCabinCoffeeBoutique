using UnityEngine;

public class PianoKey : MonoBehaviour
{
    private int numCollisions = 0;
    private Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        // Store the initial rotation of the PKey
        initialRotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only change the rotation if this is the first object colliding with the PKey
        if (numCollisions > 0)
        {
            transform.rotation = Quaternion.Euler(3.75f, 0f, 0f);
            AkSoundEngine.PostEvent("PLAY_SFX_BELLCHIME", gameObject);
        }

        // Increment the number of collisions
        numCollisions++;
    }

    private void OnTriggerExit(Collider other)
    {
        // Decrement the number of collisions
        numCollisions--;

        // Reset the rotation if there are no more collisions
        if (numCollisions == 0)
        {
            transform.rotation = initialRotation;
        }
    }
}
