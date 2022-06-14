using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public GameMode gM;
    public Vector3 rotateOffset;

    protected Outline outline;
    protected Color outlineColor;

    public virtual void Awake()
    {
        gameObject.layer = 3;
    }

    public virtual void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        outline = gameObject.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineWidth = 10;
            var sunsetYellow = new Color()
            {
                r = 1f,
                g = .7882f,
                b = .1333f,
                a = 1f,
            };
            outlineColor = sunsetYellow;
            var color = outlineColor;
            color.a = 0;
            outline.OutlineColor = color;

    }

    public abstract void OnInteract(PlayerInteraction playerInteraction);

    public virtual void OnFocus()
    {
        if (!outline) return;
        outline.OutlineColor = outlineColor;
    }

    public virtual void OnLoseFocus()
    {
        if (!outline) return;
        var color = outlineColor;
        color.a = 0;
        outline.OutlineColor = color;
    }

    public virtual void OnAltInteract(PlayerInteraction playerInteraction)
    {
    }

    public virtual void OnDrop()
    {
    }
}