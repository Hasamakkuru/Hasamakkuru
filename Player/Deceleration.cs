using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class Deceleration : MonoBehaviour {
	private PlayerController pc;
	// Use this for initialization
	void Start () {
		pc = GetComponent<PlayerController>();
		pc.Accel -= pc.Acceleration;
		if(pc.Accel <= pc.AccelLower)
			pc.Accel = 0.05f;
		EffectController.Instance.EffectGenerate(pc.PlayerNum, 4, new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), 1, pc.State);
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
