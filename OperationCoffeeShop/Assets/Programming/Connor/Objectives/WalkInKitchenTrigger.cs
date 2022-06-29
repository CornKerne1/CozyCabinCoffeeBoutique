using UnityEngine;


public class WalkInKitchenTrigger : MonoBehaviour
{


    private GameMode _gameMode;

    private void Start()
    {
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        if (_gameMode.gameModeData.inTutorial)
        {
            _gameMode.Tutorial.AddedGameObject(this.gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter the kitchen");
        if (other.gameObject == _gameMode.player.gameObject)
        {
            _gameMode.Tutorial.NextObjective(gameObject);
        }
    }
}