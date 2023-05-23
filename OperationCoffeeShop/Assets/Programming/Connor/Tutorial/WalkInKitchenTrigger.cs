using UnityEngine;


public class WalkInKitchenTrigger : MonoBehaviour
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
        Debug.Log("enter the kitchen");
        if (_gameMode.gameModeData.inTutorial)
        {
            _gameMode.Tutorial.NextObjective(gameObject);
        }
    }
}