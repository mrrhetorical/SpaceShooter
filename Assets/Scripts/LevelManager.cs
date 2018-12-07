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
    [SerializeField] private EndCondition _endCondition = EndCondition.Time;
    [SerializeField] private int _timeLimit = 61; //The amount of time that it takes to end the game (if required).
    [SerializeField] private Text _winText;

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
    private void EndGame()
    {
        MenuManager.Singleton.EnableGameOverMenu();
        MenuManager.Singleton.GameOverMessage.text = "You've completed the level!";
    }

}