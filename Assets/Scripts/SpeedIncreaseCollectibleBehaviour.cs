using UnityEngine;

public class SpeedIncreaseCollectibleBehaviour : ICollectible
{
    PlayerMovement player;

    protected override void Behave()
    {
        if (player == null)
            player = FindObjectOfType<PlayerMovement>();
        player.MoveIncrease(1);
        Debug.Log("Collected!!");
    }

}
