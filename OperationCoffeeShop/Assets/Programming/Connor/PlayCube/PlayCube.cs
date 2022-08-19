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

    public override void Start()
    {
        base.Start();
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
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
            tvRenderer.material.SetFloat(Noise, 400 );

        }
        else
        {
            tvRenderer.material.SetTexture(Emission, screenSaverTexture);
            tvRenderer.material.SetFloat(Noise, 200 );
        }

        yield return null;
    }

    public void DiskInteract(GameObject disk)
    {
        hasDisk = true;
        disk.transform.position = gameDisk.transform.position;
        disk.transform.rotation = gameDisk.transform.rotation;
        disk.GetComponent<Rigidbody>().isKinematic = true;
    }
}