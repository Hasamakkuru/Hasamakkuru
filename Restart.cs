using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Player;

public class Restart : MonoBehaviour {
    private GameController _gc;
    private EnemyController _ec;
    void Start () {
        _gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if(Configs.Instance.stageNum == 0)
            _ec = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyController>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if(Configs.Instance.stageNum == 0)
        {
            if(collision.gameObject.tag == "Block")
            {
                collision.gameObject.SetActive(false);
                _gc.ObjectCount--;
            }
            else if(collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.SetActive(false);
                _ec.EnemyCount--;
            }
            else if(collision.gameObject.tag == "Player")
                FallDownPenalty(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Block" || collision.gameObject.tag == "Player")
            FallDownPenalty(collision.gameObject);
    }
    void OnTriggerEnter(Collider other)
	{
        if(Configs.Instance.stageNum == 0)
        {
            if(other.gameObject.tag == "Block")
            {
                Debug.Log(other.gameObject.name);
                other.gameObject.SetActive(false);
                _gc.ObjectCount--;
            }
            else if(other.gameObject.tag == "Enemy")
            {
                other.gameObject.SetActive(false);
                _ec.EnemyCount--;
            }
            else if(other.gameObject.tag == "Player")
                FallDownPenalty(other.gameObject);
        }
		else if(other.gameObject.tag == "Block" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player")
			FallDownPenalty(other.gameObject);
	}

    void FallDownPenalty(GameObject obj)
    {
        float x = (float)Random.Range(-14, 15);
        float z = (float)Random.Range(-12, 15);
        obj.transform.position = new Vector3(x, 5, z);
        if(obj.GetComponent<PlayerController>() != null)
        {
            obj.GetComponent<PlayerController>().animator.SetBool("Hold", false);
            obj.GetComponent<PlayerController>().animator.SetBool("Fell", false);
            obj.AddComponent<NomalizeState>();
            ExecuteEvents.Execute<EventController>(
                target: obj,
                eventData: null,
                functor: (attack, eventData) => attack.DeathPenalty()
            );
        }
    }
}
