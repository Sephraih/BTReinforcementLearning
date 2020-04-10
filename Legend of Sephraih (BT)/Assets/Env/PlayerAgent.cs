﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class PlayerAgent : Agent
{

    public Transform enemy;
    float distanceToTarget;
    public float targetHealth;

    public Vector2 movementDirection;
    public float msi;
    public float agentHealth;


    public int posResetRndScale = 14;
    public GameObject arena;
    public int teamID;


    public GameObject attackingDirection; // object used to calculate a vector of attack


    //Skills
    private bool _a;
    private bool _q;
    private bool _w;
    private bool _e;




    void Start()
    {
        targetHealth = enemy.GetComponent<HealthController>().MaxHealth;
        agentHealth = GetComponent<HealthController>().MaxHealth;
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
    }

    public void ResetPosition(Transform t)
    {
        //Vector2 rnd = new Vector2(Random.value * posResetRndScale*2 - posResetRndScale, Random.value * posResetRndScale*2 - posResetRndScale);
        //t.position = new Vector2(rnd.x + arena.transform.position.x, rnd.y + arena.transform.position.y);
        float v = Random.Range(-11.5f, 14.5f);
        float h = Random.Range(-18.0f, 18.0f);
        Vector2 arenaPos = arena.transform.position;
        t.position = new Vector2(h, v) + arenaPos;
    }


    //observation Vector
    public override void CollectObservations()
    {
        arena.GetComponent<ArenaBehaviour>().ClosestEnemy(transform); //get closest enemy inside arena
        //AddVectorObs(distanceToTarget);
        AddVectorObs(enemy.localPosition.x);
        AddVectorObs(enemy.localPosition.y);
        AddVectorObs(transform.localPosition.x);
        AddVectorObs(transform.localPosition.y);

        //AddVectorObs(Target.GetComponent<HealthController>().health);
        //AddVectorObs(GetComponent<HealthController>().health);

    }

    //action Vector
    public override void AgentAction(float[] vectorAction)
    {


        distanceToTarget = Vector2.Distance(this.transform.position, enemy.position);
        if (GetStepCount() > 0 && GetStepCount() % 1000 == 0) GetComponent<CharacterStats>().DpSteps(GetStepCount());

        // Actions -> unity documentation: By default the output from our provided PPO algorithm pre-clamps the values of vectorAction into the [-1, 1]
        Vector2 movementAction = Vector2.zero;
        movementAction.x = vectorAction[0];
        movementAction.y = vectorAction[1];

        /*Vector2 attDir = Vector2.zero;
        attDir.x = vectorAction[2];
        attDir.y = vectorAction[3];*/


        _q = vectorAction[2] >= 0.5f ? true : false;
        if (_q)
        {
            //GetComponent<FireBolt>().BlastVec(new Vector2(attDir.x, attDir.y)); // -1 to 1 on both x y corresponding to vector added to user pos
            //GetComponent<FireBolt>().BlastAngle(vectorAction[2]); //at angle -1 to 1 corresponding to -180 to 180
            //GetComponent<FireBolt>().Blast(); //frontal blast
            GetComponent<FireBolt>().BlastTarget(new Vector2(enemy.localPosition.x, enemy.localPosition.y));
        }
        _a = vectorAction[3] >= 0.5f ? true : false;
        if (_a)
        {
            GetComponent<MultiSlash>().Attack();
        }

            movementDirection = new Vector2(movementAction.x * 100, movementAction.y * 100);
        movementDirection.Normalize();
        msi = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
        GetComponent<MovementController>().Move(movementDirection, msi);

        if (movementDirection != Vector2.zero)
        {
            attackingDirection.transform.localPosition = movementDirection * 0.5f;
        }

        // Rewards (external: healthcontroller)

        /*
        //target took dmg
        if (Target.GetComponent<HealthController>().health == Target.GetComponent<HealthController>().MaxHealth) { targetHealth = Target.GetComponent<HealthController>().MaxHealth; }
        if (targetHealth > Target.GetComponent<HealthController>().health)
        {
            float dmgDone = targetHealth - Target.GetComponent<HealthController>().health;
            SetReward(0.0001f * dmgDone); //1000 damage available = 0.1f reward per enemy, 0.01 /hit
            GetComponent<CharacterStats>().DmgDone(dmgDone);
            targetHealth = Target.GetComponent<HealthController>().health;
        }

        //agent took dmg
        if (GetComponent<HealthController>().health == GetComponent<HealthController>().MaxHealth) { agentHealth = GetComponent<HealthController>().MaxHealth; }
        if (agentHealth > GetComponent<HealthController>().health)
        {
            float dmgTaken = agentHealth - GetComponent<HealthController>().health;
            SetReward(-0.0001f * dmgTaken);
            GetComponent<CharacterStats>().DmgTaken(dmgTaken);
            agentHealth = GetComponent<HealthController>().health;
        }
        */
        if (distanceToTarget > 20.0f) SetReward(-0.005f); //far from enemy (being lame)

    }

    public void Victory()
    {
        float ks = GetComponent<CharacterStats>().ks();
        SetReward(0.05f + ks * 0.05f); ;
        GetComponent<CharacterStats>().Won();
        print("ks= " + (ks + 1));
        Camera.main.GetComponent<Statistics>().UpdateHks(ks + 1);
    }

    public void Defeat()
    {
        SetReward(-0.5f);
        int sc = GetStepCount();
        Debug.Log("defeat: " + sc);
        GetComponent<CharacterStats>().TotalSteps(-maxStep+sc); //-maxstep cause it driggers done, which adds maxstep

        Done();
    }

    public void SetEnemy(Transform e) { enemy = e; }

    public override float[] Heuristic()
    {
        var action = new float[5];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        action[2] = Input.GetAxis("Horizontal");
        action[3] = Input.GetAxis("Vertical");
        action[4] = Input.GetButtonUp("e") == true ? 1f : 0f;

        return action;
    }
}