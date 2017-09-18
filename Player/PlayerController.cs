using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Player
{
    [RequireComponent(typeof(Attack))]
    [RequireComponent(typeof(DustEffectkari))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public partial class PlayerController : MonoBehaviour {
	// Use this for initialization
	void Start () {
		stan = false;
		isCatch = false;
		recovery = false;
        isGround = true;
        isConfusion = false;
        runCoroutine = false;
        notMove = false;
		battlePoint = 0;
        _plusPoint = 0;
        stanRelease = 0;
        leverCount = 0;
        mLostX = 0;
        mLostZ = 0;
        pLostX = 0;
        pLostZ = 0;
        accelLower = 0.06f;
        throwLower = 3.5f;
        _alpha = 0.075f;
        _invincible = 0;
		_state = PlayerState.normal;
        _animator = GetComponent<Animator>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        PlayerInit();
        PointUpdate();
	}
	
	// Update is called once per frame
	void Update () {
        if (gc != null && !gc.gameOver && gc.isRun)
        {
            if(!runCoroutine)
                PlayerInput();
            if (stan)
            {
                if(isGround)
                    stanRelease += Time.deltaTime;
                if(stanRelease >= 10 || leverCount == leverMax)
                {
                    stan = false;
                    notMove = false;
                    stanRelease = 0;
                    leverCount = 0;
                    transform.parent = null;
                    gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    _animator.SetBool("Fell", false);
                    _animator.SetBool("Hold", false);
                    _animator.SetBool("GraveCatched", false);
                }
            }
            if(otherPlayer != null || throwObject != null)
                ThrowFailed();
            if(recovery)
            {
                _invincible += Time.deltaTime;
                Color face = _faceMaterial.color;
                Color body = _bodyMaterial.color;
                if(face.a >= 1 || face.a <= 0)
                    _alpha *= -1;
                face.a += _alpha;
                body.a += _alpha;
                _faceMaterial.color = face;
                _bodyMaterial.color = body;
                if(_invincible >= invisible)
                {
                    recovery = false;
                    _invincible = 0;
                    MaterialUtils.SetMode(_faceMaterial, MaterialUtils.Mode.Opaque);
                    MaterialUtils.SetMode(_bodyMaterial, MaterialUtils.Mode.Opaque);
                    _faceMaterial.color = new Color(_faceMaterial.color.r, _faceMaterial.color.g, _faceMaterial.color.b, 1);
                    _bodyMaterial.color = new Color(_bodyMaterial.color.r, _bodyMaterial.color.g, _bodyMaterial.color.b, 1);
                }
            }
        }
	}

	void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ground" || col.gameObject.tag == "Block")
        {
            if (stan && !isGround)
            {
                _animator.SetBool("Fell", true);
                throwHasPlayer = null;
                isGround = true;
                _plusPoint -= 20;
                PointUpdate();
            }
        }
        else if (col.gameObject.tag == "Player")
        {
            PlayerController pc = col.gameObject.GetComponent<PlayerController>();
            if (pc.Stan && !pc.IsGround && recovery)
            {
                if (pc.throwHasPlayer == null) return;
                if (pc.throwHasPlayer == this.gameObject)
                    return;
                Resusitation();
            }
            else if (stan)
            {
                if (throwHasPlayer == null) return;
                else if (throwHasPlayer == col.gameObject || isGround)
                {
                    throwHasPlayer = null;
                    return;
                }
                ExecuteEvents.Execute<EventController>(
                    target: throwHasPlayer,
                    eventData: null,
                    functor: (attack, eventData) => attack.BonusPoint()
                );
                throwHasPlayer = null;
                Resusitation();
            }
        }
	}

    void PlayerInit()
    {
        status = _statusAsset.GetParams();
        accel = status.accel;
        acceleration = status.acceleration;
        throwPower = status.throwPower;
        raisePower = status.raisePower;
        releaseTime = status.releaseTime;
        leverMax = status.leverMax;
        invisible = status.invisible;
        _faceMaterial.color = new Color(_faceMaterial.color.r, _faceMaterial.color.g, _faceMaterial.color.b, 1);
        _bodyMaterial.color = new Color(_bodyMaterial.color.r, _bodyMaterial.color.g, _bodyMaterial.color.b, 1);
    }

    void ThrowFailed()
    {
        if(throwObject == null)
            return;
        Transform other = throwObject.transform;
        if(other.parent == null)
        {
            isCatch = false;
            throwObject = null;
            otherPlayer = null;
            catchObject = null;
            _animator.SetBool("Throw", false);
            _animator.SetBool("Catch", false);
            _animator.SetBool("CatchPlayer", false);
            _animator.SetFloat("CatchDelay", 0);
        }
    }
    
    public void PointUpdate()
    {
        ScoreManager.Instance.ScoreUpdate(battlePoint, _plusPoint, playerNum);
        battlePoint += _plusPoint;
        if(battlePoint >= 9999)
            battlePoint = 9999;
        else if(battlePoint <= 0)
            battlePoint = 0;
        _plusPoint = 0;
    }

    public void EffectExit()
    {
        if(accel > status.accel)
            accel -= acceleration;
        if(accel < status.accel)
            accel += acceleration;
        if(throwPower > status.throwPower)
            throwPower -= raisePower;
        if(throwPower < status.throwPower)
            throwPower += raisePower;
        if(accel <= status.accel + 0.01f && accel >= status.accel - 0.01f)
            accel = status.accel;
        if(throwPower <= status.throwPower + 0.01f && throwPower >= status.throwPower - 0.01f)
            throwPower = status.throwPower;
        if(throwPower == status.throwPower)
        {
            _buff[0].SetActive(false);
            _debuff[0].SetActive(false);
            _buffIcon.buffIcons[0].gameObject.SetActive(false);
            _buffIcon.deBuffIcons[0].gameObject.SetActive(false);
			_state = PlayerState.normal;
        }
        if(accel == status.accel)
        {
            _buff[1].SetActive(false);
            _debuff[1].SetActive(false);
            _buffIcon.buffIcons[1].gameObject.SetActive(false);
            _buffIcon.deBuffIcons[1].gameObject.SetActive(false);
			_state = PlayerState.normal;
        }
        isConfusion = false;
        _confusion.SetActive(false);
    }

    public void OtherPlayerReset()
    {
        if(!notMove)
            notMove = true;
        if (throwObject != null)
        {
            throwObject.GetComponent<Rigidbody>().isKinematic = false;
            throwObject.transform.parent = null;
            throwObject = null;
            isCatch = false;
            _animator.SetBool("Throw", false);
            _animator.SetBool("Catch", false);
            _animator.SetBool("CatchPlayer", false);
            _animator.SetFloat("CatchDelay", 0);
        }
    }

    public void GraveCatched()
    {
        stan = true;
        _animator.SetBool("GraveCatched", true);
    }

    public void Resusitation()
    {
        if(throwObject != null)
        {
            throwObject.GetComponent<Rigidbody>().isKinematic = false;
            throwObject.transform.parent = null;
            throwObject = null;
            _animator.SetBool("Throw", false);
            _animator.SetBool("Catch", false);
            _animator.SetBool("CatchPlayer", false);
            _animator.SetFloat("CatchDelay", 0);
            ThrowFailed();
        }
        gameObject.AddComponent<NomalizeState>();
        _animator.SetBool("Fell", false);
        _animator.SetBool("Hold", false);
		stan = false;
        MaterialUtils.SetMode(_faceMaterial, MaterialUtils.Mode.Fade);
        MaterialUtils.SetMode(_bodyMaterial, MaterialUtils.Mode.Fade);
        Debug.Log(_faceMaterial.GetFloat("_Mode"));
        recovery = true;
        _plusPoint -= 50;
        PointUpdate();
    }
}
}
