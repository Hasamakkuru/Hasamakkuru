using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class GraveHand : MonoBehaviour {
	private HandController _hc;
	private bool _isRun = true;
	// Use this for initialization
	void OnEnable()
	{
		if(_isRun == false)
		{
			_isRun = true;
			StartCoroutine(PosChange());
		}
	}

	void Start () {
		_hc = GameObject.Find("GameController").GetComponent<HandController>();
		StartCoroutine(PosChange());
	}
	
	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "Player") 
		{
			_isRun = false;
			collision.gameObject.GetComponent<PlayerController>().GraveCatched();
			_hc.HandCount -= 1;
			this.gameObject.SetActive(false);
		}
	}

	IEnumerator PosChange()
	{
		yield return new WaitForSecondsRealtime(10);
		while (_isRun == true)
		{
			yield return new WaitForSecondsRealtime(5);
			float x = (float)Random.Range(-Configs.Instance.width, Configs.Instance.width + 1);
			float z = (float)Random.Range(-Configs.Instance.height, Configs.Instance.height + 1);
			transform.position = new Vector3(x, transform.position.y, z);
		}
		yield break;
	}
}
