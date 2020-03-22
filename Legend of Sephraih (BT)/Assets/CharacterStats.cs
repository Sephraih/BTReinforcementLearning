using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public float dmgDone=0;
    public float dmgTaken=0;
    public float lived=0;
    public float victories=0;

    
    // Update is called once per frame
    void Update()
    {
        lived += Time.deltaTime;    
    }

    public void DmgDone(float d) { 
    
    }

    public void DmgTaken(float d) { 
    
    }

    public void Won() {
        victories++;
    }

    public float ks() {
        return victories;
    }

    public void Reset()
    {
        dmgDone = 0;
        dmgTaken = 0;
        lived = 0;
        victories = 0;
    }
}
