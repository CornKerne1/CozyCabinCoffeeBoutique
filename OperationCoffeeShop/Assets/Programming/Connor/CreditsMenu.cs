using UnityEngine;

public class CreditsMenu : MonoBehaviour
{
    public GameObject creditsScreen;

    public void CloseCredits()
    {
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
        Destroy(creditsScreen);
    }
}