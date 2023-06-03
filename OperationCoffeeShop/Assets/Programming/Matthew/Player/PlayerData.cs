
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/Generic")]
public class PlayerData : ScriptableObject
{
    [Header("Controller Variables")]
    [Range(0, 10)]
    [SerializeField] public float moveSpeed;
    [Range(1, 2)]
    [SerializeField] public float sprintSpeed;
    [Range(2,6)]
    [SerializeField] public float jumpHeight;
    [Range(0, 5)]
    [SerializeField] public float inertiaVar;
    [Range(0, 100)]
    [SerializeField] public float mouseSensitivityX;
    [Range(0, 100)]
    [SerializeField] public float mouseSensitivityY;
    [SerializeField] public float neckClamp = 77.3f;
    [SerializeField] public LayerMask groundMask;
    [SerializeField] public float closeSpeed;
    [SerializeField] public bool canMove;
    [SerializeField] public bool canJump;
    [SerializeField] public bool canSprint;
    [SerializeField] public bool canCrouch;
    [SerializeField] public bool isSprinting;
    [SerializeField] public float cameraFov=60f;
    [SerializeField] public bool camMode;
    private Camera _camera;

    [Header("Crouch Variables")]
    [SerializeField] public float crouchHeight = .5f;
    [SerializeField] public float standHeight = 1f;
    [SerializeField] public Vector3 crouchingCenter = new Vector3(0,.7f,0);
    [SerializeField] public Vector3 standingCenter = new Vector3(0,.5f,0);
    
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

    [SerializeField]
    public bool inUI = false;
    public bool sleeping;

    [Header("Menus")]
    public float mouseSensitivityOptions;
    public string playerName;
    
    private void OnEnable()
    {
        busyHands = false;
        isClimbing = false;
        camMode = false;
        neckClamp = 77.3f;
        inUI = false;
        canMove = true;
        canSprint = true;
        canJump = true;
        canCrouch = true;
        if (mouseSensitivityOptions == 0)
            mouseSensitivityOptions = .5f;
        if (!SteamManager.Initialized) return;
        playerName = SteamFriends.GetPersonaName();
    }
}
