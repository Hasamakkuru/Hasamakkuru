using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
using XInputDotNetPure;
#endif
// プレイヤーコントローラーが肥大してきたので分割
// こちらは入力関係

namespace Player {
    public partial class PlayerController : MonoBehaviour
{
    private string horizontal;
    private string vertical;
    private string fire;
    public void GamePadInit()
    {
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
        pIndex = (PlayerIndex)playerNum - 1;
        gState = GamePad.GetState(pIndex);
#endif
        GetPadName();
        playerName = "Player0" + playerNum.ToString();
        horizontal = "GamePad0" + playerNum.ToString() + "_X";
        vertical = "GamePad0" + playerNum.ToString() + "_Y";
        fire = "GamePad0" + playerNum.ToString() + "_Fire";
        safetyLock = true;
    }

    void PlayerInput()
    {
        if (stan && notMove)
            LeverGacha(Horizontal, Vertical);
        else if (!stan)
        {
            if (Fire && !recovery)
            {
                if (isCatch)
                    StartCoroutine(Throw());
                else if (catchObject != null)
                    StartCoroutine(Catch());
            }
            else
                transform.position = Move(Horizontal, Vertical);
        }
    }

    void GetDirection(float moveX, float moveZ)
    {
        float absX = Mathf.Abs(moveX);
        float absZ = Mathf.Abs(moveZ);
        if (isConfusion)
        {
            if (absX > absZ)
            {
                if (moveX < -0.01f)
                    transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                else if (moveX > 0.01f)
                    transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
            }
            else
            {
                if (moveZ < -0.01f)
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                else if (moveZ > 0.01f)
                    transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
        }
        else
        {
            if (absX > absZ)
            {
                if (moveX < -0.01f)
                    transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                else if (moveX > 0.01f)
                    transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }
            else
            {
                if (moveZ < -0.01f)
                    transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                else if (moveZ > 0.01f)
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
        }
    }

    Vector3 Move(float moveX, float moveZ)
    {
        GetDirection(moveX, moveZ);
        Vector3 newPos = transform.position;
        float absX = Mathf.Abs(moveX);
        float absZ = Mathf.Abs(moveZ);
        // 両方Ver.
        if (!stan)
        {
            if(transform.eulerAngles.x >= 80 || transform.eulerAngles.x <= -80)
                newPos.y = 2.5f;
            if (moveX < 0.1f && moveX > -0.1f)
                moveX = 0;
            if (moveZ < 0.1f && moveZ > -0.1f)
                moveZ = 0;
            if (absX < absZ)
                moveX = 0;
            else
                moveZ = 0;
            float _XorZ = absX > absZ ? absX : absZ;
            _animator.SetFloat("Speed", _XorZ);
            if (isConfusion)
                newPos = new Vector3(newPos.x + (-moveX * accel), newPos.y, newPos.z + (-moveZ * accel));
            else
                newPos = new Vector3(newPos.x + (moveX * accel), newPos.y, newPos.z + (moveZ * accel));
        }
        return newPos;
    }

    void ThrowByType()
    {
        catchObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        _animator.SetBool("Throw", false);
        _animator.SetFloat("CatchDelay", 0.2f);
        if (catchObject.GetComponent<PlayerController>() != null)
        {
            otherPlayer = catchObject.GetComponent<PlayerController>();
            otherPlayer.Stan = true;
            otherPlayer.IsGround = false;
            _animator.SetBool("CatchPlayer", true);
        }
        else if (catchObject.GetComponent<ObjectController>() != null)
        {
            oc = catchObject.GetComponent<ObjectController>();
            oc.player = this.gameObject;
            oc.EmissionDisable();
            _animator.SetBool("Catch", true);
        }
        else if (catchObject.GetComponent<EnemyType>() != null)
        {
            catchObject.GetComponent<EnemyType>().thrown = true;
            _animator.SetBool("Catch", true);
        }
        rb = catchObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void LeverGacha(float moveX, float moveZ)
    {
        if (pLostX >= moveX && mLostX <= moveX)
            return;
        else if (pLostZ >= moveZ && mLostZ <= moveX)
            return;
        float absX = Mathf.Abs(moveX);
        float absZ = Mathf.Abs(moveZ);
        if (absX > 0.5f || absZ > 0.5f)
            leverCount++;
        mLostX = moveX - 0.3f;
        pLostX = moveX + 0.3f;
        mLostZ = moveZ - 0.3f;
        pLostZ = moveZ + 0.3f;
    }

    void GetPadName()
    {
        if (Input.GetJoystickNames().Length >= playerNum)
            padName = Input.GetJoystickNames()[playerNum - 1];
        else
            padName = "";
    }

    IEnumerator Catch()
    {
        if(runCoroutine == true)
            yield break;
        runCoroutine = true;
        ThrowByType();
        catchObject.transform.parent = powerHand.transform;
        catchObject.transform.position = powerHand.transform.position;
        throwObject = catchObject;
        yield return new WaitForSecondsRealtime(1);
        SeManager.Instance.PlayerSe(playerNum, 1);
        _animator.SetFloat("CatchDelay", 1f);
        if (oc != null)
        {
            int r = Random.Range(1, 11);
            if (r <= 3 && gc != null)
                gc.CreateItem(throwObject.transform.position);
        }
        else if (otherPlayer != null) {
            otherPlayer.OtherPlayerReset();
            otherPlayer.animator.SetBool("Hold", true);
        }
        isCatch = true;
        yield return new WaitForSecondsRealtime(1f);
        runCoroutine = false;
        yield break;
    }

    IEnumerator Throw()
    {
        Debug.Log(safetyLock);
        if(runCoroutine == true || safetyLock == false)
            yield break;
        runCoroutine = true;
        yield return new WaitForSecondsRealtime(0.5f);
        if (throwObject == null)
        {
            isCatch = false;
            runCoroutine = false;
            yield break;
        }
        if (throwObject.GetComponent<ObjectController>() != null)
            throwObject.GetComponent<ObjectController>().thrown = true;
        DustEffectkari dustEffect = throwObject.GetComponent<DustEffectkari>();
        dustEffect.IsDust = true;
        dustEffect.playerNum = playerNum;
        rb.isKinematic = false;
        _animator.SetBool("Throw", true);
        Vector3 throwSpeed = (transform.TransformDirection(Vector3.forward) * throwPower + Vector3.up * (throwPower / 1.2f));
        rb.velocity = throwSpeed;
        SeManager.Instance.PlayerSe(playerNum, 2);
        throwObject.transform.parent = null;
        throwObject = null;
        if (otherPlayer != null)
        {
            otherPlayer.throwHasPlayer = this.gameObject;
            _plusPoint += 50;
            PointUpdate();
            otherPlayer = null;
        }
        else if (oc != null)
            oc = null;
        isCatch = false;
        _animator.SetBool("Catch", false);
        _animator.SetBool("CatchPlayer", false);
        _animator.SetFloat("CatchDelay", 0);
        yield return new WaitForSecondsRealtime(1f);
        runCoroutine = false;
        yield break;
    }

    float Horizontal
    {
        get
        {
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
            if (padName == "")
                return Input.GetAxis(horizontal);
            else
            {
                gState = GamePad.GetState(pIndex);
                return gState.ThumbSticks.Left.X;
            }
#else
            return Input.GetAxis(horizontal);
#endif
        }
    }
    float Vertical
    {
        get
        {
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
            if (padName == "")
                return Input.GetAxis(vertical);
            else
            {
                gState = GamePad.GetState(pIndex);
                return gState.ThumbSticks.Left.Y;
            }
#else
            return Input.GetAxis(vertical);
#endif
        }
    }
    public bool Fire
    {
        get
        {
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
            if (padName == "")
                return Input.GetButtonDown(fire);
            else
            {
                gState = GamePad.GetState(pIndex);
                return (gState.Buttons.B == ButtonState.Pressed) ? true : false;
            }
#else
            return Input.GetButtonDown(fire);
#endif
        }
    }
}
}