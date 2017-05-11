using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class StandardRune : MonoBehaviour {
    public float keyPointNum = 5;
    private List<GameObject> keyPoint;

	// Use this for initialization
	void Start () {
        init();
	}

    void init()
    {
        keyPoint = new List<GameObject>();
        if (transform.childCount == 0)
        {
            for (int i = 0; i < keyPointNum; i++)
            {
                GameObject temp = new GameObject("KeyPoint_" + i);
                temp.transform.parent = transform;
                keyPoint.Add(temp);
            }
        }
        else
        {
            foreach(Transform child in transform)
            {
                keyPoint.Add(child.gameObject);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < keyPointNum; i++)
        {
            Vector3 tempPos = keyPoint[i].transform.localPosition;
            tempPos.z = 0;
            keyPoint[i].transform.localPosition = tempPos;
        }
	}

    void OnDrawGizmos()
    {
        for (int i = 0; i < keyPointNum; i++)
        {
            if (i < keyPointNum - 1)
            {
                Debug.DrawLine(keyPoint[i].transform.position, keyPoint[i + 1].transform.position, Color.red);
            }
        }
    }
}
