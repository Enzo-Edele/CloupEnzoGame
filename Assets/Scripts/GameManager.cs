using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int lives;
    [HideInInspector] public int score;
    public int fallen;

    public int level;
    public List<int> levelObjectives = new List<int>();
    bool inBossFight;

    public PlayerController player;
    public EnemySpawner spawner;

    List<Enemy> enemiesList = new List<Enemy>();

    List<Enemy> bossList = new List<Enemy>();

    public Color highResistanceIndicator, LowResistanceIndicator;

    public enum GameState
    {
        inGame,
        pause,
        victory
    }
    public static GameState gameState { get; private set; }
    public void ChangeGameState(GameState state)
    {
        gameState = state;
        switch (gameState)
        {
            case GameState.inGame:
                Time.timeScale = 1.0f;
                break;
            case GameState.pause:
                Time.timeScale = 0.0f;
                break;
            case GameState.victory:
                Time.timeScale = 1.0f;
                break;
        }
    }

    public static GameManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        score = 0;
        lives = 1;
        level = 1;
        StartGame();
    }
    void StartGame()
    {
        fallen = 0;
        ClearEnemy();
        ClearBoss();
        player.Init();
        inBossFight = false;
        UIManager.Instance.UpdateScore(levelObjectives[level - 1] - fallen);
        UIManager.Instance.UpdateLevel(level);
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
    public void EnemyFall(int point)
    {
        fallen++;
        UpdateScore(point);
        CheckLevelObjective();
    }
    public void UpdateScore(int toAdd)
    {
        score += toAdd;
        UIManager.Instance.UpdateScore(levelObjectives[level - 1] - fallen);
    }

    public void CheckLevelObjective()
    {
        if(levelObjectives[level - 1] <= fallen && !inBossFight)
        {
            switch (level)
            {
                case 1:
                    NexLevel();
                    break;
                case 2:
                    SpawnBoss();
                    break;
                case 3:
                    SpawnBoss();
                    break;
            }
        }
    }
    public void NexLevel()
    {
        inBossFight = false;
        
        
        if (!(level >= 3))
        {
            level++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            UIManager.Instance.ActivateVictoryUI();
        }
        UIManager.Instance.UpdateLevel(level);
        StartGame();
    }
    void SpawnBoss()
    {
        inBossFight = true;
        spawner.SpawnBoss();
    }

    public void AddEnemy(Enemy enemy) {
        for(int i = 0; i < enemiesList.Count; i++)
            if (enemiesList[i] == enemy)
                return;
        enemiesList.Add(enemy);
    }
    public void RemoveEnemy(Enemy enemy) {
        for (int i = 0; i < enemiesList.Count; i++) {
            if (enemiesList[i] == enemy) {
                enemiesList.RemoveAt(i);
                return;
            }
        }
    }
    public void ClearEnemy() {
        for (int i = 0; i < enemiesList.Count; i++) {
            Destroy(enemiesList[i].gameObject);
        }
        enemiesList.Clear();
    }
    public void AddBoss(Enemy enemy) {
        for (int i = 0; i < bossList.Count; i++)
            if (bossList[i] == enemy)
                return;
        bossList.Add(enemy);
        UIManager.Instance.UpdateScore(levelObjectives[level - 1] - fallen);
    }
    public void RemoveBoss(Enemy enemy) {
        for (int i = 0; i < bossList.Count; i++) {
            if (bossList[i] == enemy) {
                bossList.RemoveAt(i);
            }
        }
        UIManager.Instance.UpdateScore(levelObjectives[level - 1] - fallen);
        if (bossList.Count == 0)
            NexLevel();
    }
    public void ClearBoss() {
        for (int i = 0; i < bossList.Count; i++) {
            Destroy(bossList[i].gameObject);
        }
        bossList.Clear();
    }
    public int BossListCount()
    {
        return bossList.Count;
    }
}
