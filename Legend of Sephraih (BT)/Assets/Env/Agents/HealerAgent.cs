using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class HealerAgent : BasicAgent
{

    public Transform ally;
    public Transform target;
    //Skills
    private bool _q;
    private bool _e;
    private bool _a;

    private bool _t;


    //observation Vector
    public override void CollectObservations()
    {
        enemy = arena.GetComponent<ArenaBehaviour>().ClosestEnemy(transform, enemy); //get closest enemy inside arena
        ally = arena.GetComponent<ArenaBehaviour>().ClosestAlly(transform, ally); //get closest ally inside arena
        AddVectorObs(target.localPosition.x);
        AddVectorObs(target.localPosition.y);
        AddVectorObs(transform.localPosition.x);
        AddVectorObs(transform.localPosition.y);
    }

    //action Vector
    public override void AgentAction(float[] vectorAction)
    {

        distanceToTarget = Vector2.Distance(this.transform.position, ally.position);

        if (GetStepCount() > 0 && GetStepCount() % 1000 == 0) GetComponent<CharacterStats>().DpSteps(GetStepCount());

        // Actions -> unity documentation: By default the output from our provided PPO algorithm pre-clamps the values of vectorAction into the [-1, 1]
        Vector2 movementAction = Vector2.zero;
        movementAction.x = vectorAction[0];
        movementAction.y = vectorAction[1];

        _t = vectorAction[2] >= 0f ? true : false;
        if (_t)
        {
            target = enemy;
        }
        else { target = ally; }
        
        _e = vectorAction[3] >= 0.5f ? true : false;
        if (_e)
        {
            GetComponent<HealWave>().BlastTarget(new Vector2(target.localPosition.x, target.localPosition.y));
        }
        _a = vectorAction[4] >= 0.5f ? true : false;
        if (_a)
        {
            GetComponent<SelfHeal>().Heal();
        }
        _q = vectorAction[5] >= 0.5f ? true : false;
        if (_q)
        {
            GetComponent<HealBolt>().BlastTarget(new Vector2(target.localPosition.x, target.localPosition.y));
        }


            movementDirection = new Vector2(movementAction.x * 100, movementAction.y * 100);
        movementDirection.Normalize();
        msi = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
        GetComponent<MovementController>().Move(movementDirection, msi);

        if (movementDirection != Vector2.zero)
        {
            attackingDirection.transform.localPosition = movementDirection * 0.5f;
        }


        if (distanceToTarget > 20.0f) SetReward(-0.005f); //far from enemy (being lame)
    }


    public override float[] Heuristic()
    {
        var action = new float[6];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        action[2] = Input.GetButtonUp("t") == true ? 1f : 0f;
        action[3] = Input.GetButtonUp("w") == true ? 1f : 0f;
        action[4] = Input.GetButtonUp("a") == true ? 1f : 0f;
        action[5] = Input.GetButtonUp("q") == true ? 1f : 0f;
        return action;
    }
}
