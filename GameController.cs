/*     
アセットを使わせてもらったので表記

The MIT License

Copyright (c) 2009 Remi Gillig <remigillig@gmail.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Player;
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
using XInputDotNetPure;
#endif

public class GameController : MonoBehaviour {
	[SerializeField,Header("ステージの設定"), Tooltip("改変禁止")] 
	private StageSetting _ss;
	[SerializeField,Header("キャラクター"), Tooltip("改変禁止"), NamedArray(new string[]{"Player01","Player02","Player03","Player04"})] 
	private GameObject[] charcters;
	[SerializeField,Header("各プレイヤーのUIの親"), Tooltip("改変禁止"), NamedArray(new string[]{"Player01","Player02","Player03","Player04"})] 
	private List<GameObject> _pParent = new List<GameObject>();
	[SerializeField,Header("プレイヤーのアイコン"), Tooltip("改変禁止"), NamedArray(new string[]{"Player01","Player02","Player03","Player04"})] 
	private List<Image> _pObj = new List<Image>();
	[SerializeField,Header("変更後のアイコンの元"), Tooltip("改変禁止")] 
	private List<Sprite> _pIcon = new List<Sprite>();
	[SerializeField,Header("プレイヤーのスコア"), Tooltip("改変禁止")]
	private List<PlayerScore> _playerScore = new List<PlayerScore>();
	[SerializeField,Header("加減された値"), Tooltip("改変禁止")]
	private List<PlayerScore> _plusScore = new List<PlayerScore>();
	[SerializeField,Header("プラスマイナスの符号"), Tooltip("改変禁止")]
	private List<Image> _pSigns = new List<Image>();
	[SerializeField,Header("アニメさせる親"), Tooltip("改変禁止")]
	private List<GameObject> _pointParents = new List<GameObject>();
	[SerializeField,Header("バフのアイコン"), Tooltip("改変禁止")]
	private List<BuffIcon> _buffIcons = new List<BuffIcon>();
    [SerializeField,Header("アイテムの保存場所"), Tooltip("改変禁止")] 
	private GameObject items; // 出現するアイテムの親オブジェクト
	[SerializeField,Header("アイテムのマテリアル"), Tooltip("改変禁止")] 
	private List<Material> itemMaterial = new List<Material>();
	[SerializeField,Header("投げるオブジェクトの保存場所"), Tooltip("改変禁止")] 
	private GameObject throwObject;
	[SerializeField,Header("ゲーム終了時の背景"), Tooltip("改変禁止")] 
	private GameObject finishBg;
    [SerializeField, Header("閉じるあれ"), Tooltip("改変禁止")]
    private GameObject _shutter;
    [SerializeField,Header("BGMのミキサー"), Tooltip("改変禁止")] 
	private AudioMixerGroup _stageMixer;
	[SerializeField,Header("オブジェクトの配置位置"), Tooltip("改変禁止")] 
	private TextAsset _jsonAsset;
	private List<PlayerController> Players;
	private bool runCoroutine;
	private int stageNum;
	private int itemNum;
	private int itemCount;
	private int objectCount;
    private int maxCount;
	private GameObject[] itemBoxs;
	private GameObject[] throwObjects;
	private ObjectPos _opos;
	private EventSystem eventSystem;
    [System.NonSerialized] public bool gameOver = false; // いろんなところから変更、参照するからpublicで
    [System.NonSerialized] public bool isRun = false;
	// Use this for initialization
	void Start () {
		ObjPlacement();
		SelectedStage();
		GeneratePlayers();
		SetPlayer();
		itemCount = 0;
        maxCount = 5;
		runCoroutine = false;
        objectCount = throwObjects.Length;
		eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isRun)
        {
            if (gameOver && !runCoroutine) // ゲームオーバー処理
                StartCoroutine(GetWinnerName());
            if (Configs.Instance.stageNum != 2)
            {
                if (objectCount <= throwObjects.Length - 5)
                    CreateObjects();
            }
        }
	}

	void ObjPlacement()
	{
        // 投げるオブジェクトの数を動的に確保して登録
		_opos = _jsonAsset.GetObjPos();
		throwObjects = new GameObject[throwObject.transform.childCount];
		for(int i = 0; i < throwObjects.Length; i++) {
			throwObjects[i] = throwObject.transform.GetChild(i).gameObject;
			throwObjects[i].transform.localPosition = _opos.pos[i];
		}
        // 同じくアイテムでも同じことをする
		itemBoxs = new GameObject[items.transform.childCount];
		for(int i = 0; i < itemBoxs.Length; i++)
		{
			itemBoxs[i] = items.transform.GetChild(i).gameObject;
			itemBoxs[i].SetActive(false);
		}
		
	}

	void SelectedStage()
	{
        // 選ばれたステージを登録する
		Configs.Instance.stageNum = _ss.stageNum;
		AudioManager.Instance.BgmStop();
		Configs.Instance.width = _ss.width;
		Configs.Instance.height = _ss.height;
        //　勝者とステージ番号を初期化
		PlayerPrefs.DeleteKey("winner");
		PlayerPrefs.DeleteKey("score");
	}

	void GeneratePlayers()
	{
        // PlayerControllerのリストを作成し、プレイ人数を取得。その分だけキャラをアクティブにする
		Players = new List<PlayerController>();
        int pNum = Configs.Instance.playerNums;
		for(int i = 0; i < pNum; i++)
		{
            // キャラクター選択の時の数値を取得、追加
            // デフォルトの数字iを取得
            int charNum = PlayerPrefs.GetInt(Configs.Instance.PLAYERID[i], i); 
			Configs.Instance.pModels.Add(charNum);
            // 大本から選ばれたキャラをアクティブにし、Listに追加
            GameObject p = charcters[i].transform.GetChild(charNum).gameObject;
			PlayerController _pc = p.GetComponent<PlayerController>();
            p.SetActive(true);
            p.transform.position = _ss.pPos[i];
            p.name = Configs.Instance.PLAYERID[i];
			if(i < 2)
				p.transform.rotation = Quaternion.Euler(0, 180, 0);
			Players.Add(_pc);
            // エフェクトのマネージャーに自分のエフェクトを追加
			EffectController.Instance._effectList[i]._playerStan.Add(_pc.Confusion);
			EffectController.Instance._effectList[i]._playerBuff = new List<GameObject>(_pc.Buff);
			EffectController.Instance._effectList[i]._playerDebuff = new List<GameObject>(_pc.Debuff);
            // 選ばれたキャラにアイコンを合わせる
			_pObj[i].sprite = _pIcon[charNum];
		}
        // プレイヤーが4人以下だった場合アイコンを非表示にする
        if (pNum != 4)
        {
            for (int i = pNum; i < 4; i++)
            {
                _pParent[i].SetActive(false);
            }
        }
	}

	void SetPlayer() // 各プレイヤーに初期設定
	{
		ScoreManager.Instance.parents = new List<GameObject>(_pointParents);
		ScoreManager.Instance.playerScore = new List<PlayerScore>(_playerScore);
		ScoreManager.Instance.plusScore = new List<PlayerScore>(_plusScore);
		ScoreManager.Instance.pSigns = new List<Image>(_pSigns);
		EffectController.Instance.buffIcon = new List<BuffIcon>(_buffIcons);
        foreach(var pc in Players.Select((v, n) => new { v, n }))
        {
			pc.v.PlayerNum = pc.n + 1;
			pc.v.buffIcon = _buffIcons[pc.n];
			pc.v.GamePadInit();
        }
	}

	void CreateObjects()
	{
		ObjectRespawn.Create(throwObjects, Configs.Instance.width, Configs.Instance.height, false);
		objectCount = throwObjects.Length;
	}

    void LoadResult()
    {
        StartCoroutine(SceneChange.LoadScene("ResultScene"));
    }

	void OnApplicationQuit()
	{
		Configs.AppQuit();
	}

	IEnumerator GetWinnerName()
	{
        runCoroutine = true;
        finishBg.SetActive(true);
		AudioManager.Instance.BgmStop();
		eventSystem.enabled = false;
		yield return StartCoroutine(SeManager.Instance.EndWhisle());
        _shutter.SetActive(true);
        yield return new WaitForSecondsRealtime(_ss.delay01);
        int maxPoint = Players.Max(pc => pc.BattlePoint);
		var mvp = Players.Where(pc => pc.BattlePoint == maxPoint).Select(v => v.PlayerName).ToList();
		var n = Players.Where(pc => pc.BattlePoint == maxPoint).Select(v => v.PlayerNum).ToList();
        if (mvp.Count >= 2)
        {
            PlayerPrefs.SetString("winName", "Draw");
			PlayerPrefs.SetInt("winNum", 5);
        }
        else
        {
            PlayerPrefs.SetString("winName", mvp.First());
			PlayerPrefs.SetInt("winNum", n.First());
        }
        PlayerPrefs.SetInt("score", maxPoint);
		ScoreManager.Instance.Clear();
        EffectController.Instance.FXClear();
		LoadResult();
    }

    public void CreateItem(Vector3 objPos)
    {
        if (itemCount < maxCount)
        {
            Vector3 itemPos = new Vector3(objPos.x, itemBoxs[itemCount].transform.localPosition.y, objPos.z);
			if(Configs.Instance.stageNum == 0)
				itemNum = Random.Range(0, 3);
            else
				itemNum = Random.Range(0, 2);
            itemBoxs[itemCount].transform.position = itemPos;
            itemBoxs[itemCount].SetActive(true);
			GameObject itemM = itemBoxs[itemCount].transform.FindChild("立方体").gameObject;
			itemM.GetComponent<Renderer>().material = itemMaterial[itemNum];
			Item item = itemM.GetComponent<Item>();
            item.ItemNum = itemNum;
            itemCount++;
        }
    }
    public void GamepadVibration(int pNum, float vib)
    {
		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        GamePad.SetVibration((PlayerIndex)pNum, vib, vib);
		#endif
    }

	public void StartBgm()
	{
		AudioManager.Instance.audioSource.outputAudioMixerGroup = _stageMixer;
		AudioManager.Instance.BgmStart(_ss.bgm);
	}

	public void SizeUP(GameObject button)
    {
        button.SizeUp();
    }

    public void SizeDown(GameObject button)
    {
        button.SizeDown();
    }

	public int ObjectCount{
		set { objectCount = value; }
		get { return objectCount; }
	}

    public int ItemCount {
        set { itemCount = value; }
        get { return itemCount; }
    }
}
