using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]TMP_Text score;
    [SerializeField] TMP_Text level;
    [SerializeField] TMP_Text victory;

    [SerializeField] GameObject menuButton;
    [SerializeField] GameObject menu;

    public static UIManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        level.text = "Level : " + GameManager.Instance.level;
        score.text = "" + (GameManager.Instance.levelObjectives[SceneManager.GetActiveScene().buildIndex] - GameManager.Instance.fallen);
    }
    public void UpdateScore(int nScore)
    {
        score.text = nScore.ToString();
        if (nScore <= 0 && GameManager.Instance.inBossFight)
            score.text = "Boss : " + GameManager.Instance.BossListCount();
    }
    public void UpdateLevel(int nLevel)
    {
        level.text = "Level : " + nLevel;
    }

    public void ActivateVictoryUI()
    {
        level.text = "";
        score.text = "";
        victory.text = "Victory";
    }

    void ActivatePauseMenu(bool nState)
    {
        menuButton.SetActive(!nState);
        menu.SetActive(nState);
    }
    public void OpenMenu()
    {
        GameManager.Instance.ChangeGameState(GameManager.GameState.pause);
        ActivatePauseMenu(true);
    }
    public void Resume()
    {
        ActivatePauseMenu(false);
        GameManager.Instance.ChangeGameState(GameManager.GameState.inGame);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
