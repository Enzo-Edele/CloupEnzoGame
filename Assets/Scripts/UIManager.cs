using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    TMP_Text score;

    public static UIManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }
    public void UpdateScore(int nScore)
    {
        //score.text = nScore.ToString();
    }
}
