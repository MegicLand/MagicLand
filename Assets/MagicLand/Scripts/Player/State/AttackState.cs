using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class AttackState : State {
    private Transform player;
    private PlayerBehavior playerBehavior;
	private MagicController MagicController;

    private bool isHolding;
    private Vector3 targetPos;
    private GameObject holdEffect;
	private GameObject magic;

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

		isHolding = true;
		holdEffect = MagicController.holdRune(playerBehavior.getRuneType());//generate holdEffect
    }

    public override void LeaveState(State nextState)
    {

    }

    public override void OnUpdate()
    {
        if (HandInfo.handPressed && isHolding)
        {
            holdEffect.transform.position = HandInfo.handPosition_World;
        }
        else if (!HandInfo.handPressed && isHolding)
        {
            GameObject.Destroy(holdEffect);
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward * 10);
            if (Physics.Raycast(ray, out hit))
            {
                targetPos = hit.point;
            }
            else
            {
                targetPos = Camera.main.transform.position + Camera.main.transform.forward * 10;
            }

            //魔法生效**************************************************************************************
			magic = MagicController.generateRune(playerBehavior.getRuneType(), HandInfo.handPosition_World, targetPos);
			isHolding = false;
        }
        else
        {

        }
    }
}
