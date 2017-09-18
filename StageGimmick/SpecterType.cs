using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class SpecterType : MonoBehaviour {
	// 0が常駐さん、1が成仏する人、2がジャックポット
	private int _specterNum;
    private float _moveX;
    private float _moveZ;
	private float _lifeTime;
    private Vector3 _newPos;
	private PlayerController _pc;
    private GameController _gc;
	private SpecterController _sc;
	private float _deathTime;
	private float _time;
	private int _moveState;

	public int SpecterNum {
		set { _specterNum = value; }
		get { return _specterNum; }
	}

	public float MoveX {
		set { _moveX = value; }
		get { return _moveX; }
	}

	public float MoveZ {
		set { _moveZ = value; }
		get { return _moveZ; }
	}

	public float LifeTime {
		set { _lifeTime = value; }
		get { return _lifeTime; }
	}
	// Use this for initialization
	void Start () {
		_gc = GameObject.Find("GameController").GetComponent<GameController>();
		_sc = GameObject.Find("GameController").GetComponent<SpecterController>();
		_time = 0;
		_deathTime = 0;
		_moveState = Random.Range(0, 2);
	}
	
	// Update is called once per frame
	void Update () {
		if(_gc.isRun && !_gc.gameOver)
		{
			Move();
			if(_deathTime >= _lifeTime)
				gameObject.SetActive(false);
		}
	}

	void Move()
	{
		if (_time >= 2.9f)
		{
			_time = 0;
			_moveState++;
			_moveState %= 2;
			return;
		}
		switch (_moveState)
		{
			case 0:
				GetDirection(_moveX, _moveState);
				_newPos = new Vector3(transform.localPosition.x + _moveX, transform.localPosition.y, transform.localPosition.z);
				transform.localPosition = _newPos;
				break;
			case 1:
				GetDirection(_moveZ, _moveState);
				_newPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + _moveZ);
				transform.localPosition = _newPos;
				break;
			default:
				break;
		}
		_time += Time.deltaTime;
		if(_specterNum > 0)
			_deathTime += Time.deltaTime;
	}

	void GetDirection(float move, int state)
    {
        if(state == 0)
        {
            if(move > 0)
                transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            else if(move < 0)
                transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
        }
        else if(state == 1)
        {
            if(move > 0)
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            else if(move < 0)
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }

	void PointSteal()
	{
		int rn = Random.Range(1, 5) * 5;
		if(_pc.BattlePoint < rn)
			rn = _pc.BattlePoint;
		_pc.BattlePoint -= rn;
		_pc.PointUpdate();
		_sc.jackpotPoint += rn;
	}

	void JackPot()
	{
		Debug.Log(_sc.jackpotPoint);
		_pc.BattlePoint += _sc.jackpotPoint;
		_pc.PointUpdate();
		_sc.jackpotPoint = 0;
	}

	void OnTriggerEnter(Collider collider){
		if (collider.gameObject.tag == "Player") 
		{
			_pc = collider.gameObject.GetComponent<PlayerController>();
			if(_specterNum < 2)
				PointSteal();
			else
				JackPot();
			if(_specterNum > 0)
				this.gameObject.SetActive(false);
		}
		else
		{
			_moveX *= -1;
			_moveZ *= -1;
		}
	}
}
