using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;

public class PlayerBehavior : MonoBehaviour {

    public LineRenderer runeLine;//绘制符文的线
    public StandardRune standardRune;//符文提示轨迹点

    public GameObject magic;//当前要释放的魔法
    public GameObject holdEffect;//准备释放魔法特效

    private PlayerFSM fsm;
    private PlayerInfo info;

    // Use this for initialization
    void Start () {
        fsm = GetComponent<PlayerFSM>();
        info = GetComponent<PlayerInfo>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    /// <summary>
    /// 语音切换至绘制符文阶段
    /// </summary>
    public void ToDrawRuneState()
    {
        StartCoroutine(standardRune.FadeIn());
        fsm.SwitchState((int)PlayerState.DrawRuneState);
    }

    /// <summary>
    /// 语音切换至绘制结界阶段
    /// </summary>
    public void ToDrawEnchantmentState()
    {
        fsm.SwitchState((int)PlayerState.DrawEnchantmentState);
    }

    /// <summary>
    /// 绘制完符文后进入攻击阶段
    /// </summary>
    /// <param name="rate"> 绘制符文的完成度，结界默认为1 </param>
    public void ToAttackState(float rate = 1)
    {
        StartCoroutine(standardRune.FadeOut());
        fsm.SwitchState((int)PlayerState.AttackState);
    }

    /// <summary>
    /// 进入空闲状态
    /// </summary>
    public void ToIdleState()
    {
        fsm.SwitchState((int)PlayerState.IdleState);
    }

    /// <summary>
    /// 玩家死亡
    /// </summary>
    public void ToDieState()
    {
        fsm.SwitchState((int)PlayerState.DieState);
    }

    /// <summary>
    /// 若法力值足够，消耗法力并返回true；若法力不足，仅返回false
    /// </summary>
    /// <param name="amount">法力消耗量</param>
    /// <returns></returns>
    public bool ComsumeMp(float amount)
    {
        if (info.MP - amount < 0)
        {
            return false;
        }
        else
        {
            info.MP -= amount;
            return true;
        }
    }

    /// <summary>
    /// 对玩家造成伤害，HP为0时玩家死亡
    /// </summary>
    /// <param name="amount">计算后的伤害量</param>
    public void GetHurt(float amount)
    {
        if (info.HP - amount < 0)
        {
            info.HP = 0;
            ToDieState();
        }
        else
        {
            info.HP -= amount;
        }
    }

    /// <summary>
    /// 强制切换到绘制符文阶段， 将当前符文修改为目标符文
    /// </summary>
    /// <param name="next">目标符文</param>
    public void SwitchRune(RuneCategory next)
    {
        if (fsm.CurrentStateId != (int)PlayerState.DrawRuneState)
        {
            ToDrawRuneState();
        }
        info.currentRune = next;
        //获得标准符文***********************************************************************************************
        //standardRune = MagicManager.GetStandardRune(next)
        //GameObject GetStandardRune(RuneCategory next)
    }

    /// <summary>
    /// 强制切换到绘制结界阶段， 将当前符文修改为空，将当前结界修改为目标结界
    /// </summary>
    /// <param name="next">目标结界</param>
    public void SwitchEnchantment(EnchantmentCategory next)
    {
        if (fsm.CurrentStateId != (int)PlayerState.DrawEnchantmentState)
        {
            ToDrawEnchantmentState();
        }
        info.currentRune = RuneCategory.None;
        info.currentEnchantment = next;
    }
}
