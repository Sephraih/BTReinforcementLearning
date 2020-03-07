using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class PlayerAgent : Agent
{

    public Transform Target;
    float distanceToTarget;
    public float targetHealth;


    public Vector2 movementDirection;
    public float msi;
    public float agentHealth;


    public Vector2 startPos;
    public int posResetRndScale = 10;


    public GameObject attackingDirection; // object used to calculate a vector of attack


    //Skills
    private bool _a;
    private bool _q;
    private bool _w;
    private bool _e;


    void Update()
    {
        distanceToTarget = Vector2.Distance(this.transform.position, Target.position);
    }

    void Start()
    {
        targetHealth = Target.GetComponent<HealthController>().MaxHealth;
        agentHealth = GetComponent<HealthController>().MaxHealth;
    }

    public override void AgentReset()
    {
        ResetPosition(transform);
        GetComponent<HealthController>().max();
    }

    public void ResetPosition(Transform t)
    {
        Vector2 rnd = new Vector2(Random.value * posResetRndScale - posResetRndScale, Random.value * posResetRndScale - posResetRndScale);
        t.position = rnd + startPos;
    }

    public override void CollectObservations()
    {
        // Target and Agent positions

        AddVectorObs(Target.position);
        AddVectorObs(transform.position);
        //AddVectorObs(attackingDirection.transform.position);

        //AddVectorObs(distanceToTarget);


        AddVectorObs(Target.GetComponent<HealthController>().health);
        AddVectorObs(GetComponent<HealthController>().health);

    }

    public override void AgentAction(float[] vectorAction)
    {

        // Actions
        Vector2 controlSignal = Vector2.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.y = vectorAction[1];

        _q = vectorAction[2] >= 0 ? true : false;
        if (_q)
        {
            GetComponent<FireBolt>().Blast();
        }

        /*
        _a = vectorAction[3] >= 0 ? true : false;
        if (_a)
        {
            GetComponent<MultiSlash>().Attack();
        }
        */


        movementDirection = new Vector2(controlSignal.x * 100, controlSignal.y * 100);
        movementDirection.Normalize();
        msi = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
        GetComponent<MovementController>().Move(movementDirection, msi);

        if (movementDirection != Vector2.zero)
        {
            attackingDirection.transform.localPosition = movementDirection * 0.5f;
        }

        // Rewards

        //target dead
        if (Target.GetComponent<HealthController>().health <= 0)
        {
            SetReward(0.9f);
            Done();
        }

        //agent dead
        if (GetComponent<HealthController>().health <= 0)
        {
            SetReward(-0.9f);
            Done();
        }

        //target took dmg
        if (Target.GetComponent<HealthController>().health == Target.GetComponent<HealthController>().MaxHealth) { targetHealth = Target.GetComponent<HealthController>().MaxHealth; }
        if (targetHealth > Target.GetComponent<HealthController>().health)
        {
            float dmgtaken = targetHealth - Target.GetComponent<HealthController>().health;
            SetReward(0.0001f * dmgtaken);
            targetHealth = Target.GetComponent<HealthController>().health;
        }

        //agent took dmg
        if (GetComponent<HealthController>().health == GetComponent<HealthController>().MaxHealth) { agentHealth = GetComponent<HealthController>().MaxHealth; }
        if (agentHealth > GetComponent<HealthController>().health)
        {
            float dmgtaken = agentHealth - GetComponent<HealthController>().health;
            SetReward(-0.0001f * dmgtaken);
            agentHealth = GetComponent<HealthController>().health;
        }

    }



    public override float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        if (Input.GetButtonUp("e")) action[2] = 1;
        return action;
    }
}
