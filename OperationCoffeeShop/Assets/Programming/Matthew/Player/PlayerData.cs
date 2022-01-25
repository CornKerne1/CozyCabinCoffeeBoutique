using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/Generic")]
public class PlayerData : ScriptableObject
{
    [Header("Controller Variables")]
    [Range(0, 10)]
    public float moveSpeed;
    [Range(.01f, .99f)]
    public float inertiaVar;
    [Range(0, 100)]
    public float mouseSensitivityX;
    [Range(0, 100)]
    public float mouseSensitivityY;
    [Range(0, 100)]
    public float neckClamp;
    public LayerMask groundMask;//

    [Header("Headbob Stuff")]
    [Range(0.00001f, 0.001f)] public float amplitude = 0.0003f;
    [Range(0, 30)] public float frequency = 10.0f;

    [Header("Gameplay Related")]
    [Range(0, 100)]
    public float interactDistance;
}
