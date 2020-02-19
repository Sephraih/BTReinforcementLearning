using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a firebolt ability assignable to all characters launching a projectile that damages, burns and slows target hostile to the caster it hits
public class FireBolt : MonoBehaviour
{
    private float offset = -90.0f; // default sprite rotation to align with vector.up
    public string enemy = "Enemy";

    public GameObject projectile; //prefab
    public Transform shotPoint; // infront of character

    private float cd; //remaining cooldown
    public float startcd; //ability cooldown

    public float slow = 0.5f; //default movement speed * slow

    //reduce cooldown each frame based on time passed
    private void Update()
    {
        cd -= Time.deltaTime;

    }

    // blast towards casters attacking point
    public void Blast()
    {
        Vector2 difference = transform.position - shotPoint.transform.position; //attack vector from transform to shotpoint

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; //rotate projectile onto attack vector
        shotPoint.transform.rotation = Quaternion.Euler(0f, 0f, rotZ - offset);
        Bolt();
    }

    //same use as blast but shooting towards mouse position, delay zero for testing
    public void BlastMouse()
    {


        Vector2 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; // vector from transform to mouse

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - 180; //rotate projectile onto vector
        shotPoint.transform.rotation = Quaternion.Euler(0f, 0f, rotZ - offset);
        Bolt();

    }

    // general blast logic
    public void Bolt()
    {
        //check whether the ability is ready to use
        if (cd <= 0)
        {
            //instantiate and assign values to a firebolt projectile, which handles damaging, position and collision logic based on the fireboltprojectile script attached to it.
            var bolt = Instantiate(projectile, shotPoint.position, shotPoint.transform.rotation);
            bolt.GetComponent<FireBoltProjectile>().enemy = this.enemy;
            bolt.GetComponent<FireBoltProjectile>().dmg += this.GetComponent<StatusController>().matk;
            bolt.GetComponent<FireBoltProjectile>().dotd += this.GetComponent<StatusController>().matk;
            bolt.GetComponent<FireBoltProjectile>().slow = slow;


            cd = startcd;

        }

    }

}


