using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawEnchantmentState : State {
    private Transform player;

    public DrawEnchantmentState(Transform player)
    {
        this.player = player;
    }

    public override int GetStateId()
    {
        return (int)PlayerState.DrawEnchantmentState;
    }

    public override void EnterState(State preState)
    {
        Debug.Log("Draw Enchantment");
    }

    public override void LeaveState(State nextState)
    {

    }

    public override void OnUpdate()
    {

    }
}
