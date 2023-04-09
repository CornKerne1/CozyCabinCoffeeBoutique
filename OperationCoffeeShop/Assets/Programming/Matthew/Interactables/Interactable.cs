using System;
using System.Collections;
using System.Diagnostics.Eventing.Reader;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public abstract class Interactable : MonoBehaviour,ISaveState
{
    public GameMode gameMode;
    public Vector3 rotateOffset;
    [SerializeField] protected bool isBreakable;
    public string breakableSoundEngineEvent = "PLAY_CERAMICBOWLBREAKING";

    //Breakable
    [SerializeField] protected GameObject breakablePrefab;
    [SerializeField] protected Color smashColor;
    [SerializeField] protected Color smashEmissionColor;
    protected GameObject _breakableRef;
    [FormerlySerializedAs("_pI")] public PlayerInteraction playerInteraction;
    protected Rigidbody _rB;
    protected bool _isWaiting, _isBroken;
    public bool hasOnScreenText;
    private Outline _outline;
    private Color _outlineColor;
    protected ObjectPool<Canvas> _poolOfOnScreenPrompt;
    protected TextMeshProUGUI varOnScreenPromt;
    [SerializeField] protected Canvas onScreenPrompt;
    public string onScreenPromptText;
    protected bool lookAtPlayer;
    private float speed;
    public bool delivered;
    private bool _respawnable;
    public Dispenser dispenser;
    public DeliveryManager.ObjType objTypeShop;
    void OnEnable() => Load(0);
    void OnSaveEvent(object sender, EventArgs e) => Save(0);
    public virtual void Awake()
    {
        gameObject.layer =3 ;
        _rB = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (!_rB) return;
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
        OnFocusTextPool();
        gameMode.dynamicBatcher.AddForBatching(gameObject);
        SaveSystem.SaveGameEvent += OnSaveEvent;
    }
    private void OnFocusTextPool()
    {
        _poolOfOnScreenPrompt = new ObjectPool<Canvas>(
            () => Instantiate(onScreenPrompt, gameObject.transform),
            prompt => { prompt.gameObject.SetActive(true); },
            prompt => { prompt.gameObject.SetActive(false); }, Destroy, true, 10, 10);
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
        if (!gameMode.gameModeData.inTutorial) return;
        Debug.Log("Interactable tutorial object: " + gameObject);
        gameMode.Tutorial.AddedGameObject(gameObject);
    }

    public virtual void OnInteract(PlayerInteraction interaction)
    {
        playerInteraction = interaction;
        playerInteraction.Carry(gameObject);
    }

    public virtual void OnFocus()
    {
        if (hasOnScreenText) ShowOnScreenText();
        if (!_outline) return;
        _outline.enabled = true;
        _outline.OutlineColor = _outlineColor;
    }

    public virtual void ShowOnScreenText()
    {
        if (varOnScreenPromt) return;
        varOnScreenPromt = _poolOfOnScreenPrompt.Get().GetComponentInChildren<TextMeshProUGUI>();
        varOnScreenPromt.text = onScreenPromptText;
    }

    public virtual void OnLoseFocus()
    {
        if (hasOnScreenText) HideOnScreenText();
        if (!_outline) return;
        var color = _outlineColor;
        color.a = 0;
        _outline.OutlineColor = color;
        if (gameObject.activeSelf)
            StartCoroutine(CO_DisableOutline());
    }

    protected virtual void HideOnScreenText()
    {
        if (!varOnScreenPromt) return;
        _poolOfOnScreenPrompt.Release(varOnScreenPromt.GetComponentInParent<Canvas>());
        varOnScreenPromt = null;
    }

    private IEnumerator CO_DisableOutline()
    {
        yield return new WaitForSeconds(.01f);
        var color = _outlineColor;
        color.a = 0;
        _outline.OutlineColor = color;
        _outline.enabled = false;
    }

    public virtual void OnAltInteract(PlayerInteraction interaction)
    {
    }

    public virtual void OnDrop()
    {
        playerInteraction = null;
    }

    protected virtual void LookAtPlayer()
    {
        if (!playerInteraction) return;
        StartCoroutine(LookAt(playerInteraction));
    }

    protected virtual async Task FreezeForClippingAsync()
    {
        if (!isBreakable||_isBroken) return;
        _isBroken = true;
        _rB.isKinematic = true;
        await Task.Delay(10);
        AkSoundEngine.PostEvent(breakableSoundEngineEvent, gameObject);
        _breakableRef = Instantiate(breakablePrefab, transform.position, transform.rotation);
        await gameMode.dynamicBatcher.AddForBatching(_breakableRef);
        var particle = Instantiate(gameMode.gameModeData.breakParticle, transform.position, transform.rotation);
        particle.GetComponent<ParticleSystemRenderer>().material.color = smashColor;
        particle.GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", smashEmissionColor);
        foreach (var obj in _breakableRef.GetChildren(transform))
        {
            obj.GetComponent<Rigidbody>().AddForce(Vector3.up * 10f);
        }

        float timeElapsed = 0;
        gameMode.Surprise(gameObject);
        var colliders = GetComponentsInChildren<Collider>();
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var c in colliders)
            c.enabled = false;
        foreach (var rE in renderers)
            rE.enabled = false;
        await Task.Delay(20);
        if (TryGetComponent<Radio>(out var r))
        {
            foreach (var rC in r.radioChannels)
            {
                rC.StopChannel();
                Destroy(rC.gameObject);
            }
        }
        while (timeElapsed < 5f)
        {
            timeElapsed += Time.deltaTime;
            await Task.Yield();
        }
        DestroyImmediate(particle);
        Destroy(gameObject);
    }

    private IEnumerator LookAt(PlayerInteraction interaction)
    {
        while (lookAtPlayer)
        {
            if (gameMode.camera) transform.LookAt(gameMode.camera.transform);
            yield return null;
        }
    }
    
    private async void OnCollisionEnter(Collision collision)
    {
        if (_isBroken) return;
        if (collision.gameObject.TryGetComponent<LiquidIngredients>(out _)) return;
        try
        {
            if (!gameMode)
                gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
            if (!(speed >= gameMode.gameModeData.breakSpeed)) return;
            {
                FreezeForClippingAsync();
                collision.gameObject.GetComponent<Interactable>().FreezeForClippingAsync();
            }
        }
        catch
        {
            // ignored
        }
    }

    public virtual void Save(int gameNumber)
    {
    }

    public virtual void Load(int gameNumber)
    {
    }
}