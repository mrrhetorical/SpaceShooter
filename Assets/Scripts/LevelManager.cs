using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum EndCondition
{
    Elimination, //Eliminate all of the enemies?
    Time //Survive a certain amount of time?
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private EndCondition _endCondition = EndCondition.Elimination;
    [SerializeField] private int _timeLimit = 0; //The amount of time that it takes to end the game (if required).

    private void Start()
    {
        if (_endCondition == EndCondition.Elimination)
        {
            //Disable score text if the mode is elimination.
            Player.Singleton.GetScoreText().enabled = false;
        }
    }

    private void Update()
    {
        if (_endCondition == EndCondition.Time && Player.Singleton.PlayerScore >= _timeLimit)
        {
            EndGame();
            return;
        }

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (_endCondition == EndCondition.Elimination && enemies.Length == 0)
        {
            EndGame();
        }
    }
    
    //When the player succeeds
    private static void EndGame()
    {
        Player.Singleton.GetScoreText().enabled = false;
        MenuManager.Singleton.EnableGameOverMenu();
        MenuManager.Singleton.GameOverText.text = "Congratulations!";
        MenuManager.Singleton.GameOverMessage.text = "You've completed the level!";
        MenuManager.Singleton.GameOverScore.enabled = false;
    }

}