using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecterController : MonoBehaviour {
	[SerializeField,Header("生成までの時間"), Tooltip("改変可")] float _maxTime;
    [SerializeField,Header("最大個数"), Tooltip("改変可")] int _maxCount;
    [SerializeField,Header("横に動く量"), Tooltip("改変可")] float _moveX;
    [SerializeField,Header("縦に動く量"), Tooltip("改変可")] float _moveZ;
	[SerializeField,Header("顕界可能時間"), Tooltip("改変可")] float _lifeTime;
	[SerializeField,Header("ジャックポットの顕界時間"), Tooltip("改変可")] float _jackLife;
	[SerializeField,Header("普通の幽霊"), Tooltip("改変禁止")] GameObject _specter;
	[SerializeField,Header("ジャックポット"), Tooltip("改変禁止")] GameObject _jackSpecter;
	[System.NonSerialized] public int jackpotPoint;
	private TimeCheck _tc;
	private List<GameObject> _specters = new List<GameObject>();
	private int _sCount;
	private bool _once;

	// Use this for initialization
	void Start () {
		_tc = GetComponent<TimeCheck>();
		_sCount = 0;
		_once = false;
		SpecterInit();
		StartCoroutine(CreateSpecter());
	}
	
	// Update is called once per frame
	void Update () {
		if(_tc.TimeCount <= 30 && _once == false)
		{
			_once = true;
			JackPot();
		}
	}

	void SpecterInit()
	{
		for(int i = 0; i < _specter.transform.childCount; i++)
		{
			SpecterType sType = _specter.transform.GetChild(i).GetComponent<SpecterType>();
			if(i == 0)
				sType.SpecterNum = 0;
			else
				sType.SpecterNum = 1;
			sType.MoveX = _moveX;
			sType.MoveZ = _moveZ;
			sType.LifeTime = _lifeTime;
			_specters.Add(_specter.transform.GetChild(i).gameObject);
			_specter.transform.GetChild(i).gameObject.SetActive(false);
		}
		_specters[0].SetActive(true);
		_sCount++;
		SpecterType jack = _jackSpecter.GetComponent<SpecterType>();
		jack.SpecterNum = 2;
		jack.MoveX = _moveX * 2;
		jack.MoveZ = _moveZ * 2;
		jack.LifeTime = _jackLife;
		_jackSpecter.SetActive(false);
	}

	void JackPot()
	{
		int rn = Random.Range(1, 101);
		if(rn <= 10)
			_jackSpecter.SetActive(true);
	}

	IEnumerator CreateSpecter()
	{
		while(true)
		{
			yield return new WaitForSecondsRealtime(_maxTime);
			if(_sCount >= _maxCount) 
				yield break;
			else if(_sCount < _maxCount)
			{
				_specters[_sCount].SetActive(true);
				float x = (float)Random.Range(-Configs.Instance.width, Configs.Instance.width + 1);
				float y = 0.5f;
				float z = (float)Random.Range(-Configs.Instance.height, Configs.Instance.height + 1);
				_specters[_sCount].transform.localPosition = new Vector3(x, y, z);
				_sCount++;
			}
		}
	}
}
