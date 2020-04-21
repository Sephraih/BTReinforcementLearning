using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaBehaviour : MonoBehaviour
{
    public List<Transform> enemyList; // list of currently active enemies
    public Transform ClosestEnemy(Transform self, Transform ce)
    {
        int teamID = self.GetComponent<StatusController>().teamID;

        if (enemyList.Count >= 2) //at least self and an enemy
        {

            var distance = 10000f; //big number, if an enemy were further away, it'd be out of range.
            Transform enemy = enemyList[0];
            foreach (Transform e in enemyList)
            {
                if ( Vector2.Distance(self.position, e.position) < distance && e.GetComponent<StatusController>().teamID !=teamID)
                {
                    distance = Vector2.Distance(self.position, e.position);
                    enemy = e;
                }
            }
            if(enemy.GetComponent<StatusController>().teamID != teamID) { return enemy; }
            
        }
        return ce;
    }
    public void Register(Transform self)
    {
        enemyList.Add(self);
    }
    public void Remove(Transform self)
    {
        enemyList.Remove(transform);
    }
}
