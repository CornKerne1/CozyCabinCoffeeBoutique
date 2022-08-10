using UnityEngine;


public class PlayerConsumption : MonoBehaviour
{
    [HideInInspector] public GameMode gameMode;

    [SerializeField, Header("Tutorial Stuff")]
    private Objectives objectives;

    private bool _completedObjective;

    private void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();

        SetTutorial();
    }

    private void SetTutorial()
    {
        if (gameMode.gameModeData.inTutorial)
        {
            objectives = gameMode.Tutorial.Objectives;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<LiquidIngredients>(out var liquid)) return;
        liquid.gameObject.SetActive(false);
        if (!GameMode.IsEventPlayingOnGameObject("Play_Gulp", gameObject))
            AkSoundEngine.PostEvent("Play_Gulp", gameObject);
        //Debug.Log("Drinking the coffee");
        IfTutorial();
    }

    private void IfTutorial()
    {
        if (!gameMode.gameModeData.inTutorial || _completedObjective) return;
        _completedObjective = true;
        objectives.NextObjective(gameObject);
    }
}