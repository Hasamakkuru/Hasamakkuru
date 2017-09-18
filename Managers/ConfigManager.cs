using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigManager : SingletonMono<ConfigManager> {
    [System.NonSerialized] public int playerNums = 4;
    [System.NonSerialized] public int stageNum = 0;
    [System.NonSerialized]
    public string[] PLAYERID =
    {
        "Player01",
        "Player02",
        "Player03",
        "Player04"
    };
    [System.NonSerialized] public bool charaSelect = false;
    [System.NonSerialized] public int width;
    [System.NonSerialized] public int height;
	// Use this for initialization
	void Awake()
    {
        if (Instance != this)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
