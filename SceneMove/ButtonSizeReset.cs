using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSizeReset : MonoBehaviour {
	private Vector3 _size;
	void OnEnable()
	{
		_size = this.gameObject.GetComponent<RectTransform>().localScale;
	}
	// Use this for initialization
	void Start () {
		_size = this.gameObject.GetComponent<RectTransform>().localScale;
	}
	
	void OnDisable()
	{
		this.gameObject.GetComponent<RectTransform>().localScale = _size;
	}
}
