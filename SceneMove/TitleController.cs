using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class TitleController : MonoBehaviour {
    private static string SELECT = "Select";
    private static string PLAYERSELECT = "PlayerSelect";
    [SerializeField] bool selectCharcter;
    [SerializeField] List<GameObject> _nowLoading = new List<GameObject>();
    [SerializeField] GameObject _titleBG;
    [SerializeField] GameObject _selectNumber;
    [SerializeField] Selectable _firstSelect;
    [SerializeField] AudioClip ac;
    [SerializeField] float _flashing;
    [SerializeField] AudioMixerGroup _BGMMixer;
    [SerializeField] AudioMixerGroup _defaultSE;
    [SerializeField] AudioMixerGroup _titleCall;
    [SerializeField] EventSystem _eventSystem;
    private Button twoPersons;
    private ColorBlock _block;
    private Color _newColor;
    // Use this for initialization
    void Start () {
        AudioManager.Instance.BgmStop();
        _eventSystem.enabled = false;
        AudioManager.Instance.audioSource.outputAudioMixerGroup = _BGMMixer;
        //ConfigManager.Instance.charaSelect = selectCharcter;
        PlayerPrefs.DeleteKey("winner");
        PlayerPrefs.DeleteKey("stage");
        Configs.Instance.charaSelect = selectCharcter;
        Configs.ConfigReset();
	}

    void Update()
    {
        if(_selectNumber.activeSelf && Input.GetButtonDown("Cancel"))
        {
            _selectNumber.SetActive(false);
            _firstSelect.Select();
        }
    }

    void OnApplicationQuit()
	{
		Configs.AppQuit();
	}

    public void StartGame()
    {
        SeManager.Instance.PushedButton();
        _selectNumber.SetActive(true);
        twoPersons = GameObject.Find("TwoPersons").GetComponent<Button>();
        twoPersons.Select();
    }

    public void PlayerNumbers(int n)
    {
        SeManager.Instance.PushedButton();
        Configs.Instance.playerNums = n;
        var rNum = Random.Range(0, _nowLoading.Count);
        _nowLoading[rNum].SetActive(true);
        if (selectCharcter)
            StartCoroutine(SceneChange.LoadScene(PLAYERSELECT));
        else
            StartCoroutine(SceneChange.LoadScene(SELECT));
    }

    public void PushPrease()
    {
        _block = _firstSelect.colors;
        _newColor = _block.highlightedColor;
        _newColor.a -= _flashing;
        _block.highlightedColor = _newColor;
        _firstSelect.colors = _block;
        if (_newColor.a <= 0 || _newColor.a >= 1)
            _flashing *= -1;
    }

    public　IEnumerator BGMStart()
    {
        _titleBG.SetActive(true);
        SeManager.Instance.audioSource.outputAudioMixerGroup = _titleCall;
        yield return StartCoroutine(SeManager.Instance.TitleCall());
        _firstSelect.gameObject.SetActive(true);
        AudioManager.Instance.BgmStart(ac);
        SeManager.Instance.audioSource.outputAudioMixerGroup = _defaultSE;
        _eventSystem.enabled = true;
        _firstSelect.Select();
        yield break;
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
