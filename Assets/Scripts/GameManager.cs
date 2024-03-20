using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int lives;
    [HideInInspector] public int score;

    public PlayerController player;

    List<Enemy> enemiesList = new List<Enemy>();

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
        ClearEnemy();
        player.Init();
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

    public void AddEnemy(Enemy enemy)
    {
        for(int i = 0; i < enemiesList.Count; i++)
            if (enemiesList[i] == enemy)
                return;
        enemiesList.Add(enemy);
    }
    public void RemoveEnemy(Enemy enemy)
    {
        for (int i = 0; i < enemiesList.Count; i++)
        {
            if (enemiesList[i] == enemy)
            {
                enemiesList.RemoveAt(i);
                return;
            }
        }
    }
    public void ClearEnemy()
    {
        for (int i = 0; i < enemiesList.Count; i++)
        {
            Destroy(enemiesList[i].gameObject);
            enemiesList.RemoveAt(i);
        }
        enemiesList.Clear();
    }
}
