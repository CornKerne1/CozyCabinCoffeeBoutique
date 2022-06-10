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
        if (TryGetComponent<Outline>(out var outline))
        {
            this.outline = outline;
            outlineColor = outline.OutlineColor;
            var color = outlineColor;
            color.a = 0;
            outline.OutlineColor = color ;
        }
    }
    public abstract void OnInteract(PlayerInteraction pI);
    public abstract void OnFocus();
    public abstract void OnLoseFocus();
    public virtual void OnAltInteract(PlayerInteraction pI){}
    public virtual void OnDrop(){}
}
