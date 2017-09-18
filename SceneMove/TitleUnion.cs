using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUnion : MonoBehaviour
{
    [SerializeField, Header("画像の動く速度"), Tooltip("改変可")]
    float _moveSpeed = 10; //画像が動く速度
    [SerializeField, Header("ロゴ"), Tooltip("改変禁止")]
    GameObject _logo; // 画像の設定
    private Vector2 _pos; // スライドの関数
    private bool _actionEnd = false; // 処理変更用
    private bool _switching = false; // 順番切り替え用
    private bool _once = true; // ズーム判定
    private bool _run = false;

    // 配列宣言
    [SerializeField]
    private List<GameObject> _side = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _vertical = new List<GameObject>();
    private TitleController _tc; // TitleControllerを代入する関数
    // Use this for initialization
    void Start()
    {
        _tc = GetComponent<TitleController>(); // _tcに<TitleController>を代入
        _logo.transform.localScale = Vector3.zero; // 初期のロゴの大きさを0.1に設定
    }

    // Update is called once per frame
    void Update()
    {
        // 縦横のスライドの処理変更
        if (!_switching)
            ImageMoveToSide();
        else if(!_actionEnd)
            ImageMoveToVertical();
        
        else if(!_run)
            TitleLogoDoon();
        
    }

    // 横のスライド処理
    void ImageMoveToSide()
    {
        if (_actionEnd) return;
        for (int i = 0; i < _side.Count; i++) // 配列_sideの長さだけ回す
        {
            _pos = _side[i].GetComponent<RectTransform>().localPosition; // _pos.xに_side[i]を入れる
            if (i % 2 == 0 && _pos.x <= 0) // iを2で割った余りが0の時かつ_pos.xが0より小さいとき
            {
                _pos.x += _moveSpeed;
            }
            else if (_pos.x >= 0)
            {
                _pos.x -= _moveSpeed;
            }
            else
            {
                _pos.x = 0;
                _switching = true; // trueの時処理を止める
            }
            _side[i].GetComponent<RectTransform>().localPosition = _pos; // _posを_sideに代入する
        }
    }

    // 縦のスライド処理
    void ImageMoveToVertical()
    {
        if (_actionEnd)
            return;
        for (int i = 0; i < _vertical.Count; i++) // 配列_verticalの長さだけ回す
        {
            _pos = _vertical[i].GetComponent<RectTransform>().localPosition; // _pos.yに_vertical[i]を入れる
            if (i % 2 == 1 && _pos.y <= 0) // iを2で割った余りが1の時かつ_pos.yが0より小さいとき
            {
                _pos.y += _moveSpeed;
            }
            else if (_pos.y >= 0)
            {
                _pos.y -= _moveSpeed;
            }
            else
            {
                _pos.y = 0;
                _actionEnd = true; // trueの時処理を止める
                //StartCoroutine(TitleEzakiDoon());
            }
            _vertical[i].GetComponent<RectTransform>().localPosition = _pos; // _posを_verticalに代入する
        }
    }

    void TitleLogoDoon()
    {
        if(_run)
            return;
        Vector3 nowScale = _logo.GetComponent<RectTransform>().localScale;
        if(_once)
        {
            _once = false;
            StartCoroutine(_tc.BGMStart()); // BGMスタート
        }
        if(nowScale.x >= 1.5f)
            _run = true;
        _logo.GetComponent<RectTransform>().localScale = new Vector3(nowScale.x + 0.1f, nowScale.y + 0.1f, nowScale.z + 0.1f);
    }
}