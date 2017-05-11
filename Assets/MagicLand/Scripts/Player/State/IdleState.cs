using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State {
    private Transform player;

    public IdleState(Transform player)
    {
        this.player = player;
    }

    public override int GetStateId()
    {
        return (int)PlayerState.IdleState;
    }

    public override void EnterState(State preState)
    {
        Debug.Log("Idle");
    }

    public override void LeaveState(State nextState)
    {

    }

    public override void OnUpdate()
    {

    }
}
