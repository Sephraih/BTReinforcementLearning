using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public float dmgDone = 0;
    public float dmgDoneTotal = 0;
    public float dmgTaken = 0;

    public float lived = 0;
    public float victories = 0;

    public int totalSteps;
    public int currentSteps;



    // Update is called once per frame
    void Update()
    {
        lived += Time.deltaTime;
    }

    public void DmgDone(float d)
    {
        dmgDone += d;
        dmgDoneTotal += d;
    }

    public void DmgTaken(float d)
    {
        dmgTaken += d;
    }

    public void DpSteps(int s)
    {
        float dps = dmgDone / s;
        print("Step: " + s + ": " + dps + " dps");
        GetComponent<BasicAgent>().SetReward(dps * 0.01f);
    }

    public void Won()
    {
        victories++;
    }

    public float ks()
    {
        return victories;
    }

    public void Reset()
    {
        dmgDone = 0;
        dmgTaken = 0;
        lived = 0;
        victories = 0;
    }

    public void TotalSteps(int ts)
    {
        totalSteps += ts;
    }

    void OnApplicationQuit()
    {
        Debug.Log("steps completed: " + totalSteps + " dps: " + dmgDoneTotal / totalSteps);
    }
}
