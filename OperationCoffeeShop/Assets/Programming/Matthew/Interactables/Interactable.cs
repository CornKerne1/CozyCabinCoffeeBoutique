using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Interactable : MonoBehaviour
{
    public GameMode gameMode;
    public Vector3 rotateOffset;
    [SerializeField] private bool isBreakable;
    public string breakableSoundEngineEnvent = "PLAY_CERAMICBOWLBREAKING";

    //Breakable
    [SerializeField] private GameObject breakablePrefab;
    private GameObject _breakableRef;
    [FormerlySerializedAs("_pI")] public PlayerInteraction playerInteraction;
    private Rigidbody _rB;
    private bool _isWaiting, _isBroken;

    private Outline _outline;
    private Color _outlineColor;

    private float speed;

    public virtual void Awake()
    {
        gameObject.layer = 3;
        _rB = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (_rB.velocity.magnitude * 10 > speed)
        {
            speed = _rB.velocity.magnitude * 10f;
        }
        else if (_rB.velocity.magnitude < 1)
        {
            speed = 0;
        }
    }

    public virtual void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        InitializeOutline();
        CheckTutorial();
    }

    private void InitializeOutline()
    {
        _outline = gameObject.AddComponent<Outline>();
        _outline.enabled = true;
        _outline.OutlineMode = Outline.Mode.OutlineVisible;
        _outline.OutlineWidth = 10;
        var sunsetYellow = new Color()
        {
            r = 1f,
            g = .7882f,
            b = .1333f,
            a = 1f,
        };
        _outlineColor = sunsetYellow;
        var color = _outlineColor;
        color.a = 0;
        _outline.OutlineColor = color;
        StartCoroutine(CO_DisableOutline());
    }

    protected virtual void CheckTutorial()
    {
        if (gameMode.gameModeData.inTutorial)
        {
            Debug.Log("Interactable tutorial object: " + gameObject);
            gameMode.Tutorial.AddedGameObject(gameObject);
        }
    }

    public virtual void OnInteract(PlayerInteraction playerInteraction)
    {
        if (!isBreakable) return;
        this.playerInteraction = playerInteraction;
        this.playerInteraction.Carry(gameObject);
    }

    public virtual void OnFocus()
    {
        if (!_outline) return;
        _outline.enabled = true;
        _outline.OutlineColor = _outlineColor;
    }

    public virtual void OnLoseFocus()
    {
        if (!_outline) return;
        var color = _outlineColor;
        color.a = 0;
        _outline.OutlineColor = color;
        if (gameObject.activeSelf)
            StartCoroutine(CO_DisableOutline());
    }

    private IEnumerator CO_DisableOutline()
    {
        yield return new WaitForSeconds(.01f);
        _outline.enabled = false;
    }

    public virtual void OnAltInteract(PlayerInteraction playerInteraction)
    {
    }

    public virtual void OnDrop()
    {
    }

    private IEnumerator CO_FreezeForClipping()
    {
        if (!isBreakable) yield break;
        _rB.isKinematic = true;
        yield return new WaitForSeconds(.02f);
        var transform1 = transform;
        AkSoundEngine.PostEvent(breakableSoundEngineEnvent, gameObject);
        _breakableRef = Instantiate(breakablePrefab, transform1.position, transform1.rotation);

        foreach (var obj in _breakableRef.GetChildren(transform))
        {
            obj.GetComponent<Rigidbody>().AddForce(Vector3.up*10f);
        }
        gameMode.Surprise(gameObject);
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(.02f);
        if (TryGetComponent<Radio>(out var r))
        {
            foreach (var rC in r.radioChannels)
                rC.StopChannel();
        }

        Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (_isBroken) return;
        if(collision.gameObject.TryGetComponent<LiquidIngredients>(out _)) return;
        try
        {
            if (!gameMode)
                gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
            if (!(speed >= gameMode.gameModeData.breakSpeed)) return;
            {
                _isBroken = true;
                StartCoroutine(CO_FreezeForClipping());
                StartCoroutine(collision.gameObject.GetComponent<Interactable>().CO_FreezeForClipping());
            }
        }
        catch
        {
            // ignored
        }

    }
}