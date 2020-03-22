using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistics : MonoBehaviour
{

    public float time;
    public float hks=0; //highest killing spree achieved during game by agent


    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

    }

    public void UpdateHks(float n) {
        hks = hks > n ? hks : n;
    }
}
