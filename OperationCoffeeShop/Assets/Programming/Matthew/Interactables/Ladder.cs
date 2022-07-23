public class Ladder : Interactable
{
    public PlayerInteraction pI;
    public bool canClimb;

    public override void Start()
    {
        base.Start();
        canClimb = false;
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        this.pI = playerInteraction;
        if (gameMode.gameModeData.isOpen) return;
        if (pI.playerData.isClimbing)
        {
            pI.playerData.isClimbing = false;
        }
        else
        {
            if (!canClimb) return;
            pI.playerData.isClimbing = true;
            IfTutorial();
        }
    }

    private void IfTutorial()
    {
        if (gameMode.gameModeData.inTutorial)
        {
            gameMode.Tutorial.NextObjective(gameObject);
        }
    }
}