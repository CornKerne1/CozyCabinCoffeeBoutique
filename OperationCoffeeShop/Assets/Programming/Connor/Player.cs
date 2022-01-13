using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;//To use the EventHandler you must INCLUDE


public class Player : MonoBehaviour
{
    public float mouseX;
    public float mouseY;
    public float vertical1D;
    public float horizontal1D;

    public PlayerData PD;

    public event EventHandler Event1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Input Methods
    public void OnMouseLook(InputAction.CallbackContext context)
    {

        mouseX = context.ReadValue<Vector2>().x * PD.mouseSensitivity;
        mouseY = context.ReadValue<Vector2>().y * PD.mouseSensitivity;
    }
    public void OnHorizontalMove(InputAction.CallbackContext context)
    {
        horizontal1D = context.ReadValue<float>();

    }
    public void OnVerticaleMove(InputAction.CallbackContext context)
    {
        vertical1D = context.ReadValue<float>();

    }
    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.performed) ;// please work 
           

    }
    public void OnRightClick(InputAction.CallbackContext context)
    {
        

    }
    public void OnPause(InputAction.CallbackContext context)
    {
        //if (pauseMenuRef == null && !dead)
        //{
        //    pauseMenuRef = Instantiate(pauseMenu);
        //    pm = pauseMenu.GetComponentInChildren<PauseMenu>();
        //    pm.PauseGame(this.gameObject);
        //}
    }
    #endregion
}
