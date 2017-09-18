using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustEffect : MonoBehaviour {
       
    private GameObject dustObject;
    private ParticleSystem Dust;
    [SerializeField]
    private bool isDust = false;

    public bool IsDust
    {        
        set { isDust = value; }
    }

    void Awake()
    {
        dustObject = gameObject.transform.FindChild("Dustproof01").gameObject;
    }
    // Use this for initialization
    void Start () {

        dustObject.transform.position = gameObject.transform.position;
        dustObject.SetActive(false);
        
    }
	
	// Update is called once per frame
	void Update () {
        //エフェクトが再生終了したらエフェクトオブジェクトを非表示
        if (!Dust.isPlaying)
            dustObject.SetActive(false);
	}

    //エフェクト再生
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground" && isDust)
        {
            dustObject.SetActive(true);
            Dust = dustObject.GetComponent<ParticleSystem>();
            Dust.Play(withChildren:true);
            isDust = false;
        }

        
    }
}
