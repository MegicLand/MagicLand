using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public virtual int GetStateId()//获取状态id
    {
        return 0;
    }
    public virtual void EnterState(State preState) { }//进入状态
    public virtual void LeaveState(State nextState) { }//离开状态
    public virtual void OnUpdate() { }//更新状态
    public virtual void OnFixedUpdate() { }//更新状态
    public virtual void OnLateUpdate() { }//更新状态
    public virtual void OnCollisionEnter2D(Collision2D collision) { }//进入碰撞器
    public virtual void OnCollisionStay2D(Collision2D collision) { }//在碰撞器中
    public virtual void OnCollisionExit2D(Collision2D collision) { }//离开碰撞器
    public virtual void OnTriggerEnter2D(Collider2D collider) { }//进入触发器
    public virtual void OnTriggerStay2D(Collider2D collider) { }//在触发器中
    public virtual void OnTriggerExit2D(Collider2D collider) { }//离开触发器
}

public class StateMachine {
    private Dictionary<int, State> stateDic = new Dictionary<int, State>();
    private State currentState;
    private int currentStateId;

    public State CurrentState
    {//获取当前状态
        get { return currentState; }
    }
    public int CurrentStateId
    {//获取当前状态id
        get { return currentState.GetStateId(); }
    }

    public void RegisterState(State state)
    {//如果状态没有被注册过，注册状态
        if (state == null || stateDic.ContainsKey(state.GetStateId()))
        {
            return;
        }
        stateDic.Add(state.GetStateId(), state);
    }
    
    public void SwitchState(int stateId)
    {//切换状态
        if (currentState != null && stateId == currentStateId)
        {
            return;
        }
        State newState;
        State oldState;
        stateDic.TryGetValue(stateId, out newState);
        if (newState != null)
        {
            if (currentState != null)
            {
                currentState.LeaveState(newState);
            }
            oldState = currentState;
            currentState = newState;
            currentStateId = newState.GetStateId();
            currentState.EnterState(oldState);
        }
    }

    public void ReloadState(int stateId)
    {
        if (currentState != null &&  stateId!= currentStateId)
        {
            return;
        }
        State newState;
        stateDic.TryGetValue(stateId, out newState);
        if (newState != null)
        {
            if (currentState != null)
            {
                currentState.LeaveState(newState);
            }
            currentState = newState;
            currentStateId = newState.GetStateId();
            currentState.EnterState(newState);
        }
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }

    public void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnFixedUpdate();
        }
    }

    public void LateUpdate()
    {
        if (currentState != null)
        {
            currentState.OnLateUpdate();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState != null)
        {
            currentState.OnCollisionEnter2D(collision);
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (currentState != null)
        {
            currentState.OnCollisionStay2D(collision);
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (currentState != null)
        {
            currentState.OnCollisionExit2D(collision);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (currentState != null)
        {
            currentState.OnTriggerEnter2D(collider);
        }
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        if (currentState != null)
        {
            currentState.OnTriggerStay2D(collider);
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (currentState != null)
        {
            currentState.OnTriggerExit2D(collider);
        }
    }
}
