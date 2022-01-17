using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/Generic")]
public class PlayerData : ScriptableObject
{
    [Header("Controller Variables")]
    [Range(0, 10)]
    public float moveSpeed;
    [Range(0, 100)]
    public float mouseSensitivityX;
    [Range(0, 100)]
    public float mouseSensitivityY;
    [Range(0, 100)]
    public float neckClamp;
    public LayerMask groundMask;//
}
