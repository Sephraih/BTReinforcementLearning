using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaBehaviour : MonoBehaviour


{
    public List<Transform> characterList; // list of currently active enemies
    public List<Transform> treeList;
    public int deathcount =0;
    private void Start()
    {
        UpdateTrees();
    }

    public Transform ClosestEnemy(Transform self, Transform ce)
    {
        int teamID = self.GetComponent<StatusController>().teamID;

        if (characterList.Count >= 2) //at least self and another character
        {

            var distance = 10000f; //big number, if an enemy were further away, it'd be out of range.
            Transform enemy = characterList[0];
            foreach (Transform e in characterList)
            {
                if (Vector2.Distance(self.position, e.position) < distance && e.GetComponent<StatusController>().teamID != teamID)
                {
                    distance = Vector2.Distance(self.position, e.position);
                    enemy = e;
                }
            }
            if (enemy.GetComponent<StatusController>().teamID != teamID) { return enemy; }

        }
        return ce;
    }

    public Transform ClosestAlly(Transform self, Transform ca)
    {
        int teamID = self.GetComponent<StatusController>().teamID;

        if (characterList.Count >= 2) //at least self and and another character
        {

            var distance = 10000f; //big number, if an enemy were further away, it'd be out of range.
            Transform ally = characterList[0];
            foreach (Transform a in characterList)
            {
                if (Vector2.Distance(self.position, a.position) < distance && a.GetComponent<StatusController>().teamID == teamID && self != a)
                {
                    distance = Vector2.Distance(self.position, a.position);
                    ally = a;
                }
            }
            if (ally.GetComponent<StatusController>().teamID == teamID && self !=ally) { return ally; }

        }
        return ca;
    }

    public void Register(Transform self)
    {
        characterList.Add(self);
    }
    public void Remove(Transform self)
    {
        characterList.Remove(transform);
    }


    public void UpdateTrees()
    {

        foreach (Transform t in treeList)
        {
            float v = Random.Range(-11.5f, 14.5f);
            float h = Random.Range(-18.0f, 18.0f);
            Vector2 arenaPos = transform.transform.position;
            t.position = new Vector2(h, v) + arenaPos;
        }

    }
}
