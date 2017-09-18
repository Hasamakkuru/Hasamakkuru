using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour {
    private List<GameObject> players;
	private Vector3 nowPos;
	private Vector3 offset;
	private Vector3 newPos;
	private int count;
	private GameController gc;
    private List<Transform> playerPos = new List<Transform>();
    [SerializeField, Header("最終的なカメラの位置"), Tooltip("改変可")] Vector3 lastPos;
    [SerializeField, Header("1PからのZ位置の差。正の数で指定してください。"), Tooltip("改変可")] float distance = 4;
    [SerializeField, Header("Ready？表示するやつ"), Tooltip("改変禁止")] Text ready;
	// Use this for initialization
	void Start () {
        players = new List<GameObject>();
		count = 0;
		gc = GameObject.Find("GameController").GetComponent<GameController>();
		GetPlayers();
        nowPos = new Vector3(playerPos[0].position.x, transform.position.y, playerPos[0].position.z - distance);
        transform.position = nowPos;
        offset = nowPos - playerPos[0].position;
		newPos = Vector3.zero;
		StartCoroutine(SwitchView());
	}
	
	void GetPlayers()
	{
		GameObject player01 = GameObject.Find("Player01");
        GameObject player02 = GameObject.Find("Player02");
        GameObject player03;
        GameObject player04;
        players.Add(player01);
        players.Add(player02);
        if(Configs.Instance.playerNums >= 3)
        {
            player03 = GameObject.Find("Player03");
            players.Add(player03);
        }
		if (Configs.Instance.playerNums == 4)
		{
			player04 = GameObject.Find("Player04");
			players.Add(player04);
		}
		for(int i = 0; i < players.Count; i++)
			playerPos.Add(players[i].transform);
	}
	
	IEnumerator SwitchView()
	{
		while(true)
		{
			if(count >= players.Count)
				break;
			SeManager.Instance.StartBeep();
			newPos = playerPos[count].position + offset;
			transform.position = newPos;
            gc.GamepadVibration(count, 0.25f);
            yield return new WaitForSecondsRealtime(1);
            gc.GamepadVibration(count, 0);
			count++;
		}
		yield return StartCoroutine(PlayeGame());
		ready.gameObject.SetActive(false);
		Destroy(this);
	}

	IEnumerator PlayeGame()
	{
		transform.position = lastPos;
		ready.text = "GO!";
		SeManager.Instance.StartBuzzer();
		yield return new WaitForSecondsRealtime(0.5f);
		gc.isRun = true;
		gc.StartBgm();
		yield break;
	}
}
