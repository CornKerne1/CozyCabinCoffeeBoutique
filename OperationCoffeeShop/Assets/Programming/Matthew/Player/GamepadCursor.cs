using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class GamepadCursor : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private GameObject cursorObject;

    public PlayerInput playerInput;

    private RectTransform _virtualCursorRectTransform;
    private RectTransform _cursorTransform;
    private float _cursorHalfWidth;
    private float _cursorHalfHeight;
    private float _screenWidth;
    private float _screenHeight;
    private GameObject _currentButton;

    private void Start()
    {
        if (playerInput.inputType.Contains("Mouse"))
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }

        Cursor.lockState = CursorLockMode.None;
        _virtualCursorRectTransform = cursorObject.GetComponent<RectTransform>();
        var sizeDelta = _virtualCursorRectTransform.sizeDelta;
        _cursorHalfWidth = sizeDelta.x / 2f;
        _cursorHalfHeight = sizeDelta.y / 2f;
        _screenWidth = Screen.width;
        _screenHeight = Screen.height;
        PlayerInput.InteractEvent += OnClickAction;
    }

    private void OnEnable()
    {
        if (_virtualCursorRectTransform)
        {
            _virtualCursorRectTransform.position = Mouse.current.position.ReadValue();
        }
    }

    private void Update()
{
    if (!playerInput) return;

    // Store the previous button for pointer exit
    GameObject previousButton = _currentButton;

    // Raycast to find the button under the cursor
    PointerEventData eventData = new PointerEventData(EventSystem.current);
    eventData.position = _virtualCursorRectTransform.position;
    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(eventData, results);

    // Find the button with the highest priority
    _currentButton = null;
    foreach (RaycastResult result in results)
    {
        UnityEngine.UI.Button button = result.gameObject.GetComponent<UnityEngine.UI.Button>();
        if (button != null)
        {
            if (_currentButton == null || result.sortingOrder > results[0].sortingOrder)
            {
                _currentButton = result.gameObject;
            }
        }
    }

    // Call pointer enter and pointer exit handlers
    if (previousButton != _currentButton)
    {
        if (previousButton != null)
        {
            ExecuteEvents.Execute(previousButton, eventData, ExecuteEvents.pointerExitHandler);
        }

        if (_currentButton != null)
        {
            ExecuteEvents.Execute(_currentButton, eventData, ExecuteEvents.pointerEnterHandler);
        }
    }

    // Move the virtual cursor
    if (playerInput.inputType.Contains("Stick"))
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _virtualCursorRectTransform.gameObject.SetActive(true);
        Vector3 currentPosition = _virtualCursorRectTransform.position;
        Vector3 targetPosition = currentPosition +
                                 new Vector3(playerInput.GetMouseX(), playerInput.GetMouseY(), 0) *
                                 (speed * Time.deltaTime);
        targetPosition.x = Mathf.Clamp(targetPosition.x, _cursorHalfWidth, _screenWidth - _cursorHalfWidth);
        targetPosition.y = Mathf.Clamp(targetPosition.y, _cursorHalfHeight, _screenHeight - _cursorHalfHeight);
        _virtualCursorRectTransform.position = targetPosition;
    }
    else if (playerInput.inputType.Contains("Mouse"))
    {
        // Reset virtual cursor position to mouse position
        _virtualCursorRectTransform.position = Mouse.current.position.ReadValue();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _virtualCursorRectTransform.gameObject.SetActive(false);
    }
}
    private void OnClickAction(object sender, EventArgs e)
    {
        if (_currentButton != null)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = _virtualCursorRectTransform.position;
            ExecuteEvents.Execute(_currentButton, eventData, ExecuteEvents.pointerClickHandler);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}