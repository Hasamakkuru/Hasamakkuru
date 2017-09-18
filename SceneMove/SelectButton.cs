using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectButton : MonoBehaviour, ISelectHandler {
	private Button button;
	private SceneController sc;
	// Use this for initialization
	void Start () {
		button = GetComponent<Button>();
		sc = GameObject.Find("ChangeScene").GetComponent<SceneController>();
	}
	
	// Update is called once per frame
	public void OnSelect(BaseEventData eventData)
	{
		sc.beforeSelect = button;
	}
}
