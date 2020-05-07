using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class BasicAgent : Agent
{

    public Transform enemy;
    public float distanceToTarget;
    public float targetHealth;

    public Vector2 movementDirection;
    public float msi;
    public float agentHealth;


    public int posResetRndScale = 14;
    public GameObject arena;
    public int teamID;


    public GameObject attackingDirection; // object used to calculate a vector of attack

    void Start()
    {
        targetHealth = enemy.GetComponent<HealthController>().maxHealth;
        agentHealth = GetComponent<HealthController>().maxHealth;
        arena.GetComponent<ArenaBehaviour>().Register(transform);
        GetComponent<StatusController>().teamID = GetComponent<BehaviorParameters>().m_TeamID;
    }

    public override void AgentReset()
    {
        //currentsteps at this point is zero, triggered at max steps or when Done();
        ResetPosition(transform);
        GetComponent<HealthController>().Max();
        GetComponent<CharacterStats>().Reset();
        GetComponent<CharacterStats>().TotalSteps(maxStep);
        arena.GetComponent<ArenaBehaviour>().deathcount++;
        int a = arena.GetComponent<ArenaBehaviour>().deathcount;
        if (a % 5 == 0) arena.GetComponent<ArenaBehaviour>().UpdateTrees(); //trees randomized every 5th death
    }


    public void Victory()
    {
        float ks = GetComponent<CharacterStats>().ks();
        AddReward(0.05f + ks * 0.05f); ;
        GetComponent<CharacterStats>().Won();
        print("ks= " + (ks + 1));
        Camera.main.GetComponent<Statistics>().UpdateHks(ks + 1);
    }

    public void Defeat()
    {
        AddReward(-0.5f);
        int sc = GetStepCount();
        Debug.Log("defeat: " + sc);
        GetComponent<CharacterStats>().TotalSteps(-maxStep + sc); //-maxstep cause it driggers done, which adds maxstep

        Done();
    }

    public void ResetPosition(Transform t)
    {
        int a = Random.Range(1,5);
        float h = 0;
        float v = 0;

        switch (a)
        {
            case 1:
                v = Random.Range(-2f, 2f);
                h = Random.Range(-25.0f, -21.0f);
                break;
            case 2:
                v = Random.Range(-2f, 2f);
                h = Random.Range(21.0f, 25.0f);
                break;
            case 3:
                v = Random.Range(16f, 20f);
                h = Random.Range(-3.0f, 3.0f);
                break;
            case 4:
                v = Random.Range(-20f, -16f);
                h = Random.Range(-3.0f, 3.0f);
                break;
            default:
                break;
        }

        Vector2 arenaPos = arena.transform.position;
        t.position = new Vector2(h, v) + arenaPos;
    }

    public void SetEnemy(Transform e) { enemy = e; }
}


/*
            v = Random.Range(-11.5f, 14.5f);
            h = Random.Range(-18.0f, 18.0f);
*/
