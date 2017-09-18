using System.Collections;
using UnityEngine;
using Player;

public class Acceleration : MonoBehaviour {

	// Use this for initialization
	private PlayerController pc;
	void Start () {
		pc = GetComponent<PlayerController>();
		pc.Accel += pc.Acceleration;
		Vector3 nowPos = transform.position;
		EffectController.Instance.EffectGenerate(pc.PlayerNum, 3, nowPos, 1, pc.State);
		if(pc.State == PlayerState.normal)
			pc.State = PlayerState.buff;
		StartCoroutine(EffectExit(pc.ReleaseTime));
	}
	
	IEnumerator EffectExit(float delay)
	{
		yield return new WaitForSeconds(delay);
        pc.EffectExit();
		Destroy(this);
	}
}
