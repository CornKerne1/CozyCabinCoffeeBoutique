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
    [SerializeField]private GameObject roundTextObj,cashTextObj;
    [SerializeField]private Animator roundAnimator,cashAnimator;
    [SerializeField] private int maxRounds = 3, targetMultiplier = 3,cashAwardMultiplier=10;
    [SerializeField] private float startingSpacing=.5f;
    [SerializeField] private Vector2 xRange = new Vector2(-5f, 5f),yRange = new Vector2(-5f, 5f),zRange = new Vector2(-5f, 5f);
    private float _currentSpacing;
    private GameMode _gameMode;
    private int _currentRound=1;
    private List<GameTarget> _gameTargets=new List<GameTarget>();
    private List<Vector3> _targetPositions=new List<Vector3>();
    private TaskCompletionSource<bool> _waitForWinCompletionSource;
    private static readonly int Start1 = Animator.StringToHash("Start");
    private Vector3 _originPlayerPosition=Vector3.zero;
    private bool _roundLost=false;
    private float _roundStartTime;
    
    public static event EventHandler DepositMoney;

    // Start is called before the first frame update
    async void Start()
    {
        _gameMode=  GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        await Task.Delay(500);
        InitializeRound();
    }

    private async Task HandlePlayerMovement(bool freezePlayer)
    {
        var playerMovement = _gameMode.player.GetComponent<PlayerMovement>();
        if (freezePlayer)
        {
            var playerInput = _gameMode.playerInput;
            if (_originPlayerPosition == Vector3.zero) _originPlayerPosition = _gameMode.player.position;

                _gameMode.playerData.canJump = false;
            playerInput.ToggleMovement();
            playerMovement.TeleportPlayer(playerStart.position);
            _gameMode.player.LookAt(transform);
        }
        else
        {
            playerMovement.TeleportPlayer(_originPlayerPosition);
            _gameMode.playerData.canJump = true;
        }
    }

    async Task InitializeRound()
    {
        _roundLost=false;
        _waitForWinCompletionSource = new TaskCompletionSource<bool>();
        if (_currentRound > maxRounds) return;
        await CalculateGridPositions();
        await SpawnTargets();
        await WaitForWin();
        await ResetGame();
    }

    private async Task SpawnTargets()
    {
        for (int i = 0; i < _currentRound * targetMultiplier; i++)
        {
            var destination = _targetPositions[Random.Range(0, _targetPositions.Count)];
            double EPSILON = .01f;
            _targetPositions.RemoveAll(pos => (Mathf.Abs(pos.x - destination.x) < EPSILON && 
                                               Mathf.Abs(pos.y - destination.y) < EPSILON) ||
                                              (Mathf.Abs(pos.x - destination.x) < EPSILON &&
                                               Mathf.Abs(pos.z - destination.z) < EPSILON) ||
                                              (Mathf.Abs(pos.y - destination.y) < EPSILON &&
                                               Mathf.Abs(pos.z - destination.z) < EPSILON));

        
            var obj = Instantiate(targetPref, transform, false);
            obj.transform.localPosition = destination;
            obj.transform.rotation =
                new Quaternion(0, 0, 0, 0);
            _gameTargets.Add(obj.GetComponentInChildren<GameTarget>());
            _targetPositions.Remove(destination);
            RotateTarget(obj,false);
        }
        HandlePlayerMovement(true);
        await HandleRoundUI();
    }
    public async Task RotateTarget(GameObject target, bool reverse)
    {
        float degree;
        degree = reverse ? 0f : -90f;
        Quaternion targetRotation = Quaternion.Euler(degree, target.transform.rotation.eulerAngles.y, target.transform.rotation.eulerAngles.z);
        while (Quaternion.Angle(target.transform.rotation, targetRotation) > 0.01f)
        {
            target.transform.rotation = Quaternion.Slerp(target.transform.rotation, targetRotation, 2 * Time.deltaTime);
            await Task.Yield();
        }
        target.transform.rotation = targetRotation;
    }


    private async Task HandleRoundUI()
    {
        roundTextObj.GetComponent<TextMeshProUGUI>().text = _currentRound.ToString();
        roundAnimator.SetTrigger(Start1);
        _gameMode.playerInput.ToggleMovement();
        _roundStartTime = Time.time;
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
            bool targetBroken = false;
            foreach(var obj in _gameTargets)
            {
                if (obj.IsBroken())
                {
                    await RotateTarget(obj.transform.parent.gameObject, true);
                    Destroy(obj.transform.parent.gameObject);
                    targetBroken = true;
                }
            }
            if (targetBroken)
            {
                // If a target was broken, wait for the next frame before checking again
                await Task.Yield();
            }
            else if (Time.time - _roundStartTime > 5000 * _currentRound)
            {
                // If no target was broken for a certain amount of time, exit the loop and end the round
                _roundLost = true;
                break;
            }
            else
            {
                // Wait for a short amount of time before checking again
                await Task.Delay(100);
            }
        }
        if (!_roundLost)
        {
            DepositMoney?.Invoke(cashAwardMultiplier, EventArgs.Empty);
            _currentRound++;
            await InitializeRound();
        }
    }
    private async Task ResetGame()
    {
        _currentRound++;
        if (_currentRound > maxRounds)
        {
            await HandlePlayerMovement(false);
            Destroy(gameObject);
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
