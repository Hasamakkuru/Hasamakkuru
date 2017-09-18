using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCheck : MonoBehaviour 
{
	[SerializeField,Header("プレイ時間"), Tooltip("改変可")] 
    private float time = 121.0f;
    [SerializeField,Header("時間の画像"), Tooltip("改変禁止"), NamedArray(new string[]{"1の位","10の位","100の位"})] 
    private List<Image> _timeImage;
//    [SerializeField,Header("タイマーのバックグラウンド")] GameObject _timeBG;
    private Text timeText;
    private GameController gc;
    private bool isRun;
    private int _viewTime;

	// Use this for initialization
	void Start () 
	{
        _viewTime = Mathf.FloorToInt(time);
        foreach(var i in _timeImage)
            i.gameObject.SetActive(false);
//        _timeBG.SetActive(false);
        gc = GetComponent<GameController>();
        isRun = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (gc.isRun)
        {
            if(!isRun)
            {
//                _timeBG.SetActive(true);
                foreach(var i in _timeImage)
                    i.gameObject.SetActive(true);
                isRun = true;
            }
            if (!gc.gameOver) // ゲームオーバーしてなかったら
            {
                if (_viewTime <= 0) // タイムアップしたらGameControllerのフラグをtrueに
                {
                    _viewTime = 0;
                    gc.gameOver = true;
                }
                else time -= Time.deltaTime;
                _viewTime = Mathf.FloorToInt(time);
                ScoreManager.Instance.TimeUpdate(_timeImage, _viewTime);
            }
        }
	}

	public float TimeCount {
		get { return this.time; }
	}
    public int ViewTime {
        get { return this._viewTime; }
    }
}
