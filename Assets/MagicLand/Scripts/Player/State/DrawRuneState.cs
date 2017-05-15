using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRuneState : State
{
    private Transform player;
    private PlayerBehavior playerBehavior;
    //画线
    private float deviation = 50f;//绘制时手与点的最大偏差
    private List<Vector3> standardPoint;
    private LineRenderer line;

    private int lineCount;//提示线段上点个数
    private int lastIndex;//上一个绘制过的点
    

    public DrawRuneState(Transform player)
    {
        this.player = player;
        playerBehavior = player.GetComponent<PlayerBehavior>();
        line = playerBehavior.runeLine;
    }

    public override int GetStateId()
    {
        return (int)PlayerState.DrawRuneState;
    }

    public override void EnterState(State preState)
    {
        Debug.Log("Draw Rune");
        standardPoint = new List<Vector3>();
        foreach (RectTransform child in playerBehavior.standardRune.GetComponent<RectTransform>())
        {
            standardPoint.Add(child.anchoredPosition);
        }
        lineCount = 0;
        lastIndex = -1;
    }

    public override void LeaveState(State nextState)
    {
        line.positionCount = 0;
    }

    public override void OnUpdate()
    {
        DrawRune();
    }

    private void DrawRune()
    {
        if (HandInfo.handPressed)
        { //手势处于点击状态时开始绘制点
            for (int i = lastIndex + 1; i < standardPoint.Count; i++)
            { //当手与可绘制且未绘制的点接近时绘制点
                if (Vector3.Distance(HandInfo.handPosition_Screen, standardPoint[i]) < deviation)
                {
                    line.positionCount = lineCount + 2; //已绘制点 + 手
                    line.SetPosition(lineCount++, standardPoint[i]);
                    lastIndex = i;
                }
            }
            if (lastIndex >= 0)
            { //当绘制开始时提示线段的末端为手
                line.positionCount = lineCount + 1;
                line.SetPosition(lineCount, HandInfo.handPosition_Screen);
            }
            //绘制完最后一个点后切换至攻击阶段
            if (lastIndex == standardPoint.Count - 1)
            {
                line.positionCount -= 1; //末端点为最后绘制点
                
                playerBehavior.ToAttackState((float)line.positionCount / standardPoint.Count);
            }
        }
        else
        { //手势处于松开状态时若已进行绘制则强制完成绘制并切换至攻击阶段
            line.positionCount = lineCount; //末端点为最后绘制点
            if (lineCount > 0)
            {
                playerBehavior.ToDrawRuneState();
            }
        }
    }
}
