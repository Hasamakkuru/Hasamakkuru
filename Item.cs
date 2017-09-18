using UnityEngine;
using Player;

public class Item : MonoBehaviour {
	private int itemNum;
    private PlayerController player;
    private GameController gc;
    private Transform par;
	// Use this for initialization
	void Start () {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject.GetComponent<PlayerController>();
            ItemUse(itemNum);
            gc.ItemCount -= 1;
            par = gameObject.transform.parent;
            par.gameObject.SetActive(false);
        }
    }

    void ItemUse(int iNum)
    {
        switch(iNum)
        {
            case 0:
                Debug.Log("加速");
                player.gameObject.AddComponent<Acceleration>();
                break;
            case 1:
                Debug.Log("腕力増加");
                player.gameObject.AddComponent<RaisePower>();
                break;
            case 2:
                Debug.Log("弱体解除");
                player.gameObject.AddComponent<NomalizeState>();
                break;
            default:
                break;
        }
    }

    public int ItemNum { 
		get { return this.itemNum; }
		set { this.itemNum = value; }
	}
}
