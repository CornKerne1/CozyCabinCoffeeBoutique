using UnityEngine;
using UnityEngine.Serialization;

public class DiskInteractable : RandomInteractable
{
    private readonly Vector3 _rejectionForce = new Vector3(100, 100, 100);

    public PlayCube playCube;

    private Rigidbody _rigidbody;

    [FormerlySerializedAs("_tvEmissionTexture")] [SerializeField]private Texture tvEmissionTexture;

    public override void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        base.playerInteraction = interaction;
        interaction.Carry(gameObject);
        if (!playCube) return;
        playCube.hasDisk = false;
        playCube = null;
    }

    public override void OnAltInteract(PlayerInteraction interaction)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        var rb = GetComponent<Rigidbody>();
        if (other.gameObject.layer != 3) return;
        try
        {
            playCube = other.GetComponentInParent<PlayCube>();

            if (!playCube.hasDisk && playCube.playCubeAnimator.GetBool("isOpen"))
            {
                playCube.DiskInteract(gameObject);
                playCube.gameTexture = tvEmissionTexture;
                playerInteraction.DropCurrentObj();
            }
            else
            {
                playerInteraction.DropCurrentObj();
                _rigidbody.AddForce(_rejectionForce);
                playCube = null;
            }
        }
        catch
        {
            //ignore
        }
    }
}