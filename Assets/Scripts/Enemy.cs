using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int scoreAwarded;
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 pos = transform.position;
        if (pos.y < 2.0f)
        {
            Fall();
        }
    }

    void Fall()
    {
        GameManager.Instance.UpdateScore(scoreAwarded);
        Destroy(gameObject);
    }
}
