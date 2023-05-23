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
    private enum GameType
    {
        TargetThrow,
        RingToss
    };
    private GameType _currentGameType;
    [SerializeField] private Transform playerStart;
    [SerializeField]private GameObject targetPref,ringTossPref,roundTextObj,cashTextObj;
    [SerializeField]private Animator roundAnimator,cashAnimator;
    [SerializeField] private int maxRounds = 3, targetMultiplier = 3,cashAwardMultiplier=10;
    [SerializeField] private float startingSpacing=.5f;
    [SerializeField] private Vector2 xRange = new Vector2(-5f, 5f),yRange = new Vector2(-5f, 5f),zRange = new Vector2(-5f, 5f);
    private float _currentSpacing;
    private GameMode _gameMode;
    private int _currentRound=1,_currentBrokenTargets=0;
    private List<GameTarget> _gameTargets=new List<GameTarget>();
    private List<Vector3> _targetPositions=new List<Vector3>();
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
        SetGameType();
        InitializeRound();
    }

    private async void SetGameType()
    {
        var gameTypes = Enum.GetNames(typeof(GameType));
        int targetMultiplier = Random.Range(0, gameTypes.Length);
        _currentGameType = (GameType)targetMultiplier;
        switch (_currentGameType)
        {
            case GameType.TargetThrow:
                GameTarget.TargetBroken += IncrementBrokenTargets;
                break;
            case GameType.RingToss:
                RingTarget.RingToss+=IncrementBrokenTargets;
                break;
        }
    }

    async void IncrementBrokenTargets(object sender, EventArgs e)
    {
        GameObject obj = sender as GameObject;
        _currentBrokenTargets++;
        await PresentTarget(obj.transform.parent.gameObject, true);
        DestroyImmediate(obj.transform.parent.gameObject);
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
            _gameMode.camera.transform.LookAt(transform);
        }
        else
        {
            playerMovement.TeleportPlayer(_originPlayerPosition);
            _gameMode.playerData.canJump = true;
        }
    }

    async Task InitializeRound()
    {
        Debug.Log(_currentRound);
        _roundLost=false;
        _currentBrokenTargets = 0;
        await Task.Delay(100);
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
            GameObject obj = null;
            switch (_currentGameType)
            {
                case GameType.TargetThrow: 
                    obj = Instantiate(targetPref, transform, false);
                    obj.transform.localPosition = destination;
                    obj.transform.rotation =
                        new Quaternion(0, 0, 0, 0);
                    break;
                case GameType.RingToss: 
                    obj = Instantiate(ringTossPref, transform, false);
                    obj.transform.localPosition = new Vector3(destination.x,destination.y-.6f,destination.z);
                    break;
            }
            _gameTargets.Add(obj.GetComponentInChildren<GameTarget>());
            _targetPositions.Remove(destination);
            PresentTarget(obj,false);
        }
        HandlePlayerMovement(true);
        await HandleRoundUI();
    }
    public async Task PresentTarget(GameObject target, bool reverse)
    {
        float degree;
        Quaternion targetRotation;
        switch (_currentGameType)
        {
            case GameType.TargetThrow:
                degree = reverse ? 0f : -90f;
                targetRotation = Quaternion.Euler(degree, target.transform.rotation.eulerAngles.y, target.transform.rotation.eulerAngles.z);
                const int maxIterations = 1000; // Maximum number of iterations
                int iterations = 0; //
                while (Quaternion.Angle(target.transform.rotation, targetRotation) > 0.005f&& iterations < maxIterations)
                {
                    target.transform.rotation = Quaternion.Lerp(target.transform.rotation, targetRotation, 3 * Time.deltaTime);
                    await Task.Yield();
                    iterations++;
                }
                target.transform.rotation = targetRotation;
                break;
            case GameType.RingToss:
                degree = reverse ?target.transform.position.y -1 : target.transform.position.y+.7f;
                iterations = 0;
                while (Math.Abs(transform.position.y - degree) > .0001f&& iterations < maxIterations)
                {
                    target.transform.position = new Vector3(target.transform.position.x,
                        Mathf.Lerp(target.transform.position.y, degree, Time.deltaTime * 3),
                        target.transform.position.z);
                    await Task.Yield();
                    iterations++;
                }
                target.transform.position =
                    new Vector3(target.transform.position.x, degree, target.transform.position.z);
                break;
        }
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
        StartLossTimer();
        
        while (_currentBrokenTargets<targetMultiplier*_currentRound)
        {
            if (_currentBrokenTargets == targetMultiplier * _currentRound) break;
            await Task.Yield();
        }
        if (!_roundLost)
        {
           await UIReward();
        }
    }

    private async Task UIReward()
    {
        DepositMoney?.Invoke(cashAwardMultiplier, EventArgs.Empty);
        cashTextObj.GetComponent<TextMeshProUGUI>().text = "$" + cashAwardMultiplier.ToString();
        cashAnimator.SetTrigger(Start1);
        await Task.Delay(250);
    }

    private async void StartLossTimer()
    {
        while (Time.time - _roundStartTime < 5000 * _currentRound)
        {
            await Task.Yield();
        }
        _roundLost = true;
    }

    private async Task ResetGame()
    {
        _currentRound++;
        if (_currentRound > maxRounds)
        {
            await Task.Delay(1000);
            await HandlePlayerMovement(false);
            Destroy(gameObject);
        }
        _gameTargets.Clear();
        _targetPositions.Clear();
        // Create a new TaskCompletionSource
        await InitializeRound();
    }
}
