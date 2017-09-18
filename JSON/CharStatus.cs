using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharStatus {
	public float accel; // 移動の加速度
	public float acceleration;
	public float throwPower; // 投げる力
	public float raisePower;
    public float releaseTime; // 強化解除の時間
	public float invisible;
	public int leverMax;
}

public static class Status {
	public static CharStatus GetParams(this TextAsset value)
	{
		string json = value.text;
		CharStatus cs = JsonUtility.FromJson<CharStatus>(json);
		return cs;
	}
}
