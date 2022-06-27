using UnityEngine;


public class WalkInKitchenTrigger : MonoBehaviour
{

    private Objectives1 _objectives1;

    private GameMode _gameMode;

    private void Start()
    {
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        _objectives1 = GameObject.Find("Objectives").GetComponent<Objectives1>();

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter the kitchen");
        if (other.gameObject == _gameMode.player.gameObject)
        {
            _objectives1.NextObjective(gameObject);
        }
    }
}