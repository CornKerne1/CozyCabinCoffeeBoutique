using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Interactable : MonoBehaviour
{
    public GameMode gameMode;
    public Vector3 rotateOffset;
    [SerializeField] private bool isBreakable;
    
    //Breakable
    [SerializeField] private GameObject breakablePrefab;
    private GameObject _breakableRef;
    private PlayerInteraction _pI;
    private Rigidbody _rb;
    List<GameObject> fragments = new List<GameObject>();

    private Outline _outline;
    private Color _outlineColor;

    public virtual void Awake()
    {
        gameObject.layer = 3;
    }

    public virtual void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        InitializeOutline();
        CheckTutorial();
        if (isBreakable)
        {
            _rb = GetComponent<Rigidbody>();
        }
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
        _pI = playerInteraction;
        _pI.Carry(gameObject);
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

    IEnumerator CO_FreezeForClipping()
    {
        if (!isBreakable) yield break;
        _rb.isKinematic = true;
        yield return new WaitForSeconds(.02f);
        var transform1 = transform;
        _breakableRef = Instantiate(breakablePrefab, transform1.position, transform1.rotation);
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(.02f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isBreakable) return;
        var speed = _rb.velocity.magnitude*10f;
        if (speed >= gameMode.gameModeData.breakSpeed)
        {
            StartCoroutine(CO_FreezeForClipping());
        }
    }
}