using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class StandardRune : MonoBehaviour {
    public float keyPointNum = 5;
    private List<GameObject> keyPoint;

    public float fadeSpeed = 2;
    private Image standardRuneImage;

	// Use this for initialization
	void Start () {
        standardRuneImage = GetComponent<Image>();
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

    public IEnumerator FadeIn()
    {
        Color startColor = new Color(standardRuneImage.color.r, standardRuneImage.color.g, standardRuneImage.color.b, 0);
        Color endColor = new Color(standardRuneImage.color.r, standardRuneImage.color.g, standardRuneImage.color.b, 1);
        float timer = 0;
        standardRuneImage.color = startColor;
        while (standardRuneImage.color.a < 1)
        {
            standardRuneImage.color = Color.Lerp(startColor, endColor, fadeSpeed * timer);
            timer += Time.deltaTime;
            yield return 0;
        }
        standardRuneImage.color = endColor;
        yield return 0;
    }

    public IEnumerator FadeOut()
    {
        Color startColor = new Color(standardRuneImage.color.r, standardRuneImage.color.g, standardRuneImage.color.b, 1);
        Color endColor = new Color(standardRuneImage.color.r, standardRuneImage.color.g, standardRuneImage.color.b, 0);
        float timer = 0;
        standardRuneImage.color = startColor;
        while (standardRuneImage.color.a > 0)
        {
            standardRuneImage.color = Color.Lerp(startColor, endColor, fadeSpeed * timer);
            timer += Time.deltaTime;
            yield return 0;
        }
        standardRuneImage.color = endColor;
        yield return 0;
    }
}
