using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBase : MonoBehaviour {
	public Vector3 startPos;
	public Vector3 targetPos;
	public float magicExpend;
	public float damage;
	public int lifeTime;

	public void EndMagic()
	{
		GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerBehavior>().ToDrawRuneState();
	}
}
