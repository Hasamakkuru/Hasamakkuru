using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GeneratePos : ScriptableObject {
	[SerializeField,Header("オブジェクトの初期位置"), Tooltip("改変可")] List<Vector3> _gPos;
	public List<Vector3> pPos {
		get { return _gPos; }
	}
}

#if UNITY_EDITOR
public class CreateGeneratePos : MonoBehaviour {
	[MenuItem("Assets/Create/GeneratePos")]
	public static void CreateAsset()
	{
		CreateAsset<GeneratePos>();
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