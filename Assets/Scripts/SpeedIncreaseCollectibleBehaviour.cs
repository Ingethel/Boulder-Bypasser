using UnityEngine;

public class SpeedIncreaseCollectibleBehaviour : ICollectible
{
    PlayerMovement player;
    PopUpManager popUpManager;
    ScoreManager scoreManager;
    public int score;

    protected override void Behave()
    {
        CheckDependancies();

        scoreManager.ScoreIncrease(score);
        player.MoveIncrease(1);
        popUpManager.SpawnScorePopUp(0, score, Camera.main.WorldToScreenPoint(transform.position));
    }

    void CheckDependancies()
    {
        if (!player)
            player = FindObjectOfType<PlayerMovement>();
        if (!popUpManager)
            popUpManager = FindObjectOfType<PopUpManager>();
        if (!scoreManager)
            scoreManager = FindObjectOfType<ScoreManager>();
    }
}
