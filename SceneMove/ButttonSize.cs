using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ButttonSize {
	static float scale = 1.2f;
	public static GameObject SizeUp(this GameObject value)
	{
		value.transform.localScale *= scale;
		return value;
	}

	public static GameObject SizeDown(this GameObject value)
	{
		value.transform.localScale /= scale;
		return value;
	}
}
