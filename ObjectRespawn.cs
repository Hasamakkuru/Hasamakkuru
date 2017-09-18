using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ObjectRespawn {
	public static void Create(GameObject[] gameObj, int width, int height, bool reset)
	{
		foreach(GameObject obj in gameObj)
		{
			if(reset == true || obj.activeSelf == false)
			{
				float x = (float)Random.Range(-width, width + 1);
                float z = (float)Random.Range(-height, height + 1);
				if(Configs.Instance.stageNum != 3)
					obj.transform.position = new Vector3(x, 10, z);
				else
					obj.transform.position = new Vector3(x, obj.transform.position.y, z);
				obj.transform.rotation = Quaternion.Euler(Vector3.zero);
				obj.SetActive(true);
				if(reset == false)
				{
					obj.GetComponent<Rigidbody>().isKinematic = false;
					obj.GetComponent<Rigidbody>().useGravity = true;
				}
			}
		}
	}

	public static void Create(List<GameObject> gameObj, int width, int height, bool reset)
	{
		foreach(GameObject obj in gameObj)
		{
			if(reset == true || obj.activeSelf == false)
			{
				float x = (float)Random.Range(-width, width + 1);
                float z = (float)Random.Range(-height, height + 1);
				if(Configs.Instance.stageNum == 1)
					obj.transform.position = new Vector3(x, 5, z);
				else
					obj.transform.position = new Vector3(x, obj.transform.position.y, z);
				obj.transform.rotation = Quaternion.Euler(Vector3.zero);
				obj.SetActive(true);
				if(reset == false)
				{
					obj.GetComponent<Rigidbody>().isKinematic = false;
					obj.GetComponent<Rigidbody>().useGravity = true;
				}
			}
		}
	}
}
