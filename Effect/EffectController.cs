using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;

[System.Serializable]
public class BuffIcon {
	[SerializeField]
	private List<Image> _buffIcons = new List<Image>();
	public List<Image> buffIcons {
		get { return _buffIcons; }
	}
	[SerializeField]
	private List<Image> _deBuffIcons = new List<Image>();
	public List<Image> deBuffIcons {
		get { return _deBuffIcons; }
	}
}

public class EffectController : SingletonMono<EffectController>
{
	[System.Serializable]
	public class EffectList
	{
		public List<GameObject> _dustEffect = new List<GameObject> ();
		public List<GameObject> _playerStan = new List<GameObject> ();
		// _playerbuff[0] = RaisePower, _playerbuff[1] = Accelerationダヨ
		public List<GameObject> _playerBuff = new List<GameObject> ();
		// _playerDebuff[0] = DownPower, _playerDebuff[1] = Decelerationダヨ
		public List<GameObject> _playerDebuff = new List<GameObject> ();
		public List<GameObject> _playerSpawn = new List<GameObject> ();
	}

	[SerializeField]
	public List<EffectList> _effectList = new List<EffectList> ();

	private List<BuffIcon> _buffIcon = new List<BuffIcon>();
	public List<BuffIcon> buffIcon {
		get { return _buffIcon; }
		set { _buffIcon = value; }
	}

	//	enum PlayerState {
	//		normal = 0,
	//		buff,
	//		debuff,
	//	};
	//	PlayerState playerState = 0;

	// Use this for initialization
	void Awake ()
	{
		if (Instance != this) {
			Destroy (this);
			return;
		}
		DontDestroyOnLoad (this.gameObject);
	}

	public void EffectGenerate (int playerNum, int effectKind, Vector3 genePos, int stateKind = 0, PlayerState playerState = PlayerState.normal)
	{
		playerNum -= 1;
		switch (effectKind) {
		case 1:
			_effectList [playerNum]._dustEffect [0].gameObject.SetActive (true);
			_effectList [playerNum]._dustEffect [0].gameObject.transform.position = genePos;
			break;
		case 2:
			_effectList [playerNum]._playerStan [0].gameObject.SetActive (true);
			_effectList [playerNum]._playerStan [0].gameObject.transform.position = genePos;
			break;
		case 3:
			var _buffModule = _effectList [playerNum]._playerBuff [stateKind].GetComponent<ParticleSystem> ().main;
			if (playerState == PlayerState.debuff) {
				_buffModule.loop = false;
				_effectList [playerNum]._playerBuff [stateKind].gameObject.SetActive (true);
				_effectList [playerNum]._playerBuff [stateKind].gameObject.transform.position = genePos;
				_buffIcon[playerNum].buffIcons[stateKind].gameObject.SetActive(true);
			} 
			else {
				_buffModule.loop = true;
				_effectList [playerNum]._playerBuff [stateKind].gameObject.SetActive (true);
				_effectList [playerNum]._playerBuff [stateKind].gameObject.transform.position = genePos;
				_buffIcon[playerNum].buffIcons[stateKind].gameObject.SetActive(true);
			}
                
                //_effectList[playerNum]._playerBuff[stateKind].gameObject.SetActive(true);
                //_effectList[playerNum]._playerBuff[stateKind].gameObject.transform.position = genePos;
			break;
		case 4:
			var _debuffModule = _effectList [playerNum]._playerDebuff [stateKind].GetComponent<ParticleSystem> ().main;
			if (playerState == PlayerState.buff) {
				_debuffModule.loop = false;
				_effectList [playerNum]._playerDebuff [stateKind].gameObject.SetActive (true);
				_effectList [playerNum]._playerDebuff [stateKind].gameObject.transform.position = genePos;
				_buffIcon[playerNum].deBuffIcons[stateKind].gameObject.SetActive(true);
			} 
			else {
				_debuffModule.loop = true;
				_effectList [playerNum]._playerDebuff [stateKind].gameObject.SetActive (true);
				_effectList [playerNum]._playerDebuff [stateKind].gameObject.transform.position = genePos;
				_buffIcon[playerNum].deBuffIcons[stateKind].gameObject.SetActive(true);
			}

                //_effectList[playerNum]._playerDebuff[stateKind].gameObject.SetActive(true);
                //_effectList[playerNum]._playerDebuff[stateKind].gameObject.transform.position = genePos;
			break;
		case 5:
			_effectList [playerNum]._playerSpawn [stateKind].gameObject.SetActive (true);
			_effectList [playerNum]._playerSpawn [stateKind].gameObject.transform.position = genePos;
			break;
		default:
			break;
		}
	}

	public void FXClear ()
	{
		for (int i = 0; i < _effectList.Count; i++) {
			_effectList [i]._playerStan.Clear ();
			_effectList [i]._playerBuff.Clear ();
			_effectList [i]._playerDebuff.Clear ();
			_buffIcon[i].buffIcons.Clear();
			_buffIcon[i].deBuffIcons.Clear();
		}
	}

	public void FXExit (int playerNum, int effectKind)
	{
		playerNum--;
		switch (effectKind) {
		case 1:
			_effectList [playerNum]._dustEffect [0].gameObject.SetActive (false);
			break;
		case 2:
			_effectList [playerNum]._playerStan [0].gameObject.SetActive (false);
			break;
		case 5:
			_effectList [playerNum]._playerSpawn [0].gameObject.SetActive (false);
			break;
		default:
			Debug.Log ("は？そんなエフェクトないけど？");
			break;
		}
	}
}
