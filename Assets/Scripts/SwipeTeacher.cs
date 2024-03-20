using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTeacher : MonoBehaviour
{
    Vector2 initialPos;
    Vector2 swipeDelta;
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            //detect stationnary for charge bump
            if (touch.phase == TouchPhase.Began)
            {
                initialPos = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                swipeDelta = touch.position - initialPos;
                if (swipeDelta != Vector2.zero)
                    Destroy(gameObject);
            }
        }
    }
}
