using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayCube : MachineInteraction
{
    [FormerlySerializedAs("PlayCubeAnimator")]
    public Animator playCubeAnimator;

    public GameObject gameDisk;

    private const string IsOpen = "isOpen";

    private bool _canOpen = true;
    private static readonly int Open = Animator.StringToHash(IsOpen);

    public bool hasDisk;

    [FormerlySerializedAs("_tvRenderer")] [SerializeField]
    private Renderer tvRenderer;

    [FormerlySerializedAs("_screenSaverTexture")] [SerializeField]
    private Texture screenSaverTexture;

    public Texture gameTexture;
    private static readonly int Emission = Shader.PropertyToID("_emission");
    private static readonly int Noise = Shader.PropertyToID("_Noise");

    private DiskInteractable _diskInteractable;

    public override void Start()
    {
        base.Start();
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        AkSoundEngine.PostEvent("PLAY_TVSTATIC", gameObject);
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        StartCoroutine(CO_Interact());
    }

    private IEnumerator CO_Interact()
    {
        if (_canOpen)
        {
            _canOpen = false;
            playCubeAnimator.SetBool(Open, playCubeAnimator.GetBool(Open) != true);

            yield return new WaitForSeconds(1);
            _canOpen = true;
        }

        if (!playCubeAnimator.GetBool(Open) && hasDisk)
        {
            tvRenderer.material.SetTexture(Emission, gameTexture);
            tvRenderer.material.SetFloat(Noise, 1000);
            AkSoundEngine.PostEvent("STOP_TVSTATIC", gameObject);

            _diskInteractable.PlayMusic();
        }
        else
        {
            if (hasDisk)
            {
                _diskInteractable.StopMusic();
                AkSoundEngine.PostEvent("PLAY_TVSTATIC", gameObject);
            }

            tvRenderer.material.SetTexture(Emission, screenSaverTexture);
            tvRenderer.material.SetFloat(Noise, 200);
        }

        yield return null;
    }

    public void DiskInteract(DiskInteractable disk)
    {
        _diskInteractable = disk;
        hasDisk = true;
        var o = disk.gameObject;
        o.transform.position = gameDisk.transform.position;
        o.transform.rotation = gameDisk.transform.rotation;
        o.GetComponent<Rigidbody>().isKinematic = true;
    }


    public override void ShowOnScreenText()
    {
        if (!hasDisk)
            base.ShowOnScreenText();
    }
}