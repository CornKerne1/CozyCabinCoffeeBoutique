using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Interactable : MonoBehaviour
{
    [FormerlySerializedAs("gM")] public GameMode gameMode;
    public Vector3 rotateOffset;

    private Outline _outline;
    private Color _outlineColor;

    public virtual void Awake()
    {
        gameObject.layer = 3;
    }

    public virtual void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        _outline = gameObject.AddComponent<Outline>();
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
    }

    public abstract void OnInteract(PlayerInteraction playerInteraction);

    public virtual void OnFocus()
    {
        if (!_outline) return;
        _outline.OutlineColor = _outlineColor;
    }

    public virtual void OnLoseFocus()
    {
        if (!_outline) return;
        var color = _outlineColor;
        color.a = 0;
        _outline.OutlineColor = color;
    }

    public virtual void OnAltInteract(PlayerInteraction playerInteraction)
    {
    }

    public virtual void OnDrop()
    {
    }
}