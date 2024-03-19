using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float speed;

    Vector3 velocity;

    Vector2 initialPos;
    Vector2 swipeDelta;

    bool isBump;
    [SerializeField] float bumpTime, bumpTimer;
    [SerializeField] float swipeTime, swipeTimer;

    //use a isgrounded for isOnSliddingTerrain

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            //detect stationnary for charge bump
            if(touch.phase == TouchPhase.Began)
            {
                initialPos = touch.position;
            }
            if(touch.phase == TouchPhase.Ended)
            {
                swipeDelta = touch.position - initialPos;
                if (swipeDelta != Vector2.zero)
                    swipePerformed(swipeDelta);
            }
        }

        Vector3 pos = transform.position;
        if(pos.y < 2.0f)
        {
            Fall();
        }
    }
    
    private void FixedUpdate()
    {
        //check isBounce

        Vector3 velocity = transform.position;
        velocity.x += Input.GetAxis("Horizontal") * speed * Time.fixedDeltaTime;
        velocity.z += Input.GetAxis("Vertical") * speed * Time.fixedDeltaTime;

        transform.position = velocity;
    }

    void swipePerformed(Vector2 direction)
    {
        //check isBounce
        Vector3 velocity = new Vector3();
        velocity.x = direction.x;
        velocity.z = direction.y;
        rb.AddForce(velocity);
    }

    void Fall()
    {
        GameManager.Instance.ChangeLife(-1);
    }
}
