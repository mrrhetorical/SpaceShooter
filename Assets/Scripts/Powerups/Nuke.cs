using UnityEngine;

public class Nuke : Powerup
{    
    public override void PerformPowerAction()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            if (!enemy.GetComponent<BossEnemy>())
            {
                Destroy(enemy);
            }
        }

        base.PerformPowerAction();
    }
}