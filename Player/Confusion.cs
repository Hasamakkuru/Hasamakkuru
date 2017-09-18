using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

[DisallowMultipleComponent]
public class Confusion : MonoBehaviour {
	private PlayerController pc;
	// Use this for initialization
	void Start () {
		pc = GetComponent<PlayerController>();
		pc.IsConfusion = true;
		Vector3 nowPos = gameObject.transform.position;
		nowPos.y += 5;
		EffectController.Instance.EffectGenerate(pc.PlayerNum, 2, nowPos);
		StartCoroutine(EffectExit(pc.ReleaseTime));
	}
	
	IEnumerator EffectExit(float delay)
	{
		yield return new WaitForSeconds(delay);
		pc.EffectExit();
		Destroy(this);
	}
}
