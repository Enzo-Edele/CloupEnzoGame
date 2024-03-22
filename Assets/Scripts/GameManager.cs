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
    public bool inBossFight;

    public PlayerController player;
    public EnemySpawner spawner;
    public CameraManager cam;

    List<Enemy> enemiesList = new List<Enemy>();

    List<Enemy> bossList = new List<Enemy>();

    public Color highResistanceIndicator, LowResistanceIndicator;

    public GameObject EnemyIndicatorPrefab;
    [SerializeField] GameObject FireWorkParticle;

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
        StartLevel();
    }
    void StartLevel()
    {
            fallen = 0;
            ClearEnemy();
            ClearBoss();
            player.Init();
            inBossFight = false;
            ChangeGameState(GameState.inGame);
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
            StartCoroutine(Respawn(0.5f));
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
                    StartCoroutine(PrepareNextLevel(5.0f));
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
            return;
        }
        UIManager.Instance.UpdateLevel(level);
        StartLevel();
    }
    void SpawnBoss()
    {
        inBossFight = true;
        spawner.SpawnBoss();
    }

    public IEnumerator Respawn(float duration)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            yield return null;
        }

        StartLevel();
    }
    public IEnumerator PrepareNextLevel(float duration)
    {
        float elapsed = 0.0f;

        ChangeGameState(GameState.victory);
        ClearEnemy();
        Instantiate(FireWorkParticle, player.transform.position, Quaternion.identity);
        player.rb.velocity = Vector3.zero;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            yield return null;
        }

        NexLevel();
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
            enemiesList[i].Die();
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
            StartCoroutine(PrepareNextLevel(5.0f));
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
