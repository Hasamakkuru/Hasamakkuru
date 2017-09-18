using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPos {
	public Vector3[] pos;
}

public static class ObjPosEx {
	public static ObjectPos GetObjPos(this TextAsset value)
	{
		string json = value.text;
		ObjectPos op = JsonUtility.FromJson<ObjectPos>(json);
		return op;
	}
}
