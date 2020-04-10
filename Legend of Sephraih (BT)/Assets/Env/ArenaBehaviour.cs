using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaBehaviour : MonoBehaviour
{
    public List<Transform> enemyList; // list of currently active enemies
    public void ClosestEnemy(Transform self)
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
            if(enemy.GetComponent<PlayerAgent>().teamID != teamID) { self.GetComponent<PlayerAgent>().SetEnemy(enemy); }
            
        }
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
