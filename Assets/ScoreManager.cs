using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the score of the player both in the game and in the database
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public Scoreboard scoreBoardHandler;
    private int totalScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        scoreBoardHandler = GameObject.FindObjectOfType<Scoreboard>();
    }

    public void AddScore(int score)
    {
        totalScore += score;
    }

    public void DecreaseScore(int value)
    {
        if (totalScore - value <= 0)
        {
            totalScore = 0;
            return;
        }
        else
        {
            totalScore -= value;
        }
    }

}
