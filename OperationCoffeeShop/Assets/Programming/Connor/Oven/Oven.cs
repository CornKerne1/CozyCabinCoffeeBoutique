using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : Interactable
{
    public Animator ovenAnimator;

    string isOpen = "isOpen";

    private bool canOpen =true;

    public override void OnFocus()
    {
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        StartCoroutine(interact());
    }

    IEnumerator interact()
    {
        if (canOpen)
        {
            canOpen = false;
            if (ovenAnimator.GetBool(isOpen) == true)
            {
                ovenAnimator.SetBool(isOpen, false);
            }
            else
            {
                ovenAnimator.SetBool(isOpen, true);
            }
            yield return new WaitForSeconds(1);
            canOpen = true;
        }
        yield return null;
    }

    public override void OnLoseFocus()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
