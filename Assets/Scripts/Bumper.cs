using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float bounceForce;
    //list if tag that should bump

    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            Rigidbody collisionRB = collision.rigidbody;
            Vector3 collisionDirection = collision.transform.position - transform.position;
            Bump(collisionRB, collisionDirection);

            collision.gameObject.GetComponent<PlayerController>().SetBumpTime(0.5f);

            //collisionRB.AddExplosionForce(bounceForce, collision.contacts[0].point, 2);
        }
        if(collision.transform.tag == "Enemy")
        {
            Rigidbody collisionRB = collision.rigidbody;
            Vector3 collisionDirection = collision.transform.position - transform.position;
            Bump(collisionRB, collisionDirection);
        }
        if (collision.transform.tag == "MobileBumper" || collision.transform.tag == "Bumper")
        {
            animator.SetTrigger("Bump");
        }

        //Debug.Log("Bump on : " + gameObject.name /* + " By : " + collision.gameObject.name*/);
        //Debug.Log("Unvalid Bump on : " + gameObject.name + " By : " + collision.gameObject.name);
    }

    void Bump(Rigidbody rb, Vector3 dir)
    {
        dir.y = 0;
        dir = dir.normalized;
        rb.AddForce(dir * bounceForce);
        animator.SetTrigger("Bump");
    }
}

//Debug.Log("Bump on : " + gameObject.name + " By : " + collision.gameObject.name);