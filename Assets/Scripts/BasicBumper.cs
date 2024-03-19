using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBumper : MonoBehaviour
{
    [SerializeField] float bounceForce;
    //list if tag that should bump

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Bumpable")
        {
            Rigidbody collisionRB = collision.rigidbody;

            Vector3 collisionDirection = collision.transform.position - transform.position;
            collisionDirection.y = 0;
            collisionDirection = collisionDirection.normalized;
            collisionRB.AddForce(collisionDirection * bounceForce);

            //collisionRB.AddExplosionForce(bounceForce, collision.contacts[0].point, 2);
            Debug.Log("Bump on : " + gameObject.name + " By : " + collision.gameObject.name);
        }
        //check if tag == childofbumpable

        //Debug.Log("Unvalid Bump on : " + gameObject.name + " By : " + collision.gameObject.name);
    }
}
