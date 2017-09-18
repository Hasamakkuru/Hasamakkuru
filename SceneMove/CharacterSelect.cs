using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
using XInputDotNetPure;
#endif

public class CharacterSelect : MonoBehaviour {
	private int[] selecter;
    private string[] _playerid;
	private int padNum;
	private string _cancel;
	private StandaloneInputModule inputModule;
	private Button firstSelect;
	private Button _backSelect;
	#if(UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
	private PlayerIndex pIndex;
	#endif
	[SerializeField] List<GameObject> tutorials = new List<GameObject>();
	[SerializeField] Image _playerName;
	[SerializeField] GameObject _goBack;
	[SerializeField] AudioClip ac;
	[SerializeField] Sprite[] _nowSelect;
	// Use this for initialization
	void Start () {
		_cancel = "Cancel";
        _playerid = Configs.Instance.PLAYERID;
		foreach(var p in _playerid)
			PlayerPrefs.DeleteKey(p);
		inputModule = GameObject.Find("EventSystem").GetComponent<StandaloneInputModule>();
		firstSelect = GameObject.Find("Character01").GetComponent<Button>();
		padNum =　0;
        selecter = Enumerable.Range(0, Configs.Instance.playerNums).OrderBy(i => System.Guid.NewGuid()).ToArray();
		ChangeUser();
        AudioManager.Instance.BgmStart(ac);
	}

	void Update()
	{
		if(Input.GetButtonDown(_cancel))
		{
			if(_goBack.activeSelf)
				BackCancel();
			else if(padNum == 0)
			{
				padNum--;
				ChangeUser();
				_goBack.SetActive(true);
				_backSelect = GameObject.Find("BackCancel").GetComponent<Button>();
				_backSelect.Select();
			}
			else
			{
				padNum--;
				ChangeUser();
			}
		}
	}

	void OnApplicationQuit()
	{
		Configs.AppQuit();
	}

	void ChangeUser()
	{
		if(padNum < 0)
		{
			_cancel = "Cancel";
			inputModule.horizontalAxis = "Horizontal";
			inputModule.submitButton = "Submit";
			inputModule.cancelButton = _cancel;
		}
		else
		{
			#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
			pIndex = (PlayerIndex)selecter[padNum];
			StartCoroutine(SelecterVib());
			#else
			_cancel = "GamePad0" + (selecter[padNum] + 1).ToString() + "_Cancel";
			inputModule.horizontalAxis = "GamePad0" + (selecter[padNum] + 1).ToString() + "_X";
			inputModule.submitButton = "GamePad0" + (selecter[padNum] + 1).ToString() + "_Fire";
			inputModule.cancelButton = _cancel;
			#endif
			firstSelect.Select();
			_playerName.sprite = _nowSelect[selecter[padNum]];
		}
	}

	IEnumerator SelecterVib(){
		#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
		GamePad.SetVibration(pIndex, 0.5f, 0.5f);
		yield return new WaitForSecondsRealtime(1f);
		GamePad.SetVibration(pIndex, 0, 0);
		#endif
		yield break;
	}

	public void UserSelected(int charNum)
	{
		SeManager.Instance.PushedButton();
		PlayerPrefs.SetInt(_playerid[selecter[padNum]], charNum);
		if(padNum == Configs.Instance.playerNums - 1)
		{
			int r = Random.Range(0, tutorials.Count);
			tutorials[r].SetActive(true);
			StartCoroutine(SceneChange.LoadScene("Select"));
		}
		else
		{
			padNum++;
			ChangeUser();
		}
	}

	public void BackCancel()
	{
		_goBack.SetActive(false);
		firstSelect.Select();
		padNum++;
		ChangeUser();
	}

	public void BackTitle()
	{
		SeManager.Instance.PushedButton();
		SceneChange.ChangeScene("Title");
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
