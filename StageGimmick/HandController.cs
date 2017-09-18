using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {
	private GameController gc;
    private List<GraveHand> graveHands = new List<GraveHand>();
    private int handMax;
    private int handCount;
    private float timeCount;
    [SerializeField,Header("手の復活までの時間"), Tooltip("改変可")] float rebornTime;
    [SerializeField,Header("おててたち"), Tooltip("改変禁止")] GameObject[] hands;
	// Use this for initialization
	void Start () {
		gc = GetComponent<GameController>();
		HandInit();
		handMax = handCount = hands.Length;
		timeCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (!gc.gameOver && gc.isRun)
        {
            if (handCount <= 3)
                timeCount += Time.deltaTime;
            if (handCount == 0 || timeCount >= rebornTime)
                RespawnHand();
        }
	}

	void HandInit()
	{
		foreach(var h in hands)
		{
			float x = (float)Random.Range(-13, 14);
			float z = (float)Random.Range(-13, 14);
			h.transform.position = new Vector3(x, h.transform.position.y, z);
			graveHands.Add(h.GetComponent<GraveHand>());
		}
	}

	void RespawnHand()
	{
		ObjectRespawn.Create(hands, Configs.Instance.width, Configs.Instance.height, true);
		timeCount = 0;
		handCount = handMax;
	}

	public int HandCount {
		set { handCount = value; }
		get { return handCount; }
	}
}
