using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerafollow : MonoBehaviour
{

    public Transform target; // target the camera looks at 

    //the three defined player characters
    public Transform ichi; // the first character, the dps
    public Transform ni; // the second character, the healer
    public Transform san; // the third character, the tank
    public Transform dummy; //dummy target to see that there is no enemy

    public Vector3 offset;
    public List<Transform> enemylist; // list of currently active enemies
    public Animator ShakeAnimation; // various actions in the game use a shake animation to simulate collision effects

    // follow target
    void LateUpdate()
    {
        transform.position = target.position + offset;

    }

    private void Update()
    {



        if (Input.GetButtonDown("Swap"))
        {
            if (target == ichi)
            {
                //target.gameObject.SetActive(false);
                target = ni;
            }
            else if (target == ni)
            {
                // target.gameObject.SetActive(false);
                target = san;
            }
            else if (target == san)
            {
                //target.gameObject.SetActive(false);
                target = ichi;
            }

            target.gameObject.SetActive(true);
        }
        InstantiateEnemy();
        SelectCharacter();



    }

    // plays a shake animation using an animator attached to the camera object, essentially altering the camera's position for the duration of  the animation
    public void CamShake()
    {
        ShakeAnimation.SetTrigger("shake");
    }

    // returns the closest player in relation to a character - enemy or player
    public Transform ClosestPlayer(Transform self)
    {

        if (ni.gameObject.activeSelf && san.gameObject.activeSelf)
        {
            var one = Vector2.Distance(self.position, ichi.position);
            var two = Vector2.Distance(self.position, ni.position);
            var three = Vector2.Distance(self.position, san.position);
            if (one < two && one < three) return ichi; //one is the closest
            if (three < two) return san; //one is not the closest, three is closer than two
            return ni;
        }
        return ichi;
    }

    // returns the enemy closest to the calling character
    public Transform ClosestEnemy(Transform self)
    {
        if (enemylist.Count == 0) { return dummy; }
        var distance = Vector2.Distance(self.position, enemylist[0].position);
        Transform enemy = enemylist[0];
        foreach (Transform e in enemylist)
        {
            if (Vector2.Distance(self.position, e.position) < distance)
            {
                distance = Vector2.Distance(self.position, e.position);
                enemy = e;
            }
        }

        return enemy;
    }

    // determines and returns the player with the lowest health among the three defined player characters
    public Transform LowestHealthPlayer()
    {

        if (ni.gameObject.activeSelf && san.gameObject.activeSelf)
        {
            var one = ichi.GetComponent<HealthController>().health;
            var two = ni.GetComponent<HealthController>().health;
            var three = san.GetComponent<HealthController>().health;
            if (one < two && one < three) return ichi; //one is the lowest
            if (three < two) return san; //one is not the lowest, three is lower than two
            return ni;
        }
        return ichi;
    }

    // determines lowest health player that is not the user, should only be called from a player character
    public Transform LowestHealthOtherPlayer(Transform self)
    {
        if (self == ichi) { if (ni.GetComponent<HealthController>().health <= san.GetComponent<HealthController>().health) return ni; else return san; }
        else if (self == ni) { if (ichi.GetComponent<HealthController>().health <= san.GetComponent<HealthController>().health) return ichi; else return san; }
        else { if (ichi.GetComponent<HealthController>().health <= ni.GetComponent<HealthController>().health) return ichi; else return ni; }
    }

    public void InstantiateEnemy()
    {

        if (Input.GetButtonDown("enemy1"))
        {
            // load an enemy at current mouse position, transformed to game world position
            Instantiate((Resources.Load("Prefabs/Enemy") as GameObject), Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 1), Quaternion.identity);
        }
        if (Input.GetButtonDown("enemy2"))
        {
            // load an enemy at current mouse position, transformed to game world position
            GameObject a = Instantiate((Resources.Load("Prefabs/Guard") as GameObject), Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 1), Quaternion.identity);
            a.GetComponent<GuardBehaviour>().guardSpot = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 1);
        }
        if (Input.GetButtonDown("enemy3"))
        {
            // load an enemy at current mouse position, transformed to game world position
            Instantiate((Resources.Load("Prefabs/Wizard") as GameObject), Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 1), Quaternion.identity);
        }
    }

    public void SelectCharacter()
    {
        
        if (Input.GetButtonDown("ichi"))
        {
            DisableCharacters();
            ichi.gameObject.SetActive(true);
            target = ichi;
        }
        if (Input.GetButtonDown("ni"))
        {
            DisableCharacters();
            ni.gameObject.SetActive(true);
            target = ni;
        }
        if (Input.GetButtonDown("san"))
        {
            DisableCharacters();
            san.gameObject.SetActive(true);
            target = san;
        }

    }

    public void DisableCharacters()
    {
        ichi.gameObject.SetActive(false);
        ni.gameObject.SetActive(false);
        san.gameObject.SetActive(false);
    }
}
