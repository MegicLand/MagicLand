using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    IdleState,//Idle阶段
    DrawRuneState,//绘制符文阶段
    DrawEnchantmentState,//绘制结界阶段
    AttackState,//瞄准并释放法术阶段
    DieState//死亡阶段
}

public class PlayerFSM : MonoBehaviour
{
    private StateMachine fsm;
    public int CurrentStateId
    {
        get { return fsm.CurrentStateId; }
    }
    //初始化
    void Awake()
    {//初始化状态机
        fsm = new StateMachine();
    }

    void Start()
    {//注册状态
        RegisterState();
    }

    private void RegisterState()
    {//注册状态
        fsm.RegisterState(new IdleState(transform));
        fsm.RegisterState(new DrawRuneState(transform));
        fsm.RegisterState(new DrawEnchantmentState(transform));
        fsm.RegisterState(new AttackState(transform));
        fsm.RegisterState(new DieState(transform));
        fsm.SwitchState((int)PlayerState.DrawRuneState);//指定当前状态为Idle
    }

    public void SwitchState(int stateId)
    {//进行状态切换
        fsm.SwitchState(stateId);
    }

    public void ReloadState(int stateId)
    {//重回状态
        fsm.ReloadState(stateId);
    }

    // 更新数据
    void Update()
    {
        fsm.Update();
    }
    void FixedUpdate()
    {
        fsm.FixedUpdate();
    }
    void LateUpdate()
    {
        fsm.LateUpdate();
    }

    //碰撞
    void OnCollisionEnter2D(Collision2D collision)
    {
        fsm.OnCollisionEnter2D(collision);
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        fsm.OnCollisionStay2D(collision);
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        fsm.OnCollisionExit2D(collision);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        fsm.OnTriggerEnter2D(collider);
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        fsm.OnTriggerStay2D(collider);
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        fsm.OnTriggerExit2D(collider);
    }
}
