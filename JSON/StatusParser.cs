using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum CharType
{
	Default,
	Power,
	Speed,
	Normal
}

public static class CharTypeEx {
	public static CharType ChangeType(this string value) {
		CharType cType = CharType.Default;
		if(value == "Default")
			cType = CharType.Default;
		if(value == "Power")
		 	cType = CharType.Power;
		if(value == "Speed")
			cType = CharType.Speed;
		if(value == "Normal")
			cType = CharType.Normal;
		 return cType;
	}
}

public class StatusParser : MonoBehaviour {
	[SerializeField] List<InputField> inputFields = new List<InputField>();
	private string path;
	[SerializeField] ToggleGroup toggleGroup;
	private CharStatus _cs;
	private string[] pathList = {
		"/Resources/JSON/defaultStatus.json",
		"/Resources/JSON/powerStatus.json",
		"/Resources/JSON/speedStatus.json",
		"/Resources/JSON/normalStatus.json"
	};
	private CharType charType;
	// Use this for initialization
	void Start () {
		PathInit();
		Load();
	}

	void PathInit()
	{
		for(int i = 0; i < pathList.Length; i++)
			pathList[i] = Application.dataPath + pathList[i];
        charType = CharType.Default;
        path = pathList[(int)charType];
    }

	public void DataInput()
	{
		_cs.accel = float.Parse(inputFields[0].text);
		_cs.acceleration = float.Parse(inputFields[1].text);
		_cs.throwPower = float.Parse(inputFields[2].text);
		_cs.raisePower = float.Parse(inputFields[3].text);
		_cs.releaseTime = float.Parse(inputFields[4].text);
		_cs.invisible = float.Parse(inputFields[5].text);
		_cs.leverMax = int.Parse(inputFields[6].text);
		Save();
	}

	void Load()
	{
		using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)){
			using(StreamReader sr = new StreamReader(fs)){
				_cs = JsonUtility.FromJson<CharStatus>(sr.ReadToEnd());
			}
		}
		inputFields[0].text = _cs.accel.ToString();
		inputFields[1].text = _cs.acceleration.ToString();
		inputFields[2].text = _cs.throwPower.ToString();
		inputFields[3].text = _cs.raisePower.ToString();
		inputFields[4].text = _cs.releaseTime.ToString();
		inputFields[5].text = _cs.invisible.ToString();
		inputFields[6].text = _cs.leverMax.ToString();
	}
	
	void Save()
	{
		using(FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write)){
			using(StreamWriter sw = new StreamWriter(fs)){
				sw.WriteLine(JsonUtility.ToJson(_cs));
			}
		}
		Debug.Log("書き換え完了！");
		ShowDebug(_cs);
	}

	void ShowDebug(CharStatus cs)
	{
		Debug.Log(cs.accel);
		Debug.Log(cs.acceleration);
		Debug.Log(cs.throwPower);
		Debug.Log(cs.raisePower);
		Debug.Log(cs.releaseTime);
		Debug.Log(cs.invisible);
		Debug.Log(cs.leverMax);
	}

	public void ChangePath()
	{
		var cType = toggleGroup.ActiveToggles()
			.First().GetComponentsInChildren<Text>()
			.First(t => t.name == "Label").text;
		charType = cType.ChangeType();
		Debug.Log(charType);
		path = pathList[(int)charType];
		Load();
	}
}
