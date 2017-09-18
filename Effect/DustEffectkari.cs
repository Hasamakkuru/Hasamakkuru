using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustEffectkari : MonoBehaviour {

    private bool isDust;
    private int _playerNum;

    public bool IsDust
    {
        set { isDust = value; }
    }
    
    public int playerNum {
        set { _playerNum = value; }
        get { return _playerNum; }
    }

    //エフェクトの生成
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground"  && isDust)
        {
            //Instantiate(dust, new Vector3(x, y, z), Quaternion.Euler(-90,0,0));
            EffectController.Instance.EffectGenerate(_playerNum, 1, gameObject.transform.position);
            isDust = false;
        }
    }
}
