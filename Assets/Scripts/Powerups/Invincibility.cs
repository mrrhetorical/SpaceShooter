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
        var current = Player.Singleton.GetComponent<SpriteRenderer>().color;
        Player.Singleton.GetComponent<SpriteRenderer>().color = Color.yellow;
        yield return new WaitForSeconds(InvincibilityTime - 2);

        var t = (InvincibilityTime - (InvincibilityTime - 2)) / 6f;
        
        for (var i = 0; i < 3; i++)
        {
            Player.Singleton.GetComponent<SpriteRenderer>().color = Player.Singleton.GetHealth() < 4 ? Color.white : new Color(0, 0.5f, 1, 1);
            yield return new WaitForSeconds(t);
            Player.Singleton.GetComponent<SpriteRenderer>().color = Color.yellow;
            yield return new WaitForSeconds(t);
        }

        Player.Singleton.Invincible = false;

        Player.Singleton.GetComponent<SpriteRenderer>().color =
            Player.Singleton.GetHealth() == 4 ? new Color(0, 0.5f, 1, 1) : Color.white;

        base.PerformPowerAction();
    }

}