using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteUpdate {
	static string _ui = "UI/";
	static Sprite n0 = Resources.Load<Sprite>(_ui + "0");
	static Sprite n1 = Resources.Load<Sprite>(_ui + "1");
	static Sprite n2 = Resources.Load<Sprite>(_ui + "2");
	static Sprite n3 = Resources.Load<Sprite>(_ui + "3");
	static Sprite n4 = Resources.Load<Sprite>(_ui + "4");
	static Sprite n5 = Resources.Load<Sprite>(_ui + "5");
	static Sprite n6 = Resources.Load<Sprite>(_ui + "6");
	static Sprite n7 = Resources.Load<Sprite>(_ui + "7");
	static Sprite n8 = Resources.Load<Sprite>(_ui + "8");
	static Sprite n9 = Resources.Load<Sprite>(_ui + "9");
	static Sprite[] views = { n0, n1, n2, n3, n4, n5, n6, n7, n8, n9 };
	static Color _upPoint = new Color(255, 75, 75, 255);
	static Color _downPoint = new Color(75, 75, 255, 255);
	public static void Update(List<Image> nummbers, int score, int plusPoint, Text plusView)
	{
		if(plusPoint == 0) return;
		plusView.gameObject.SetActive(true);
		if(plusPoint <= 0)
            plusView.color = _downPoint;
        else
            plusView.color = _upPoint;
		plusView.text += plusPoint.ToString("+#;-#;");
		var _save = score + plusPoint;
        if (_save <= 0)
            _save = 0;
        if (_save >= 9999)
            _save = 9999;
		plusView.StartCoroutine(ViewUpdate(plusView.gameObject, _save, nummbers));
	}

	public static IEnumerator ViewUpdate(GameObject view, int score, List<Image> nums)
	{
		yield return new WaitForSecondsRealtime(1f);
		view.SetActive(false);
		if (score <= 0)
			foreach(var num in nums)
            	num.sprite = views[0];
        else
        {
            foreach(var num in nums)
            {
                num.sprite = views[score % 10];
                score /= 10;
            }
        }
		yield break;
	}
}
