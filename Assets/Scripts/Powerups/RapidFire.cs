using System.Collections;
using UnityEngine;

public class RapidFire : Powerup
{
    [SerializeField] private float _duration = 10f;
    [SerializeField] private float _rapidFireRate = 0.05f;
    
    public override void PerformPowerAction()
    {        
        StartCoroutine(ApplyRapidFire());
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    private IEnumerator ApplyRapidFire()
    {
        var fireRate = Player.Singleton.FireRate;
        Player.Singleton.FireRate = _rapidFireRate;
        Player.Singleton.ApplyHeat = false;
        yield return new WaitForSeconds(_duration);
        Player.Singleton.FireRate = fireRate;
        Player.Singleton.ApplyHeat = true;
        
        base.PerformPowerAction();
    }
}