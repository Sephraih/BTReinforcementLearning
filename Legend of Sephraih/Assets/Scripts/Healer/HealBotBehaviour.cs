using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script to be attached to the healer character
public class HealBotBehaviour : MonoBehaviour
{


    public Rigidbody2D rb; // handles physics
    public Animator animator; // handles the animation

    public Vector2 movementDirection; // from input x,y movement
    public float msi; // movement speed input strenght zero to one

    public GameObject attackingDirection; // an object so that its difference to the character is the vector determining the direction of attack

    public Transform dps; // reference to the first character

    // every frame
    void Update()
    {
        if (transform != Camera.main.GetComponent<camerafollow>().target) //if not followed by camera it is not the active char
        {
            Move();
            Aim();
            UseSkills();
        }
    }
       
    //move player based on input and play movement animation
    void Move()
    {
        // follow character one, keep distance safety distance
        if (Vector2.Distance(transform.position, dps.position) > 5.0f)
        {
            movementDirection = new Vector2(-1 * (rb.position.x - dps.transform.position.x), -1 * (rb.position.y - dps.transform.position.y));
            movementDirection.Normalize();
        }
        else movementDirection = new Vector2(0, 0);

        msi = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f); // equalize to a value from zero to one

        GetComponent<MovementController>().Move(movementDirection, msi); //move using the movement controller

    }


    // set attacking direction object's position
    void Aim()
    {
        if (movementDirection != Vector2.zero)
        {
            attackingDirection.transform.localPosition = movementDirection * 0.5f; //the object is positioned at a distance of 0.5 to character in movement direction
        }
        else
        {
            var dir = Camera.main.GetComponent<camerafollow>().LowestHealthOtherPlayer(transform).transform.position - transform.position; // if not moving, target the lowest health player
            attackingDirection.transform.localPosition = dir / dir.magnitude;

        }
    }

    void UseSkills()
    {
        if (Camera.main.GetComponent<camerafollow>().LowestHealthOtherPlayer(transform).GetComponent<HealthController>().health 
            != Camera.main.GetComponent<camerafollow>().LowestHealthOtherPlayer(transform).GetComponent<HealthController>().MaxHealth) //if the lowest health target that isnt the healer itself is not at max health.
            this.GetComponent<HealBolt>().Blast(); // blast @ target direction, which is movement direction or aim direction of which either is towards the target, the one that is applies
    }



}
