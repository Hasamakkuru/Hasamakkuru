using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMove : MonoBehaviour {
	private Renderer ren;
	private float wrap;
	private Vector2 offset;
	[SerializeField,Range(0.01f, 0.5f)] float delay;
	// Use this for initialization
	void Start () {
		ren = GetComponent<Renderer>();
		wrap = 0;
		offset = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () {
		wrap = -Mathf.Repeat (Time.time * delay, 1);
		offset = new Vector2(wrap, wrap);
		ren.sharedMaterial.SetTextureOffset("_MainTex", offset);
	}
}
