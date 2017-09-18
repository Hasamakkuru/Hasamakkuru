using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {
	private static string[] STAGENAMES =
    {
        "CyberField",
        "TempleField",
        "FlatField",
        "CemeteryField"
    };
    private static string PLAYERSELECT = "PlayerSelect";
    private static string TITLE = "Title";
    [SerializeField] Button _firstSelect;
    [SerializeField] Text _stageDiscText;
    [SerializeField] List<GameObject> _loadingImage = new List<GameObject>();
    [SerializeField] GameObject _goBack;
    [SerializeField] AudioClip ac;
    private Button _backSelect;
    [System.NonSerialized] public Button beforeSelect;
    private int _beforeNum;
    private int _afterNum;
    private Vector3 _centerPos = new Vector3(0, 15, 0);
    private Vector3 _rightPos = new Vector3(275, 35, 0);
    private Vector3 _leftPos = new Vector3(-275, 35, 0);
    [SerializeField]
    List<Button> _buttonList = new List<Button>();
    [SerializeField] GameObject _goTutorial;
    private Button _noTutorial;
    private int stageNum;
    private bool _onece = true;
    private string[] _stageDisc;
    private char _delim = '/';
    // ごめんなさい、テスト用で作ります
    [SerializeField]
    List<GameObject> _testButtonList = new List<GameObject>();
    private string _stageText;
    private string path;
    void Start()
    {
        path = Application.dataPath + "/stageDisc.txt";
        _stageText = GetDescription();
        _stageDisc = _stageText.Split(_delim);
        _firstSelect.Select();
        AudioManager.Instance.BgmStart(ac);
        stageNum = 0;
        Configs.ConfigReset(false);
    }

    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if(_goBack.activeSelf)
                BackCancel();
            else if(_goTutorial.activeSelf == false)
            {
                _goBack.SetActive(true);
                _backSelect = GameObject.Find("BackCancel").GetComponent<Button>();
                _backSelect.Select();
            }
        }
    }

    void OnApplicationQuit()
	{
		Configs.AppQuit();
	}

    string GetDescription() {
        if(!File.Exists(path)) {
            Debug.Log("kitenai");
            return "";
        }
        using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
            using(StreamReader sr = new StreamReader(fs)) {
                string disc = sr.ReadToEnd();
                if(disc == null) return "";
                return disc;
            }
        }
    }

	public void StageSelect(int n)
    {
        SeManager.Instance.PushedButton();
        if(n < _buttonList.Count - 1)
            stageNum = n;
        else
            stageNum = UnityEngine.Random.Range(0, _buttonList.Count - 1);
        _goTutorial.SetActive(true);
        _noTutorial = GameObject.Find("TutoNo").GetComponent<Button>();
        _noTutorial.Select();
    }

    public void Return()
    {
        SeManager.Instance.PushedButton();
        if(Configs.Instance.charaSelect)
            SceneChange.ChangeScene(PLAYERSELECT);
        else
            SceneChange.ChangeScene(TITLE);
    }

    public void BackCancel()
    {
        _goBack.SetActive(false);
        beforeSelect.Select();
    }

    public void StageTextChange(int stageNum)
    {
        if (!_onece)
            SeManager.Instance.Scroll();
        for(int i = 0; i < _testButtonList.Count; i++)
        {
            if(i == stageNum)
            {
                _testButtonList[i].SetActive(true);
            }
            else
            {
                _testButtonList[i].SetActive(false);
            }
        }
        _stageDiscText.text = _stageDisc[stageNum];
        if (_onece)
            _onece = false;
    }

    public void SelectMove(int stageNum)
    {
        _beforeNum = stageNum - 1;
        if(_beforeNum < 0)
        {
            _beforeNum = _buttonList.Count - 1;
        }
        _afterNum = stageNum + 1;
        if(_afterNum > _buttonList.Count - 1)
        {
            _afterNum = 0;
        }

        for(int i = 0; i < _buttonList.Count; i++)
        {
            if(i == stageNum)
            {
                _buttonList[stageNum].transform.localPosition = _centerPos;
                _buttonList[stageNum].transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
            }
            if (i == _beforeNum)
            {
                _buttonList[_beforeNum].transform.localPosition = _leftPos;
                _buttonList[_beforeNum].transform.localScale = Vector3.one;
            }
            if (i == _afterNum)
            {
                _buttonList[_afterNum].transform.localPosition = _rightPos;
                _buttonList[_afterNum].transform.localScale = Vector3.one;
            }
            if(i != stageNum && i != _beforeNum && i != _afterNum)
            {
                _buttonList[i].transform.localPosition = new Vector3(1000, 1000, 0);
            }
        }
    }

    public void StageLoad()
    {
        SeManager.Instance.PushedButton();
        int r = UnityEngine.Random.Range(0, _loadingImage.Count);
        _loadingImage[r].SetActive(true);
        StartCoroutine(SceneChange.LoadScene(STAGENAMES[stageNum]));
    }

    public void SizeUP(GameObject button)
    {
        button.SizeUp();
    }

    public void SizeDown(GameObject button)
    {
        button.SizeDown();
    }
}
