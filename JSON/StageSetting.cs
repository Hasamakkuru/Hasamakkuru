using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StageSetting : ScriptableObject {
	[SerializeField,Header("キャラの初期出現位置"), Tooltip("改変可")] List<Vector3> _pPos;
	[SerializeField,Header("ゲーム終了時の間"), Tooltip("改変可")] float _delay01 = 1f;
	[SerializeField,Header("ボタン表示の遅延時間"), Tooltip("改変可")] float _delay02 = 1.5f;
	[SerializeField,Header("リスポーンの範囲_横"), Tooltip("改変可")] int _width;
	[SerializeField,Header("リスポーンの範囲_縦"), Tooltip("改変可")] int _height;
	[SerializeField,Header("BGM"), Tooltip("改変可")] AudioClip _bgm;
	[SerializeField,Header("ステージ番号"), Tooltip("改変禁止")] int _stageNum;
	public List<Vector3> pPos {
		get { return _pPos; }
	}
	public float delay01 {
		get { return _delay01; }
	}
	public float delay02 {
		get { return _delay02; }
	}
	public int width {
		get { return _width; }
	}
	public int height {
		get { return _height; }
	}
	public AudioClip bgm {
		get { return _bgm; }
	}
	public int stageNum {
		get { return _stageNum; }
	}
}

#if UNITY_EDITOR
public class CreateStageSetting : MonoBehaviour {
	[MenuItem("Assets/Create/StageSetting")]
	public static void CreateAsset()
	{
		CreateAsset<StageSetting>();
	}

	public static void CreateAsset<T>() where T : ScriptableObject {
		T item = ScriptableObject.CreateInstance<T>();
		string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/JSON/" + typeof(T) + ".asset");
		ProjectWindowUtil.CreateAsset(item, path);

		EditorUtility.FocusProjectWindow();
		Selection.activeObject = item;
	}
}
#endif