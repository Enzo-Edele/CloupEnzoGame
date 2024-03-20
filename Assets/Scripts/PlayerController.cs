using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] Image dashBarMask;
    float originalDashBarSize;

    //use a isgrounded for isOnSliddingTerrain

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originalDashBarSize = dashBarMask.rectTransform.rect.width;

        Init();
    }

    public void Init()
    {
        rb.velocity = new Vector3(0, 0, 0);
        transform.position = new Vector3(0, 0.9f, 0);
        swipeTimer = 0.0f;
        UpdateDashBar(swipeTime, swipeTimer);
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
                if (swipeDelta != Vector2.zero && swipeTimer <= 0.0f)
                    swipePerformed(swipeDelta);
            }
        }

        Vector3 pos = transform.position;
        if(pos.y < -2.0f)
        {
            Fall();
        }

        if(swipeTimer > 0.0f)
        {
            swipeTimer -= Time.deltaTime;
            UpdateDashBar(swipeTime, swipeTimer);
        }
        else if (swipeTimer < 0.0f)
        {
            swipeTimer = 0.0f;
        }
        if (bumpTimer > 0.0f)
        {
            bumpTimer -= Time.deltaTime;
        }
        else if (bumpTimer < 0.0f)
        {
            bumpTimer = 0.0f;
        }
    }

    void swipePerformed(Vector2 direction)
    {
        //Debug.Log("swipe force : " + direction);
        Vector3 velocity = new Vector3();
        velocity.x = direction.x;
        velocity.z = direction.y;
        rb.AddForce(velocity);
        swipeTimer = swipeTime;
    }
    bool CanSwipe()
    {
        return (bumpTimer <= 0.0f && swipeTimer <= 0.0f);
    }

    public void UpdateDashBar(float total, float actual)
    {
        dashBarMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalDashBarSize * (actual / total));
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.transform.tag == "MobileBumper")
        {
            trigger.GetComponent<BasicBumper>().animator.SetTrigger("Bump");
        }
    }

    void Fall()
    {
        GameManager.Instance.ChangeLife(-1);
    }
}
