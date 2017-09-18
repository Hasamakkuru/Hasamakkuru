using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
class EnemyKind
{
    public List<GameObject> _enemyKind = new List<GameObject>();
}
public class EnemyController : MonoBehaviour {
    
    private GameController gc;
    private List<EnemyType> enemys = new List<EnemyType>();
    private int enemyMax;
    private int enemyCount;
    private float timeCount;
    [SerializeField,Header("生成までの時間"), Tooltip("改変可")] float maxTime;
    [SerializeField,Header("最大個数"), Tooltip("改変可")] int maxCount;
    [SerializeField,Header("横に動く量"), Tooltip("改変可")] float moveX;
    [SerializeField,Header("縦に動く量"), Tooltip("改変可")] float moveZ; 
    [SerializeField,Header("エネミーのリスト"), Tooltip("改変禁止")] List<EnemyKind> _enemyKindList = new List<EnemyKind>();

    // Use this for initialization
    void Start () {
        gc = GetComponent<GameController>();

        EnemyInit();
        enemyCount = enemyMax = maxCount;
        timeCount = 0;
	}

	// Update is called once per frame
	void Update () {
        if (!gc.gameOver && gc.isRun)
        {
            if (enemyCount <= 2)
                timeCount += Time.deltaTime;
            if (enemyCount == 0 || timeCount >= maxTime)
                RespawnEnemy();
        }
	}
    
    /*
    void EnemyInit()
    {
        enemyObjs = new GameObject[maxCount];
        enemys = new EnemyType[maxCount];
        for(var i = 0; i < maxCount; i++)
        {
            int rNum = Random.Range(0, enemyMaterials.Length);
            float posX = (float)Random.Range(-11, 12);
            float posZ = (float)Random.Range(-11, 12);
            GameObject enemyObj = Instantiate(enemyInstance, enemy.transform);
            enemyObjs[i] = enemyObj;
            enemyObj.transform.localPosition = new Vector3(posX, enemyObj.transform.position.y, posZ);
            enemys[i] = enemyObj.GetComponent<EnemyType>();
            enemys[i].EnemyNum = rNum;
            enemys[i].MoveX = moveX;
            enemys[i].MoveZ = moveZ;
            enemys[i].GetComponent<Renderer>().material = enemyMaterials[rNum];
        }
    }
    */
    
    // こちらが修正後です(動作確認まだです)
    void EnemyInit()
    {
        for (var i = 0; i < maxCount; i++)
        {
            int rNum = Random.Range(0, _enemyKindList.Count);
            float posX = (float)Random.Range(-10, 11);
            float posZ = (float)Random.Range(-10, 11);
            foreach(GameObject j in _enemyKindList[rNum]._enemyKind)
            {
                if(j.activeSelf == false)
                {
                    j.SetActive(true);
                    j.gameObject.transform.localPosition = new Vector3(posX, j.transform.position.y + 5, posZ);
                    enemys.Add(j.gameObject.GetComponent<EnemyType>());
                    enemys[i].EnemyNum = rNum;
                    enemys[i].MoveX = moveX;
                    enemys[i].MoveZ = moveZ;
                    break;
                }
            }
        }
    }

    /*
    void RespawnEnemy()
    {
        ObjectRespawn.Create(enemyObjs, false);
        foreach(var e in enemys)
        {
            int r = Random.Range(0, enemyMaterials.Length);
            e.EnemyNum = r;
            e.GetComponent<Renderer>().material = enemyMaterials[r];
        }
        enemyCount = enemyMax;
        timeCount = 0;
    }
    */

    // こっちが修正後デス(まだ未完成です)
    void RespawnEnemy()
    {
        foreach(var e in enemys)
        {
            e.gameObject.SetActive(false);
        }
        enemys.Clear();
        for(int e = 0; e < maxCount; e++)
        {
            int r = Random.Range(0, _enemyKindList.Count);
            foreach (GameObject i in _enemyKindList[r]._enemyKind)
            {
                if (i.activeSelf == false)
                {
                    float x = (float)Random.Range(-Configs.Instance.width, Configs.Instance.width + 1);
                    float z = (float)Random.Range(-Configs.Instance.height, Configs.Instance.height + 1);
                    i.SetActive(true);
                    enemys.Add(i.gameObject.GetComponent<EnemyType>());
                    enemys[e].EnemyNum = r;
                    enemys[e].gameObject.transform.localPosition = new Vector3(x, 5, z);
                    enemys[e].gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
                    enemys[e].GetComponent<Rigidbody>().isKinematic = false;
                    enemys[e].GetComponent<Rigidbody>().useGravity = true;
                    enemys[e].MoveX = moveX;
                    enemys[e].MoveZ = moveZ;
					enemys [e].InitEnemyState ();
                    break;
                }
            } 
        }
        enemyCount = enemyMax;
        timeCount = 0;
    }

    public int EnemyCount {
        set { enemyCount = value; }
        get { return enemyCount; }
    }
}
