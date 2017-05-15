using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingChain : MonoBehaviour {
    private List<LineRenderer> line;
    private List<Vector3> lightingPos;

    public List<GameObject> target;
    public Vector3 startPos;
    public Vector3 endPos;

    public float width = 0.25f;
    public float range = 5;//射程
    public float moveSpeed = 2;//法术移动速度
    public float lifeTime = 2;//法术持续时间（包括移动）
    public float radius = 0.5f;//偏离半径
    public float segment = 0.2f;//分段数
    public float endOffset = 0.5f;//目标点偏差

    private int index;
    private float timer;

    void Start()
    {
        init();
        index = 0;
        timer = 0;
    }

    public void init()
    {
        line = new List<LineRenderer>(GetComponentsInChildren<LineRenderer>());
        for (int i = 0; i < line.Count; i++)
        {
            line[i].positionCount = 0;
            line[i].startWidth = line[i].endWidth = width;
        }
        lightingPos = new List<Vector3>();
        startPos = transform.InverseTransformPoint(startPos);
        endPos = transform.InverseTransformPoint(endPos);
        
    }

    void Update()
    {
        if (timer < lifeTime)
        {
            for (int i = 0; i < line.Count; i++)
            {
                lightingPos.Clear();
                ConnectChain(line[i], startPos, endPos);
                for (int j = 0; j < index; j++)
                {
                    Vector3 nextStartPos = transform.InverseTransformPoint(target[j].transform.position);
                    Vector3 nextEndPos = transform.InverseTransformPoint(target[j + 1].transform.position);
                    if ((nextStartPos - nextEndPos).magnitude < range / 2)
                    {
                        lightingPos.Add(nextStartPos);//使lineRender的转折后始终面向摄像机
                        ConnectChain(line[i], nextStartPos, nextEndPos);
                    }
                }
            }
        }
        else
        {
            EndMagic();
            Destroy(gameObject);
        }
        timer += Time.deltaTime;
    }

    public void ConnectChain(LineRenderer line, Vector3 start, Vector3 end)
    {
        Vector3 endInRange = start + Vector3.ClampMagnitude(end - start, range);
        float lerpArgv = Mathf.Clamp01(timer * moveSpeed);

        LightingAnim(line, start,
            Vector3.Lerp(start, endInRange, lerpArgv),
            Mathf.Lerp(0, radius, lerpArgv));
        if (lerpArgv == 1)
        {
            if (index + 1 < target.Count)
            {
                index++;
            }
        }
    }

    public void LightingAnim(LineRenderer line, Vector3 start, Vector3 end, float radius)
    {
        Vector3 endTemp = end + new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f) * endOffset;
        GetLightingPos(start, endTemp, radius);
        lightingPos.Add(endTemp);
        if ((endPos - start).magnitude <= range)
        {//射程内
            HitAnim();
        }
        line.positionCount = lightingPos.Count;
        line.SetPositions(lightingPos.ToArray());
    }

    public void HitAnim()
    {

    }

    public void GetLightingPos(Vector3 start, Vector3 end, float delta)
    {
        if (delta < segment)
        {
            lightingPos.Add(start);
        }
        else
        {
            Vector3 midPos = (start + end) / 2 + new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f) * delta;
            GetLightingPos(start, midPos, delta / 2);
            GetLightingPos(midPos, end, delta / 2);
        }
    }

    public void EndMagic()
    {
        GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerBehavior>().ToDrawRuneState();
    }
}
