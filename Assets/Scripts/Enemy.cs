using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform pointer;
    Rigidbody rb;

    [SerializeField] float moveForce;
    [SerializeField] float rotateSpeed;
    [SerializeField] float bounceForce;
    [SerializeField] int scoreAwarded;
    [SerializeField] float moveTime, moveTimer;

    [SerializeField] bool isBoss;

    [SerializeField] GameObject deathParticle;

    void Start()
    {
        if (!isBoss)
            GameManager.Instance.AddEnemy(this);
        else
            GameManager.Instance.AddBoss(this);
        rb = GetComponent<Rigidbody>();
        moveTimer = moveTime;
    }

    void Update()
    {
        Vector3 position = transform.position;
        Vector3 positionPlayer = GameManager.Instance.player.transform.position;

        Quaternion rotation = Quaternion.LookRotation(positionPlayer - transform.position, transform.TransformDirection(Vector3.back));
        pointer.rotation = new Quaternion(0, rotation.y, 0, rotation.w);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, pointer.rotation, rotateSpeed);

        if(moveTimer > 0.0f)
        {
            moveTimer -= Time.deltaTime;
        }
        else if(moveTimer < 0.0f)
        {
            moveTimer = moveTime;
            Vector3 direction = positionPlayer - position;
            direction = direction.normalized;
            direction *= moveForce;
            Move(direction);
        }
        Vector3 pos = transform.position;

        if (pos.y < -1.0f && GameManager.gameState != GameManager.GameState.victory)
        {
            Fall();
        }
    }

    private void Move(Vector3 dir)
    {
        //Debug.Log("swipe force : " + dir);
        Vector3 velocity = new Vector3();
        velocity = dir;
        Mathf.Clamp(velocity.x, 0, 300);
        Mathf.Clamp(velocity.y, 0, 300);
        velocity.y = 0;
        rb.AddForce(velocity);
    }

    void Fall()
    {
        GameManager.Instance.EnemyFall(scoreAwarded);
        if(!isBoss)
            GameManager.Instance.RemoveEnemy(this);
        else
            GameManager.Instance.RemoveBoss(this);
        Die();
    }
    public void Die()
    {
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.transform.tag == "MobileBumperEnemy")
        {
            trigger.GetComponent<Bumper>().animator.SetTrigger("Bump");
            Rigidbody collisionRB = trigger.gameObject.GetComponentInParent<Rigidbody>();
            Bumper bumper = trigger.gameObject.GetComponent<Bumper>();

            Vector3 collisionDirection = trigger.transform.position - transform.position;
            collisionDirection.y = 0;
            collisionDirection = collisionDirection.normalized;
            collisionRB.AddForce(collisionDirection * bounceForce);
            rb.AddForce(-collisionDirection * bumper.bounceForce);
        }
        if (trigger.transform.tag == "MobileBumperPlayer")
        {
            trigger.GetComponent<Bumper>().animator.SetTrigger("Bump");
            Rigidbody collisionRB = trigger.gameObject.GetComponentInParent<Rigidbody>();
            Bumper bumper = trigger.gameObject.GetComponent<Bumper>();

            Vector3 collisionDirection = trigger.transform.position - transform.position;
            collisionDirection.y = 0;
            collisionDirection = collisionDirection.normalized;
            collisionRB.AddForce(collisionDirection * bounceForce);
            rb.AddForce(-collisionDirection * bumper.bounceForce);

            trigger.gameObject.GetComponentInParent<PlayerController>().SetBumpTimer(0.5f);
            GameManager.Instance.cam.ShakeCamera();
        }
    }
}
