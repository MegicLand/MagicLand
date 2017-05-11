using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class AttackState : State {
    private Transform player;
    private PlayerBehavior playerBehavior;

    public AttackState(Transform player)
    {
        this.player = player;
        playerBehavior = player.GetComponent<PlayerBehavior>();
    }

    public override int GetStateId()
    {
        return (int)PlayerState.AttackState;
    }

    public override void EnterState(State preState)
    {
        Debug.Log("Attack");
    }

    public override void LeaveState(State nextState)
    {

    }

    public override void OnUpdate()
    {
        if (!HandInfo.handPressed)
        {
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward * 10);
            Physics.Raycast(ray, out hit);
            Vector3[] path = new Vector3[3];
            path[0] = player.position;
            path[1] = (player.position + hit.point) / 2;
            path[2] = hit.point;

            //魔法生效**************************************************************************************

            playerBehavior.ToDrawRuneState();
        }
    }
}
