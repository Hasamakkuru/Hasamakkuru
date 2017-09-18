using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBarrier : MonoBehaviour {

	private Material _material;
	private Color _baseColor;
	private Color _flashColor;

	public Color baseColor {
		get { return _baseColor; }
		set { _baseColor = value; }
	}

	public Color flashColor {
		get { return _flashColor; }
		set { _flashColor = value; }
	}
	// Use this for initialization
	void Start () {
		SetMaterial();
	}
	
	void SetMaterial() {
		_material = Instantiate(GetComponent<Renderer>().material);
		MaterialUtils.Emission(_material, true);
		_material.color = _baseColor;
		_material.SetColor("_EmissionColor", _flashColor);
		GetComponent<Renderer>().material = _material;
	}
	void OnCollisionEnter(Collision col) {
		if(col.gameObject.tag == "Block" || col.gameObject.tag == "Player") {
			var newC = _material.color;
			_material.color = new Color(newC.r, newC.g, newC.b, 1f);
		}
	}

	void OnCollisionExit(Collision col) {
		if(col.gameObject.tag == "Block" || col.gameObject.tag == "Player") {
			var newC = _material.color;
			_material.color = new Color(newC.r, newC.g, newC.b, 0f);
		}
	}
}
