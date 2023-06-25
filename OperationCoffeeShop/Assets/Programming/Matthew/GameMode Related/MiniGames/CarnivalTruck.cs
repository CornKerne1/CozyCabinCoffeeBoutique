using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class CarnivalTruck : MonoBehaviour
{
    [SerializeField] private Transform playerStart,dispenserTransform;
    [SerializeField]private GameObject roundTextObj,cashTextObj;
    [SerializeField]private Animator roundAnimator,cashAnimator;
    [SerializeField] private float startingSpacing=.5f;
    [SerializeField] private Vector2 xRange = new Vector2(-5f, 5f),yRange = new Vector2(-5f, 5f),zRange = new Vector2(-5f, 5f);
    [SerializeField]private List<CarnivalGameData> carnivalGameData=new List<CarnivalGameData>();
    [SerializeField]private int _currentGameType;
    private float _currentSpacing;
    private GameMode _gameMode;
    private int _currentRound=1,_currentBrokenTargets=0;
    private List<GameObject> _gameTargets=new List<GameObject>();
    private List<Vector3> _targetPositions=new List<Vector3>();
    private static readonly int Start1 = Animator.StringToHash("Start");
    private Vector3 _originPlayerPosition=Vector3.zero;
    private bool _roundLost=false;
    private float _roundStartTime;
    private bool _destroyed;
    private GameObject _currentDispenser;

    public static event EventHandler DepositMoney;

    // Start is called before the first frame update
    async void Start()
    {
        _gameMode=  GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        await Task.Delay(500);
        _gameMode.inMiniGame=true;
        SetGameType();
        AkSoundEngine.PostEvent("Play_TryYourLuck",_gameMode.gameObject);
        InitializeRound();
    }

    private async void SetGameType()
    {
        int i = Random.Range(0, 2);
        _currentGameType = i;
        switch (carnivalGameData[_currentGameType].gameType)
        {
            case CarnivalGameData.GameType.TargetThrow:
                GameTarget.TargetBroken += IncrementBrokenTargets;
                break;
            case CarnivalGameData.GameType.RingToss:
                RingTarget.RingToss+=IncrementBrokenTargets;
                break;
        }
        _currentDispenser=Instantiate(carnivalGameData[_currentGameType].dispenserPref);
        _currentDispenser.transform.position = dispenserTransform.position;
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
        while (_gameMode.playerData.inUI)
        {
            await Task.Yield();
        }
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
            if(!_gameMode.gameModeData.isOpen)
                _gameMode.playerData.canJump = true;
        }
    }

    async Task InitializeRound()
    {
        Debug.Log(_currentRound);
        _roundLost=false;
        _currentBrokenTargets = 0;
        await Task.Delay(100);
        if (_currentRound > carnivalGameData[_currentGameType].maxRounds) return;
        await CalculateGridPositions();
        await SpawnTargets();
        await WaitForWin();
        await ResetGame();
    }

    private async Task SpawnTargets()
    {
        for (int i = 0; i < _currentRound * carnivalGameData[_currentGameType].targetMultiplier; i++)
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
            switch (carnivalGameData[_currentGameType].gameType)
            {
                case CarnivalGameData.GameType.TargetThrow: 
                    obj = Instantiate(carnivalGameData[_currentGameType].gameTargetPref, transform, false);
                    obj.transform.localPosition = destination;
                    obj.transform.rotation =
                        new Quaternion(0, 0, 0, 0);
                    break;
                case CarnivalGameData.GameType.RingToss: 
                    obj = Instantiate(carnivalGameData[_currentGameType].gameTargetPref, transform, false);
                    obj.transform.localPosition = new Vector3(destination.x,destination.y-.6f,destination.z);
                    break;
            }
            _gameTargets.Add(obj);
            _targetPositions.Remove(destination);
            PresentTarget(obj,false);
        }
        await HandlePlayerMovement(true);
        await HandleRoundUI();
    }
    public async Task PresentTarget(GameObject target, bool reverse)
    {
        float degree;
        Quaternion targetRotation;
        switch (carnivalGameData[_currentGameType].gameType)
        {
            case CarnivalGameData.GameType.TargetThrow:
                degree = reverse ? 0f : -90f;
                targetRotation = Quaternion.Euler(degree, target.transform.rotation.eulerAngles.y, target.transform.rotation.eulerAngles.z);
                const int maxIterations = 1000; // Maximum number of iterations
                int iterations = 0; //
                
                while (!_destroyed&&Quaternion.Angle(target.transform.rotation, targetRotation) > 0.005f&& iterations < maxIterations)
                {
                    target.transform.rotation = Quaternion.Lerp(target.transform.rotation, targetRotation, 3 * Time.deltaTime);
                    await Task.Yield();
                    iterations++;
                }
                if (_destroyed) return;
                target.transform.rotation = targetRotation;
                break;
            case CarnivalGameData.GameType.RingToss:
                degree = reverse ?target.transform.position.y -1 : target.transform.position.y+.7f;
                iterations = 0;
                while (!_destroyed&&Math.Abs(transform.position.y - degree) > .0001f&& iterations < maxIterations)
                {
                    target.transform.position = new Vector3(target.transform.position.x,
                        Mathf.Lerp(target.transform.position.y, degree, Time.deltaTime * 3),
                        target.transform.position.z);
                    await Task.Yield();
                    iterations++;
                }
                if (_destroyed) return;
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
        
        while (_currentBrokenTargets<carnivalGameData[_currentGameType].targetMultiplier*_currentRound)
        {
            if (_currentBrokenTargets == carnivalGameData[_currentGameType].targetMultiplier * _currentRound) break;
            await Task.Yield();
        }
        if (!_roundLost)
        {
           await UIReward();
        }
    }

    private async Task UIReward()
    {
        DepositMoney?.Invoke(carnivalGameData[_currentGameType].cashAwardMultiplier, EventArgs.Empty);
        cashTextObj.GetComponent<TextMeshProUGUI>().text = "$" + carnivalGameData[_currentGameType].cashAwardMultiplier.ToString();
        cashAnimator.SetTrigger(Start1);
        await Task.Delay(250);
    }

    private async void StartLossTimer()
    {
        while (Time.time - _roundStartTime < carnivalGameData[_currentGameType].roundTime * _currentRound)
        {
            await Task.Yield();
        }

        foreach (GameObject gO in _gameTargets)
        {
            Destroy(gO);
        }
        _roundLost = true;
        await ResetGame();
    }

    private async Task ResetGame()
    {
        _currentRound++;
        if (_currentRound > carnivalGameData[_currentGameType].maxRounds)
        {
            await Task.Delay(1000);
            await HandlePlayerMovement(false);
            Destroy(gameObject);
        }
        foreach (GameObject gO in _gameTargets)
        {
            Destroy(gO);
        }
        _gameTargets.Clear();
        _targetPositions.Clear();
        // Create a new TaskCompletionSource
        await InitializeRound();
    }

    private void OnDestroy()
    {
        _gameMode.inMiniGame=false;
        AkSoundEngine.PostEvent("Stop_TryYourLuck",_gameMode.gameObject);
        Destroy(_currentDispenser);
        switch (carnivalGameData[_currentGameType].gameType)
        {
            case CarnivalGameData.GameType.TargetThrow:
                GameTarget.TargetBroken -= IncrementBrokenTargets;
                break;
            case CarnivalGameData.GameType.RingToss:
                RingTarget.RingToss-=IncrementBrokenTargets;
                break;
        }
        _destroyed = true;
    }
}
