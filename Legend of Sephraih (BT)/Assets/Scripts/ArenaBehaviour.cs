using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaBehaviour : MonoBehaviour


{
    public List<Transform> characterList; // list of currently active enemies
    public List<Transform> treeList;
    public int deathcount =0;
    private float distance =10000; //big value, characters further away cannot be targeted

    private void Start()
    {
       // UpdateTrees(); //randomize at start
    }

    //return a calling agent's closest enemy
    public Transform ClosestEnemy(Transform self, Transform ce)
    {
        int teamID = self.GetComponent<StatusController>().teamID;

        if (characterList.Count >= 2) //at least self and another character
        {

            Transform enemy = characterList[0];
            foreach (Transform e in characterList)
            {
                if (Vector2.Distance(self.position, e.position) < distance && e.GetComponent<StatusController>().teamID != teamID) //search for the enemy with the shortest distance to calling agent
                {
                    distance = Vector2.Distance(self.position, e.position);
                    enemy = e;
                }
            }
            if (enemy.GetComponent<StatusController>().teamID != teamID) { return enemy; }

        }
        return ce;
    }

    //return a calling agent's closest ally
    public Transform ClosestAlly(Transform self, Transform ca)
    {
        int teamID = self.GetComponent<StatusController>().teamID;

        if (characterList.Count >= 2) //at least self and and another character
        {

            Transform ally = characterList[0];
            foreach (Transform a in characterList)
            {
                if (Vector2.Distance(self.position, a.position) < distance && a.GetComponent<StatusController>().teamID == teamID && self != a) //search for the ally with the shortest distance (not self)
                {
                    distance = Vector2.Distance(self.position, a.position);
                    ally = a;
                }
            }
            if (ally.GetComponent<StatusController>().teamID == teamID && self !=ally) { return ally; }

        }
        return ca;
    }

    public void Register(Transform self) //called by each agent at the start of the game or training, add it to the character list
    {
        characterList.Add(self);
    }
    public void Remove(Transform self) //remove character from list if it stops existing (currently, agents respawn upon death, hence this is not called)
    {
        characterList.Remove(self);
    }

    // function that may be called at the game's start or when an agent achieves a kill divisible through five 
    public void UpdateTrees()
    {
        //reassign every tree to a random location within the arena it belongs to
        foreach (Transform t in treeList)
        {
            float v = Random.Range(-11.5f, 14.5f);
            float h = Random.Range(-18.0f, 18.0f);
            Vector2 arenaPos = transform.transform.position;
            t.position = new Vector2(h, v) + arenaPos;
        }

    }
}
