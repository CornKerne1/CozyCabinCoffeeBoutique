using UnityEngine;

public class CupPositionTrigger : MonoBehaviour
{
    private GameMode _gameMode;

    private void Start()
    {
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        CheckTutorial();
    }

    private void CheckTutorial()
    {
        if (_gameMode.gameModeData.inTutorial)
        {
            _gameMode.Tutorial.AddedGameObject(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("placed cup");
        if (!_gameMode.gameModeData.inTutorial || _gameMode.playerData.busyHands ||
            !other.gameObject.TryGetComponent<IngredientContainer>(out _)) return;
        _gameMode.Tutorial.NextObjective(gameObject);
        gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("placed cup");
        if (!_gameMode.gameModeData.inTutorial || _gameMode.playerData.busyHands ||
            !other.gameObject.TryGetComponent<IngredientContainer>(out _)) return;
        _gameMode.Tutorial.NextObjective(gameObject);
        gameObject.SetActive(false);
    }
}