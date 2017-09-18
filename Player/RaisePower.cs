using System.Collections;
using UnityEngine;
using Player;

public class RaisePower : MonoBehaviour
{
    private PlayerController pc;
    // Use this for initialization
    void Start()
    {
        pc = GetComponent<PlayerController>();
        pc.ThrowPower += pc.RaisePower;
        Vector3 nowPos = transform.position;
		EffectController.Instance.EffectGenerate(pc.PlayerNum, 3, nowPos, 0, pc.State);
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
