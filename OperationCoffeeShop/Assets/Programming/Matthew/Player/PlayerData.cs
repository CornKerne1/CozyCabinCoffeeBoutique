
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/Generic")]
public class PlayerData : ScriptableObject
{
    [Header("Controller Variables")]
    [Range(0, 10)]
    [SerializeField] public float moveSpeed;
    [Range(0, 5)]
    [SerializeField] public float inertiaVar;
    [Range(0, 100)]
    [SerializeField] public float mouseSensitivityX;
    [Range(0, 100)]
    [SerializeField] public float mouseSensitivityY;
    [SerializeField] public float neckClamp = 77.3f;
    [SerializeField] public LayerMask groundMask;
    [SerializeField] public float closeSpeed;
    [SerializeField] public float openSpeed;
    [SerializeField] public bool canMove;
    [SerializeField] public float cameraFov=60f;
    [SerializeField] public bool camMode;
    private Camera _camera;

    [Header("Headbob Stuff")]
    [Range(0f, 1f)] [SerializeField] public float amplitude = 0.0003f;
    [Range(0, 30)] [SerializeField] public float frequency = 10.0f;

    [Header("Gameplay Related")]
    [Range(0, 100)]
    [SerializeField] public Vector3 currentMovement;
    [SerializeField] public bool isClimbing;

    [SerializeField] public float objMoveSpeed =0.1f;
    [SerializeField] public float interactDistance;
    [SerializeField] public float carryDistanceClamp;
    
    [Range(1, 20)]
    [SerializeField] public float objRotationSpeed;
    
    [SerializeField] public bool canInteract;
    [SerializeField] public bool busyHands;
    [Range(0, 10)]
    [SerializeField] public float carryDistance;
    [Range(0, 10)]
    [SerializeField] public float smooth;
    [SerializeField] public bool inUI = false;
    public List<Texture2D> screenShots = new List<Texture2D>();

    [Header("Menus")]
    public float mouseSensitivityOptions;
    
    private void OnEnable()
    {
        var c = screenShots.Count;
        screenShots = new List<Texture2D>();
        for (int i = 0; i <= c; i++)
        {
            if (File.Exists(Application.persistentDataPath + "ScreenShot" + i + ".png"))
            {
                byte[] textureBytes = File.ReadAllBytes(Application.persistentDataPath + "ScreenShot" + i + ".png");
                var sS = new Texture2D(0, 0);
                sS.LoadImage(textureBytes);
                sS.filterMode = FilterMode.Point;
                screenShots.Insert(i,sS);
            }
        }
        busyHands = false;
        isClimbing = false;
        camMode = false;
        neckClamp = 77.3f;
        inUI = false;
        canMove = true;
        if (mouseSensitivityOptions == 0)
            mouseSensitivityOptions = .5f;
    }
}
