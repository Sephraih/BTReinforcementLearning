﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class PlayerAgent : BasicAgent
{

    //Skills
    private bool _a;
    private bool _q;
    private bool _w;
    private bool _e;
    
    //observation Vector
    public override void CollectObservations()
    {
        enemy = arena.GetComponent<ArenaBehaviour>().ClosestEnemy(transform); //get closest enemy inside arena
        //AddVectorObs(distanceToTarget);
        AddVectorObs(enemy.localPosition.x);
        AddVectorObs(enemy.localPosition.y);
        AddVectorObs(transform.localPosition.x);
        AddVectorObs(transform.localPosition.y);
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
            GetComponent<FireBolt>().BlastTarget(enemy);
        }
        _a = vectorAction[3] >= 0.5f ? true : false;
        if (_a)
        {
            GetComponent<MultiSlash>().Attack();
        }
        _e = vectorAction[4] >= 0.5f ? true : false;
        if (_e)
        {
            GetComponent<ChargeAttack>().Charge(enemy);
        }

        movementDirection = new Vector2(movementAction.x * 100, movementAction.y * 100);
        movementDirection.Normalize();
        msi = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
        GetComponent<MovementController>().Move(movementDirection, msi);

        if (movementDirection != Vector2.zero)
        {
            attackingDirection.transform.localPosition = movementDirection * 0.5f;
        }

        if (distanceToTarget > 20.0f) AddReward(-0.005f); //far from enemy (being lame)

    }

    /* //used for debugging, controlled through player script otherwise
    public override float[] Heuristic()
    {
        var action = new float[5];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        action[2] = Input.GetButtonUp("q") == true ? 1f : 0f;
        action[3] = Input.GetButtonUp("a") == true ? 1f : 0f;
        action[4] = Input.GetButtonUp("e") == true ? 1f : 0f;

        return action;
    }*/
}
