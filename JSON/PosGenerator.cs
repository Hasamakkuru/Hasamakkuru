using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PosGenerator : MonoBehaviour {
	[SerializeField] ToggleGroup toggleGroup;
	private int[] wList = {	16, 14, 25, 14 };
	private int[] hList = { 13, 14, 14, 14 }; 
	private int width;
	private int height;
	private string path;
	private string[] pathList = {
		"/Resources/JSON/cyber.json",
		"/Resources/JSON/flat.json",
		"/Resources/JSON/temple.json",
		"/Resources/JSON/cemetery.json"
	};
	private ObjectPos _op;

	// Use this for initialization
	void Start () {
		PathInit();
	}

	void PathInit()
	{
		for(int i = 0; i < pathList.Length; i++)
			pathList[i] = Application.dataPath + pathList[i];
		path = pathList[0];
		width = wList[0];
		height = hList[0];
	}

	public void Generate()
	{
		_op = new ObjectPos();
		_op.pos = new Vector3[20];
		for(int i = 0; i < _op.pos.Length; i++)
		{
			float rX = (float)Random.Range(-width, width + 1);
			float rZ = (float)Random.Range(-height, height + 1);
			Vector3 p = new Vector3(rX, 0.5f, rZ);
			_op.pos[i] = p;
		}
		Save();
	}
	
	void Save()
	{
		using(FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write)){
			using(StreamWriter sw = new StreamWriter(fs)){
				sw.WriteLine(JsonUtility.ToJson(_op));
			}
		}
		Debug.Log("書き換え完了！");
		ShowDebug(_op);
	}

	public void ChangePath()
	{
		var type = toggleGroup.ActiveToggles()
			.First().GetComponentsInChildren<Text>()
			.First(t => t.name == "Label").text;
		if(type == "電脳"){
			path = pathList[0];
			width = wList[0];
			height = hList[0];
		}
		else if(type == "フラット"){
			path = pathList[1];
			width = wList[1];
			height = hList[1];
		}
		else if(type == "神殿"){
			path = pathList[2];
			width = wList[2];
			height = hList[2];
		}
		else if(type == "墓地"){
			path = pathList[3];
			width = wList[3];
			height = hList[3];
		}
	}

	void ShowDebug(ObjectPos op)
	{
		foreach(var value in op.pos)
		{
			Debug.Log(value);
		}
	}
}
