using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CarnivalGameData", menuName = "MiniGames/CarnivalTruck")]
public class CarnivalGameData : ScriptableObject
{
    public enum GameType
    {
        TargetThrow,
        RingToss
    };
    public GameType gameType;
    public GameObject gameTargetPref;
    public int maxRounds = 3, targetMultiplier = 3,cashAwardMultiplier=10;
    public float startingSpacing;
}
