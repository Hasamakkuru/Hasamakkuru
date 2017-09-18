using UnityEngine;
using System.Linq;
using Player;

public class Safety : MonoBehaviour {
	[SerializeField] PlayerController pc;
	private PlayerController _otherPc;
	private ObjectController _oc;
	// Use this for initialization
	void Start () {
		_otherPc = null;
		_oc = null;
	}
		
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Block" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player")
		{
			if(pc.catchObject != null)
				return;
			if(other.gameObject.GetComponent<PlayerController>() != null)
			{
				_otherPc = other.gameObject.GetComponent<PlayerController>();
				if(_otherPc.Stan || _otherPc.Recovery)
				{
					_otherPc = null;
					return;
				}
			}
			if(other.gameObject.GetComponent<ObjectController>() != null)
			{
				_oc = other.gameObject.GetComponent<ObjectController>();
				_oc.EmissionEnable();
			}
			pc.catchObject = other.gameObject;
		}
		else if(other.gameObject.tag == "Wall" && pc.throwObject != null) {
			pc.SafetyLock = false;
			Debug.Log("wallに触れた");
		}
	}

    void OnTriggerExit(Collider other)
    {
		if(other.gameObject.tag == "Wall" && pc.throwObject != null)
			pc.SafetyLock = true;
		else
		{
			if(_oc != null)
				_oc.EmissionDisable();
			_otherPc = null;
			_oc = null;
        	pc.catchObject = null;
		}
    }
}
