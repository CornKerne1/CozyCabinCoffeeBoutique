using UnityEngine;

public class ComputerShopInteractable : Interactable
{
    [SerializeField]private GameObject shopUI;
    private GameObject _shopRef;

    public override void Start()
    {
        base.Start();
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        {
            _shopRef = Instantiate(shopUI);
            _shopRef.SetActive(true);
            gameMode.playerData.inUI = true;
            gameMode.playerInput.ToggleMovement();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}