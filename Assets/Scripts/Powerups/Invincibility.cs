using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : Powerup
{

    public float InvincibilityTime = 10f;
    
    public override void PerformPowerAction()
    {
        if (Player.Singleton.Invincible)
        {
            base.PerformPowerAction();
            return;
        }

        StartCoroutine(ApplyInvincibility());

        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    private IEnumerator ApplyInvincibility()
    {
        Player.Singleton.Invincible = true;
        Player.Singleton.GetComponent<SpriteRenderer>().color = Color.yellow;
        yield return new WaitForSeconds(InvincibilityTime - 2);

        var t = (InvincibilityTime - (InvincibilityTime - 2)) / 6f;
        
        for (var i = 0; i < 3; i++)
        {
            Player.Singleton.GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(t);
            Player.Singleton.GetComponent<SpriteRenderer>().color = Color.yellow;
            yield return new WaitForSeconds(t);
        }

        Player.Singleton.Invincible = false;
        Player.Singleton.GetComponent<SpriteRenderer>().color = Color.white;
        
        base.PerformPowerAction();
    }

}