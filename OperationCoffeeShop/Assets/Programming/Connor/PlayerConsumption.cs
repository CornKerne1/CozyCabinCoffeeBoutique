using UnityEngine;


public class PlayerConsumption : MonoBehaviour
{
    [HideInInspector] public GameMode gameMode;

    [SerializeField, Header("Tutorial Stuff")]
    private Objectives objectives;

    [SerializeField]private bool _completedObjective;

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
        Destroy(liquid);
        if (!GameMode.IsEventPlayingOnGameObject("Play_Gulp", gameObject))
            AkSoundEngine.PostEvent("Play_Gulp", gameObject);
        //Debug.Log("Drinking the coffee");
        Destroy(other.gameObject);
        IfTutorial();
    }

    private void IfTutorial()
    {
        if (!gameMode.gameModeData.inTutorial ||!objectives.CheckObjective(13)|| _completedObjective) return;
        _completedObjective = true;
        objectives.NextObjective(gameObject);
    }
}