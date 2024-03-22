using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;

    [SerializeField] float speed;
    [SerializeField] float velocityMax;

    Vector2 initialPos;
    Vector2 swipeDelta;

    [SerializeField] float bumpTimer;
    public float bumpTime;
    [SerializeField] float swipeTime, swipeTimer;

    [SerializeField] GameObject bumpImage;
    [SerializeField] Image bumpBarMask;
    float originalBumpBarSize;

    [SerializeField] Image dashBarMask;
    float originalDashBarSize;

    [SerializeField] GameObject deathParticle;

    bool hasFell;

    //use a isgrounded for isOnSliddingTerrain (suggestion)

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        rb = GetComponent<Rigidbody>();
        originalDashBarSize = dashBarMask.rectTransform.rect.width;
        originalBumpBarSize = bumpBarMask.rectTransform.rect.width;

        Init();
    }

    public void Init()
    {
        rb.velocity = new Vector3(0, 0, 0);
        transform.position = new Vector3(0, 0.9f, 0);
        swipeTimer = 0.0f;
        UpdateDashBar(swipeTime, swipeTimer);
        hasFell = false;
    }

    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            //use stationnary for charge a bump (suggestion)
            if(touch.phase == TouchPhase.Began)
            {
                initialPos = touch.position;
            }
            if(touch.phase == TouchPhase.Ended)
            {
                swipeDelta = touch.position - initialPos;
                if (swipeDelta != Vector2.zero && CanSwipe())
                    swipePerformed(swipeDelta);
            }
        }

        Vector3 pos = transform.position;
        if(pos.y < -1.0f && !hasFell)
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
            UpdateBumpBar(bumpTime, bumpTimer);
        }
        else if (bumpTimer < 0.0f)
        {
            bumpTimer = 0.0f;
            bumpImage.SetActive(false);
        }

        Vector3 velocity = rb.velocity;
        if (velocity.y > 0)
            velocity.y = 0;
        rb.velocity = velocity;
        
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, velocityMax);

        //Debug.Log("velocity : " + rb.velocity);
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
        return (bumpTimer <= 0.0f && swipeTimer <= 0.0f && !(GameManager.gameState == GameManager.GameState.victory));
    }

    public void UpdateDashBar(float total, float actual)
    {
        dashBarMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalDashBarSize * (actual / total));
    }
    public void UpdateBumpBar(float total, float actual)
    {
        bumpBarMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalBumpBarSize * (actual / total));
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.transform.tag == "MobileBumper")
        {
            trigger.GetComponent<Bumper>().animator.SetTrigger("Bump");
        }
    }
    public void SetBumpTimer(float time)
    {
        bumpTime = time;
        bumpTimer = time;
        bumpImage.SetActive(true);
    }

    void Fall()
    {
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        hasFell = true;
        GameManager.Instance.ChangeLife(-1);
    }
}
