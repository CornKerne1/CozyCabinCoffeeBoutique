using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadCursor : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Cursor movement speed
    [SerializeField] private GameObject cursorObject; // Reference to the cursor object

    [SerializeField] private PlayerInput playerInput; // Reference to the PlayerInput component

    private RectTransform cursorRectTransform;
    private float cursorHalfWidth;
    private float cursorHalfHeight;
    private float screenWidth;
    private float screenHeight;

    private void Start()
    {
        cursorRectTransform = cursorObject.GetComponent<RectTransform>();
        cursorHalfWidth = cursorRectTransform.sizeDelta.x / 2f;
        cursorHalfHeight = cursorRectTransform.sizeDelta.y / 2f;
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    private void OnEnable()
    {
        // Enable the movement action map
    }

    private void OnDisable()
    {
        // Disable the movement action map
    }

    private void Update()
    {
        Vector3 currentPosition = cursorObject.transform.position;

        Vector3 targetPosition = currentPosition + new Vector3(playerInput.GetMouseX(), playerInput.GetMouseY(), 0) * (speed * Time.deltaTime);
        targetPosition.x = Mathf.Clamp(targetPosition.x, cursorHalfWidth, screenWidth - cursorHalfWidth);
        targetPosition.y = Mathf.Clamp(targetPosition.y, cursorHalfHeight, screenHeight - cursorHalfHeight);

        cursorObject.transform.position = targetPosition;
    }
}