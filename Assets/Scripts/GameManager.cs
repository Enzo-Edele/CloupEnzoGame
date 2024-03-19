using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int lives;
    [HideInInspector] public int score;

    public PlayerController player;

    public Color highResistanceIndicator, LowResistanceIndicator;

    public static GameManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        StartGame();
    }
    void StartGame()
    {
        score = 0;
        lives = 1;
        UIManager.Instance.UpdateScore(score);
    }

    void Update()
    {
        
    }

    public void ChangeLife(int toAdd)
    {
        lives += toAdd;
        if(lives <= 0)
        {
            StartGame();
        }
    }
    public void UpdateScore(int toAdd)
    {
        score += toAdd;
        UIManager.Instance.UpdateScore(score);
    }
}
