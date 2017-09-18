using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class Stun : MonoBehaviour {

	private GameObject obj;

	private void OnTriggerEnter(Collider collider){
		if (collider.gameObject.tag == "Player") 
		{
			obj = collider.gameObject;
			obj.GetComponent<PlayerController>().Stan = true;
		}
	}
}

