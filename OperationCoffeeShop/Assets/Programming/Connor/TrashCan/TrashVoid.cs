using UnityEngine;

public class TrashVoid : MonoBehaviour
{
    private GameMode _gameMode;
    private PlayerInteraction _playerInteraction;

    private void Start()
    {
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        _playerInteraction = _gameMode.player.gameObject.GetComponent<PlayerInteraction>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp") && _playerInteraction.carriedObj != other.gameObject)
        {
            other.gameObject.SetActive(false);
        }
    }
}