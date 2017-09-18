using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierColor : MonoBehaviour {
	[SerializeField, Header("元の色。アルファ値は0固定"), Tooltip("いじってよし")]
	private Color _baseColor;
	[SerializeField, Header("光る色。元の色より暗い方がいい？要検証"), Tooltip("いじってよし")]
	private Color _flashColor;

	// Use this for initialization
	void Start () {
		int count = gameObject.transform.childCount;
		for(int i = 0; i < count; i++) {
			WallBarrier w = transform.GetChild(i).GetComponent<WallBarrier>();
			w.baseColor = _baseColor;
			w.flashColor = _flashColor;
		}
	}
}
