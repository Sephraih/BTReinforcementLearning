using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class PlayerAgent : Agent
{
    Rigidbody2D rBody;
    public float speed = 10;
    float distanceToTarget;
    public Vector2 movementDirection;
    public float msi;
    public float targetHealth;


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
        rBody = GetComponent<Rigidbody2D>();
        targetHealth = Target.GetComponent<HealthController>().MaxHealth;
    }

    public Transform Target;
    public override void AgentReset()
    {
        //reset outofboundaries
        if (distanceToTarget > 15.0f)
        {
            transform.position = new Vector2(30, 25);
            rBody.velocity = Vector2.zero;
        }
        // Move the target to a new spot
        else
        Target.position = new Vector2(Random.value * 10 +25, Random.value * 10 + 20);
       

    }

    public override void CollectObservations()
    {
        // Target and Agent positions

        AddVectorObs(Target.position);
        AddVectorObs(this.transform.position);
        AddVectorObs(attackingDirection.transform.position);

        AddVectorObs(distanceToTarget);


        AddVectorObs(Target.GetComponent<HealthController>().health);

        // Agent velocity
        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.y);
    }

    public override void AgentAction(float[] vectorAction)
    {
        
        // Actions
        Vector2 controlSignal = Vector2.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.y = vectorAction[1];
        
        _q = vectorAction[2] >=0? true : false;
        if (_q)
        {
            GetComponent<FireBolt>().Blast();
        }

        _a = vectorAction[3] >= 0 ? true : false;
        if (_a)
        {
            GetComponent<MultiSlash>().Attack();
        }


        
        movementDirection = new Vector2(controlSignal.x,controlSignal.y);
        movementDirection.Normalize();
        msi = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
        GetComponent<MovementController>().Move(movementDirection, msi);
        //rBody.AddForce(controlSignal * speed);

        if (movementDirection != Vector2.zero)
        {
            attackingDirection.transform.localPosition = movementDirection * 0.5f;
        }

        // Rewards



        // ran away
        if (distanceToTarget > 15.0f)
        {
            SetReward(-0.1f);
            Done();
        }

        //target took dmg
        if (Target.GetComponent<HealthController>().health == Target.GetComponent<HealthController>().MaxHealth) { targetHealth = Target.GetComponent<HealthController>().MaxHealth; }
        if (targetHealth > Target.GetComponent<HealthController>().health) {
            SetReward(0.2f);
            targetHealth = Target.GetComponent<HealthController>().health;
        }

    }

    

    public override float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }
}
