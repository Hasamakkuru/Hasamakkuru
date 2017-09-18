using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class DownPower : MonoBehaviour {
	private PlayerController pc;
	// Use this for initialization
	void Start () {
		pc = GetComponent<PlayerController>();
		pc.ThrowPower -= pc.RaisePower;
		if(pc.ThrowPower <= pc.ThrowLower)
			pc.ThrowPower = 3;
		EffectController.Instance.EffectGenerate(pc.PlayerNum, 4, new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), 0, pc.State);
		if(pc.State == PlayerState.normal)
			pc.State = PlayerState.debuff;
		StartCoroutine(EffectExit(pc.ReleaseTime));
	}
	
	IEnumerator EffectExit(float delay)
	{
		yield return new WaitForSeconds(delay);
		pc.EffectExit();
		Destroy(this);
	}
}
