using UnityEngine;

public class ComputerShopInteractable : Interactable
{
    [SerializeField]private GameObject shopUI;
    private GameObject _shopRef;

    public override void Start()
    {
        base.Start();
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        if (_shopRef)
        {
            _shopRef.SetActive(true);
            gM.pD.canMove = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            _shopRef = Instantiate(shopUI);
            _shopRef.SetActive(true);
            gM.pD.canMove = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}