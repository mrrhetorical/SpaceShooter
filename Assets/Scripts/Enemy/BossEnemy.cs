using UnityEngine;

public class BossEnemy : ShootingEnemy
{
    public override void Start()
    {
        base.Start();
        _moveDown = false;
        _spriteRenderer.color = Color.cyan;
    }

    public override void BaseKill()
    {
        EnemySpawner.Singleton.BossSpawned = false;
        base.BaseKill();
    }
    
}