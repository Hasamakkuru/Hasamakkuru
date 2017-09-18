using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatAnimController : MonoBehaviour {
	private TimeCheck _tc;
	private Animator _animator;
	private bool _countDown = false;
	// Use this for initialization
	void Start () {
		_tc = GameObject.FindGameObjectWithTag("GameController").GetComponent<TimeCheck>();
		_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!_countDown && _tc.ViewTime <= 10)
		{
			_countDown = true;
			_animator.SetBool("CountDown", true);
		}
	}
}
