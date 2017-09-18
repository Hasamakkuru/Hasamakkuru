using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StageConfig : ScriptableObject {

	private int _playerNums = 4;
    private int _stageNum = 0;
    private string[] _PLAYERID =
    {
        "Player01",
        "Player02",
        "Player03",
        "Player04"
    };
    private bool _charaSelect = false;
    private int _width;
    private int _height;

	private List<int> _pModels = new List<int>();

	public int playerNums {
		get { return _playerNums; }
		set { _playerNums = value; }
	}

	public int stageNum {
		get { return _stageNum; }
		set { _stageNum = value; }
	}

	public string[] PLAYERID {
		get { return _PLAYERID; }
	}

	public bool charaSelect {
		get { return _charaSelect; }
		set { _charaSelect = value; }
	}

	public int width {
		get { return _width; }
		set { _width = value; }
	}

	public int height {
		get { return _height; }
		set { _height = value; }
	}

	public List<int> pModels {
		get { return _pModels; }
		set { _pModels = value; }
	}
}

#if UNITY_EDITOR
public class CreateConfig : MonoBehaviour {
	[MenuItem("Assets/Create/Config")]
	public static void CreateAsset()
	{
		CreateAsset<StageConfig>();
	}

	public static void CreateAsset<T>() where T : ScriptableObject {
		T item = ScriptableObject.CreateInstance<T>();
		string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/" + typeof(T) + ".asset");
		ProjectWindowUtil.CreateAsset(item, path);

		EditorUtility.FocusProjectWindow();
		Selection.activeObject = item;
	}
}
#endif

public class Configs : MonoBehaviour {
	static private readonly string RESOURCE_PATH = "StageConfig";
	static private StageConfig _instance = null;
	static public StageConfig Instance {
		get {
			if(_instance == null)
			{
				var asset = Resources.Load<StageConfig>(RESOURCE_PATH);
				if(asset == null)
                {
                    Debug.AssertFormat( false, "Missing ParameterTable! path={0}", RESOURCE_PATH );
                }
				_instance = asset;
			}
			return _instance;
		}
	}
	public static void ConfigReset(bool isReset = true)
	{
        _instance.stageNum = 0;
        _instance.width = 0;
        _instance.height = 0;
        _instance.pModels.Clear();
        if (isReset)
            _instance.playerNums = 0;
	}

	public static void AppQuit()
	{
		_instance.playerNums = 4;
		_instance.stageNum = 0;
		_instance.width = 0;
		_instance.height = 0;
        _instance.pModels.Clear();
	}
}