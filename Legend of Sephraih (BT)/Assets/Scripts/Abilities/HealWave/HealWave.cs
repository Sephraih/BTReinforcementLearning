using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//healwave ability launching healwave projectiles
public class HealWave : MonoBehaviour
{
    private float offset = -90.0f; //make the sprite and particle system face upwards
    public string friendly = "Player";  //default, may be inverted in inspector 
    public string hostile = "Enemy";

    public GameObject projectile; // the healwave projectile prefab is attached to the editor in this public field
    public Transform shotPoint;

    //ability cooldown and cooldown remaining
    private float cd;
    public float startcd;

    private void Update()
    {
        cd -= Time.deltaTime;

    }

    public void Blast()
    {
        Vector2 difference = transform.position - shotPoint.transform.position;

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        shotPoint.transform.rotation = Quaternion.Euler(0f, 0f, rotZ - offset);
        Wave();

    }



    //same use as blast but shooting towards mouse position
    public void BlastMouse()
    {


        Vector2 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - 180;
        shotPoint.transform.rotation = Quaternion.Euler(0f, 0f, rotZ - offset);
        Wave();
    }

    public void Wave()
    {

        //check whether ability cooldown is ready
        if (cd <= 0)
        {

            var bolt = Instantiate(projectile, transform.position, shotPoint.transform.rotation);
            bolt.GetComponent<HealWaveProjectile>().friendly = friendly;
            bolt.GetComponent<HealWaveProjectile>().hostile = hostile;
            bolt.GetComponent<HealWaveProjectile>().heal += this.GetComponent<StatusController>().matk;
            bolt.GetComponent<HealWaveProjectile>().user = transform;

            cd = startcd;

        }
    }

}


