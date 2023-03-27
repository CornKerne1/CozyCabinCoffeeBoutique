using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarnivalTruck : MonoBehaviour
{
    [SerializeField] private Transform playerStart;
    [SerializeField] private GameObject targetPref;
    [SerializeField]private GameObject textObj;
    [SerializeField]private Animator animator;
    [SerializeField] private int maxRounds = 3, targetMultiplier = 3;
    [SerializeField] private float startingSpacing=.5f;
    [SerializeField] private Vector2 xRange = new Vector2(-5f, 5f),yRange = new Vector2(-5f, 5f),zRange = new Vector2(-5f, 5f);
    private float _currentSpacing;
    private GameMode _gameMode;
    private int _currentRound=1;
    private List<GameObject> _gameTargets=new List<GameObject>();
    private List<Vector3> _targetPositions=new List<Vector3>();
    private TaskCompletionSource<bool> _waitForWinCompletionSource;
    private static readonly int Start1 = Animator.StringToHash("Start");

    // Start is called before the first frame update
    async void Start()
    {
        _gameMode=  GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        InitializeRound();
        _gameMode.player.GetComponent<PlayerMovement>().TeleportPlayer(playerStart,false);
        _gameMode.player.LookAt(transform);
    }

    async Task InitializeRound()
    {
        _waitForWinCompletionSource = new TaskCompletionSource<bool>();
        if (_currentRound > maxRounds) return;
        await CalculateGridPositions();
        await SpawnTargets();
        await WaitForWin();
        ResetGame();
    }

    private async Task SpawnTargets()
    {
        for (int i = 0; i < _currentRound * targetMultiplier; i++)
        {
            var destination = _targetPositions[Random.Range(0, _targetPositions.Count)];

            // Remove any positions that share the same two axis values as the destination
            double EPSILON = .01f;
            _targetPositions.RemoveAll(pos => (Mathf.Abs(pos.x - destination.x) < EPSILON && 
                                               Mathf.Abs(pos.y - destination.y) < EPSILON) ||
                                              (Mathf.Abs(pos.x - destination.x) < EPSILON &&
                                               Mathf.Abs(pos.z - destination.z) < EPSILON) ||
                                              (Mathf.Abs(pos.y - destination.y) < EPSILON &&
                                               Mathf.Abs(pos.z - destination.z) < EPSILON));

        
            var obj = Instantiate(targetPref, transform, false);
            obj.transform.localPosition = destination;
            _gameTargets.Add(obj.transform.GetChild(0).gameObject);
            _targetPositions.Remove(destination);
        }

        await HandleRoundUI();
    }

    private async Task HandleRoundUI()
    {
        textObj.GetComponent<TextMeshProUGUI>().text = _currentRound.ToString();
        animator.SetTrigger(Start1);
    }
    
    private async Task CalculateGridPositions()
    {
        _currentSpacing = startingSpacing / _currentRound;
        for (float x = xRange.x; x <= xRange.y; x += _currentSpacing)
            for (float y = yRange.x; y <= yRange.y; y+=  .05f)
        for (float z = zRange.x; z <= zRange.y; z += _currentSpacing)
        {
            _targetPositions.Add(new Vector3(x, y, z));
        }

    }
    private async Task WaitForWin()
    {
        while (_gameTargets.Count > 0)
        {
            for (int i = _gameTargets.Count - 1; i >= 0; i--)
                if(_gameTargets[i]==null) _gameTargets.RemoveAt(i);
            await Task.Yield();
        }
        // Use TaskCompletionSource to signal completion
        _waitForWinCompletionSource.SetResult(true);
    }
    private async Task ResetGame()
    {
        _currentRound++;
        if (_currentRound > maxRounds)
        {
            // end game
        }
        _gameTargets.Clear();
        _targetPositions.Clear();
        // Create a new TaskCompletionSource
        _waitForWinCompletionSource = new TaskCompletionSource<bool>();
        await InitializeRound();
        // Wait for WaitForWin to complete before continuing
        await _waitForWinCompletionSource.Task;
    }
}
