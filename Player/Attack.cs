using UnityEngine;
using Player;
public class Attack : MonoBehaviour,EventController {
	private PlayerController pc;
	// Use this for initialization
	void Start () {
		pc = GetComponent<PlayerController>();
	}
	
	public void KilledPoint()
	{
		pc.PlusPoint += 100;
        pc.PointUpdate();
	}

    public void BonusPoint()
    {
        pc.PlusPoint += 300;
        pc.PointUpdate();
    }

	public void DeathPenalty()
	{
		pc.PlusPoint -= 300;
		pc.PointUpdate();
	}
}
