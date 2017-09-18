using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteEffect : MonoBehaviour {

    //エフェクトの再生が終わったら削除
    private ParticleSystem particle;
    void Awake()
    {
        particle = gameObject.GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        StartCoroutine(DestroyParticle());
    }
	
    private IEnumerator DestroyParticle()
    {
        yield return new WaitWhile(() => particle.IsAlive(false));
        gameObject.SetActive(false);
    }
}
