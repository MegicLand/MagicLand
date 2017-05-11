using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : State
{
    private Transform player;

    public DieState(Transform player)
    {
        this.player = player;
    }

    public override int GetStateId()
    {
        return (int)PlayerState.DieState;
    }

    public override void EnterState(State preState)
    {
        Debug.Log("Die");
    }

    public override void LeaveState(State nextState)
    {

    }

    public override void OnUpdate()
    {

    }
}
