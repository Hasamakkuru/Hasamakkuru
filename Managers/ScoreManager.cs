using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
class Nummbers {
	[SerializeField]
	private List<Sprite> _nums = new List<Sprite>();
	public List<Sprite> nums {
		get { return _nums; }
	}
}
[System.Serializable]
public class PlayerScore {
    [SerializeField]
    private List<Image> _scoreImage = new List<Image>();
    public List<Image> scoreImage {
        get { return _scoreImage; }
    }
}

public class ScoreManager : SingletonMono<ScoreManager> {
	// 0がタイム、1〜4がプレイヤー
	[SerializeField] Nummbers _nummber;
    [SerializeField] Nummbers _scores;
    [SerializeField] List<Sprite> _sign = new List<Sprite>();
    private List<PlayerScore> _playerScore = new List<PlayerScore>();
    private List<GameObject> _parents = new List<GameObject>();
    private List<PlayerScore> _plusScore = new List<PlayerScore>();
    private List<Image> _pSigns = new List<Image>();
    [SerializeField] float _delayTime;

    public List<GameObject> parents { 
        get { return _parents; }
        set { _parents = value; }
    }

    public List<PlayerScore> playerScore {
        get { return _playerScore; }
        set { _playerScore = value; }
    }

    public List<PlayerScore> plusScore {
        get { return _plusScore; }
        set { _plusScore = value; }
    }

    public List<Image> pSigns {
        get { return _pSigns; }
        set { _pSigns = value; }
    }

	void Awake()
    {
        if (Instance != this)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void NumUpdate(List<Image> nums, int score, bool isTime)
    {
        var save = score;
        foreach(var num in nums)
        {
            // trueが時間、falseがスコア
            if(isTime)
                num.sprite = _nummber.nums[save % 10];
            else
                num.sprite = _scores.nums[save % 10];
            save /= 10;
        }
    }
    
	public void TimeUpdate(List<Image> nums, int score)
	{
		var _save = score;
        if (_save <= 0)
            foreach(var n in nums)
                n.sprite = _nummber.nums[0];
        else
            NumUpdate(nums, score, true);
	}

    public void ScoreUpdate(int score, int plusPoint, int pNum)
	{
        pNum -= 1;
		if(plusPoint == 0)
            _parents[pNum].SetActive(false);
        else
    		_parents[pNum].gameObject.SetActive(true);
        if(plusPoint < 0) {
            // マイナスを表示
            _pSigns[pNum].sprite = _sign[1];
        }
        else {
            // プラスを表示
            _pSigns[pNum].sprite = _sign[0];
        }
        NumUpdate(_plusScore[pNum].scoreImage, Mathf.Abs(plusPoint), false);
		var _save = score + plusPoint;
        if (_save <= 0)
            _save = 0;
        if (_save >= 9999)
            _save = 9999;
		StartCoroutine(ViewUpdate(_parents[pNum], _save, _playerScore[pNum].scoreImage));
	}

    public void Clear() {
        _playerScore.Clear();
        _plusScore.Clear();
        _parents.Clear();
        _pSigns.Clear();
    }
	IEnumerator ViewUpdate(GameObject view, int score, List<Image> nums)
	{
		yield return new WaitForSecondsRealtime(_delayTime);
		view.SetActive(false);
		if (score <= 0)
			foreach(var num in nums)
            	num.sprite = _scores.nums[0];
        else
            NumUpdate(nums, score, false);
		yield break;
	}
}
