using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour {
	//holdEffect
	public GameObject hEPyroblast;
	public GameObject hECoreOfGaea;
	public GameObject hELightingChain;
	public GameObject hEFrostLance;
	public GameObject hERockWall;
	public GameObject hEMageShield;

	//AttackEffect
	public GameObject mPyroblast;
	public GameObject mCoreOfGaea;
	public GameObject mLightingChain;
	public GameObject mFrostLance;
	public GameObject mRockWall;
	public GameObject mMageShield;

	//PresentEffect
	private GameObject runeMagic;//Present Rune Attack Effect
	private GameObject runeHoldEffect;//Present Rune Hold Effect
	private GameObject enchMagic;//Present Enchantment Attack Effect
	private GameObject enchHoldEffect;//Present Enchantment Hold Effect

	public void holdRune (RuneCategory type){
		switch (type) {
		case RuneCategory.Pyroblast:
			break;
		case RuneCategory.CoreOfGaea:
			break;
		case RuneCategory.LightingChain:
			runeHoldEffect = GameObject.Instantiate(hELightingChain);
			break;
		case RuneCategory.FrostLance:
			break;
		case RuneCategory.RockWall:
			break;
		case RuneCategory.MageShield:
			break;
		default:
			break;
		}
	}

	public void generateRune (RuneCategory type, Vector3 handPosition, Vector3 targetPos){
		switch (type) {
		case RuneCategory.Pyroblast:
			break;
		case RuneCategory.CoreOfGaea:
			break;
		case RuneCategory.LightingChain:
			runeMagic = GameObject.Instantiate(mPyroblast, handPosition, Quaternion.Euler(Vector3.zero));
			LightingChain lightingChain = runeMagic.GetComponent<LightingChain>();
			lightingChain.startPos = handPosition;
			lightingChain.endPos = targetPos;
			break;
		case RuneCategory.FrostLance:
			break;
		case RuneCategory.RockWall:
			break;
		case RuneCategory.MageShield:
			break;
		default:
			break;
		}
	}

	public void holdEnchantment(EnchantmentCategory type){
		switch (type) {
		case EnchantmentCategory.StarFall:
			break;
		case EnchantmentCategory.ThunderBlast:
			break;
		default:
			break;
		}
	}

	public void generateEnchantment(EnchantmentCategory type){
		switch (type) {
		case EnchantmentCategory.StarFall:
			break;
		case EnchantmentCategory.ThunderBlast:
			break;
		default:
			break;
		}
	}


}
