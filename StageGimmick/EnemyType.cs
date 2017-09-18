using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

[RequireComponent (typeof(DustEffectkari))]
public class EnemyType : MonoBehaviour
{
	private int enemyNum;
	private int moveState;
	private float moveX;
	private float moveZ;
	private float moveTime;
	private bool running;
	private bool isTouch;
	private Transform parent;
	private Vector3 newPos;
	private PlayerController pc;
	private GameController gc;
	private EnemyController ec;
	private Animator anim;
	[System.NonSerializedAttribute] public bool thrown;
	// Use this for initialization
	void Start ()
	{
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		ec = GameObject.Find ("GameController").GetComponent<EnemyController> ();
		anim = GetComponent<Animator> ();
		parent = transform.parent;
		moveTime = 0;
		moveState = 0;
		running = false;
		isTouch = false;
		newPos = Vector3.zero;
	}

	void Update ()
	{
		if (gc.isRun && !gc.gameOver) {
			if (!thrown && !GetComponent<Rigidbody> ().isKinematic)
				Move ();
			else {
				newPos = transform.localPosition;
				transform.localPosition = newPos;
			}
			if (transform.position.y < -50) {
				ec.EnemyCount--;
				gameObject.SetActive (false);
			}
		}
	}

	void OnCollisionEnter (Collision col)
	{
		if (!isTouch) {
			if (thrown) {
				if (col.gameObject.tag == "Ground") {
					GetComponent<Rigidbody> ().isKinematic = false;
					transform.parent = parent;
					thrown = false;
				}
			} else if (col.gameObject.tag == "Player") {
				pc = col.gameObject.GetComponent<PlayerController> ();
//				transform.parent = col.gameObject.transform;
				Weakening (enemyNum, col.transform);
//				gameObject.SetActive (false);
			} else {
				moveX *= -1;
				moveZ *= -1;
			}
		}
	}

	void Move ()
	{
		switch (enemyNum) {
		case 0:
			GetDirection (moveX, enemyNum);
			newPos = new Vector3 (transform.localPosition.x + moveX, transform.localPosition.y, transform.localPosition.z);
			transform.localPosition = newPos;
			break;
		case 1:
			GetDirection (moveZ, enemyNum);
			newPos = new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + moveZ);
			transform.localPosition = newPos;
			break;
		case 2:
			if (running == false)
				running = true;
			break;
		}
		if (running) {
			if (moveTime >= 4.9f) {
				moveTime = 0;
				running = false;
				moveState++;
				moveState %= 2;

				return;
			}
			switch (moveState) {
			case 0:
				GetDirection (moveX, moveState);
				newPos = new Vector3 (transform.localPosition.x + moveX, transform.localPosition.y, transform.localPosition.z);
				transform.localPosition = newPos;
				break;
			case 1:
				GetDirection (moveZ, moveState);
				newPos = new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + moveZ);
				transform.localPosition = newPos;
				break;
			default:
				break;
			}
			moveTime += Time.deltaTime;
		}
	}

	void GetDirection (float move, int state)
	{
		if (state == 0) {
			if (move > 0)
				transform.rotation = Quaternion.Euler (new Vector3 (0, 90, 0));
			else if (move < 0)
				transform.rotation = Quaternion.Euler (new Vector3 (0, 270, 0));
		} else if (state == 1) {
			if (move > 0)
				transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
			else if (move < 0)
				transform.rotation = Quaternion.Euler (new Vector3 (0, 180, 0));
		}
	}

	void Weakening (int eNum, Transform playerTransform)
	{
		switch (eNum) {
		case 0:
			Debug.Log ("減速");
			pc.gameObject.AddComponent<Deceleration> ();
			StartCoroutine (EnemyAttack (playerTransform));
			break;
		case 1:
			Debug.Log ("腕力低下");
			pc.gameObject.AddComponent<DownPower> ();
			StartCoroutine (EnemyAttack (playerTransform));
			break;
		case 2:
			Debug.Log ("混乱");
			pc.gameObject.AddComponent<Confusion> ();
			StartCoroutine (EnemyAttack (playerTransform));
			break;
		default:
			break;
		}
	}

	IEnumerator EnemyAttack(Transform playerTransform) {
		isTouch = true;
		this.GetComponent<CapsuleCollider> ().enabled = false;
		anim.SetBool ("isHit", true);
		yield return new WaitWhile (() => {
			var animatorState = anim.GetCurrentAnimatorStateInfo (0);
			this.transform.position = playerTransform.position;
			Debug.Log("TIme:" + string.Format("{0:F1}", animatorState.normalizedTime));
			return animatorState.normalizedTime >= 1;
		});
		ec.EnemyCount -= 1;
		anim.SetBool ("isHit", false);
		this.gameObject.SetActive (false);
    }

	public void InitEnemyState(){
//		this.gameObject.transform.SetParent (parent);
		isTouch = false;
		this.GetComponent<CapsuleCollider> ().enabled = true;
	}

	public int EnemyNum {
		set { enemyNum = value; }
		get { return enemyNum; }
	}

	public float MoveX {
		set { moveX = value; }
		get { return moveX; }
	}

	public float MoveZ {
		set { moveZ = value; }
		get { return moveZ; }
	}
}
