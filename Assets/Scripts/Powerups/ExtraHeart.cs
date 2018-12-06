using UnityEngine;

public class ExtraHeart : Powerup
{
    public override void PerformPowerAction()
    {
        Player.Singleton.AddHeart();
        base.PerformPowerAction();
    }
}