using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Player;

public class NomalizeState : MonoBehaviour {
	private PlayerController pc;
	private List<RaisePower> raisePowers;
	private List<Acceleration> accelerations;
	private List<DownPower> downPowers;
	private List<Deceleration> decelerations;
	private Confusion confusion;
	// Use this for initialization
	void Start () {
		pc = GetComponent<PlayerController>();
		pc.IsGround = true;
		pc.Stan = false;
		if(GetComponent<RaisePower>() != null)
		{
			raisePowers = GetComponents<RaisePower>().ToList();
			foreach(var r in raisePowers)
			{
				pc.EffectExit();
				Destroy(r);
			}
		}
		if(GetComponent<Acceleration>() != null)
		{
			accelerations = GetComponents<Acceleration>().ToList();
			foreach(var a in accelerations)
			{
				pc.EffectExit();
				Destroy(a);
			}
		}
		if(GetComponent<DownPower>() != null)
		{
			downPowers = GetComponents<DownPower>().ToList();
			foreach(var d in downPowers)
			{
				pc.EffectExit();
				Destroy(d);
			}
		}
		if(GetComponent<Deceleration>() != null)
		{
			decelerations = GetComponents<Deceleration>().ToList();
			foreach(var d in decelerations)
			{
				pc.EffectExit();
				Destroy(d);
			}
		}
		if(GetComponent<Confusion>() != null)
		{
			confusion = GetComponent<Confusion>();
			pc.EffectExit();
			Destroy(confusion);
		}
		Destroy(this);
	}
}
