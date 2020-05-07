using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// projectile launched by heal wave ability, see fire bolt projectile for documentation, heals instead of damaging
public class HealBoltProjectile : MonoBehaviour
{
    public string target = "Player";
    public Transform user;

    public float speed;
    public float lifetime;
    public float distance;
    public int heal;
    public int dotd;
    public int dott;
    public float slow;

    public LayerMask whatIsEnemy;

    public GameObject destroyEffect;

    private void Start()
    {
        Invoke("DestroyProjectile", lifetime);
    }
    private void FixedUpdate()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance);

        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag(target) && hitInfo.collider.transform != user)
            {
                Debug.Log(hitInfo.collider);
                hitInfo.collider.GetComponent<HealthController>().Heal(heal,user);
                DestroyProjectile();
            }

        }


        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void DestroyProjectile()
    {
        GameObject a = Instantiate(destroyEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
        Destroy(a, 1);
        Destroy(gameObject);
    }

}
