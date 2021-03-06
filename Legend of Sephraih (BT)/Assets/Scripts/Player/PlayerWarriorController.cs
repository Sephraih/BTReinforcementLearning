﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The character controller that allows a player to control a warrior
public class PlayerWarriorController : PlayerController
{
    // used in the process of determining whether the player wants to use a skill
    private bool _a;
    private bool _e;

    public override void Skills()
    {
        // attacks and skills
        _a = Input.GetButtonUp("a");
        _e = Input.GetButtonUp("e");

    }

    // using the skills assigned to the keys depending on input
    public override void Attack()
    {
        enemy = arena.GetComponent<ArenaBehaviour>().ClosestEnemy(transform); //get closest enemy inside arena
        if (_a)
        {
            GetComponent<MultiSlash>().Attack();
        }

        if (_e)
        {
            GetComponent<ChargeAttack>().Charge(enemy);
        }
    }
}