using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Player;

[RequireComponent(typeof(DustEffectkari))]
[RequireComponent(typeof(Rigidbody))]
public class ObjectController : MonoBehaviour {

    //private DustEffect dust;
    private Transform _parent;
    private DustEffectkari kariDust;
    private GameController gc;
    private bool _thrown;
    private bool _falling;
    private GameObject _player;
    [SerializeField, Header("光る色"), Tooltip("いじってよし")]
    private Color _flashColor;
    [SerializeField, Header("マテリアル"), Tooltip("改変禁止")]
    private List<GameObject> gMats = new List<GameObject>();
    private List<Material> materials = new List<Material>();
	// Use this for initialization
    void Start() {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        _parent = GameObject.Find("ThrowObjects").transform;
        //dust = GetComponent<DustEffect>();
        kariDust = GetComponent<DustEffectkari>();
        GetMaterial();
    }

    void GetMaterial()
    {
        foreach(var gMat in gMats)
        {
            var m = Instantiate(gMat.GetComponent<Renderer>().material);
            gMat.GetComponent<Renderer>().material = m;
            materials.Add(m);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        //TagがBlockかつ投げたブロックなら処理
        // だいぶ変更しました
        if(thrown)
        {
            if (col.gameObject.tag == "Ground")
            {
                thrown = false;
                player = null;
                falling = false;
                transform.parent = _parent;
            }
            else if (col.gameObject.tag == "Block")
            {
                transform.parent = _parent;
                if (Configs.Instance.stageNum == 1 || Configs.Instance.stageNum == 3)
                {
                    gc.ObjectCount -= 1;
                    //dust.IsDust = false;
                    kariDust.IsDust = false;
                    gameObject.SetActive(false);
                }
            }
            else if (col.gameObject.tag == "Player")
            {
                if (col.gameObject == player || col.gameObject.GetComponent<PlayerController>().Recovery)
                    return;
                ExecuteEvents.Execute<EventController>(
                    target: player,
                    eventData: null,
                    functor: (attack, eventData) => attack.KilledPoint()
                );
                //dust.IsDust = false;
                kariDust.IsDust = false;
                col.gameObject.GetComponent<PlayerController>().Resusitation();
                transform.parent = _parent;
                if (Configs.Instance.stageNum == 1 || Configs.Instance.stageNum == 3)
                {
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.GetComponent<Rigidbody>().useGravity = false;
                    gc.ObjectCount--;
                    gameObject.SetActive(false);
                }
            }
        }
        else if(falling)
        {
            if (col.gameObject.tag == "Ground")
                falling = false;
            if(col.gameObject.tag == "Block")
            {
                gc.ObjectCount -= 2;
                col.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }

            else if(col.gameObject.tag == "Player")
            {
                col.gameObject.GetComponent<PlayerController>().Stan = true;
                gc.ObjectCount--;
                gameObject.SetActive(false);
            }
        }
    }
    
    public void EmissionEnable()
    {
        if(Configs.Instance.stageNum == 0)
        {
            foreach(var m in materials)
                MaterialUtils.Emission(m, false);
        }
        else
        {
            foreach(var m in materials)
            {
                MaterialUtils.Emission(m, true);
                m.SetColor("_EmissionColor", _flashColor);
            }
        }
    }

    public void EmissionDisable()
    {
        if(Configs.Instance.stageNum == 0)
        {
            foreach(var m in materials)
                MaterialUtils.Emission(m, true);
        }
        else
        {
            foreach(var m in materials)
                MaterialUtils.Emission(m, false);
        }
    }

    public bool thrown {
        set { _thrown = value; }
        get { return _thrown; }
    }
    
    public bool falling {
        set { _falling = value; }
        get { return _falling; }
    }

    public GameObject player {
        set { _player = value; }
        get { return _player; }
    }
}
