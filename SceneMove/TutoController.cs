using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TutoController : MonoBehaviour {
	private SceneController _sc;
	private GameObject _tutoObj;
    [SerializeField] Image _tutorial;
    [SerializeField] List<Sprite> _tutoDisc = new List<Sprite>();
    [SerializeField] Button _nextButton;
	[SerializeField] GameObject _backButton;
	[SerializeField] Text _nextText;
    private int _tutoCount;
	// Use this for initialization
	void Start () {
		_sc = GetComponent<SceneController>();
		_tutoCount = 0;
	}
	
	// Update is called once per frame

	public void Tutorial()
    {
        _tutoObj = _tutorial.gameObject;
        _tutoObj.SetActive(true);
        _tutorial.sprite = _tutoDisc[_tutoCount];
		_nextButton.Select();
    }

	public void NextTuto()
	{
		if(_tutoCount == _tutoDisc.Count - 1)
			_sc.StageLoad();
		else
		{
			if(_tutoCount == 0)
				_backButton.SetActive(true);
			_tutoCount++;
			_tutorial.sprite = _tutoDisc[_tutoCount];
			if(_tutoCount == _tutoDisc.Count - 1)
				_nextText.text = "Start!";
		}
	}

	public void BackTuto()
	{
		_tutoCount--;
		if(_tutoCount <= 0)
		{
			_tutoCount = 0;
			_backButton.SetActive(false);
			_nextButton.Select();
		}
		_tutorial.sprite = _tutoDisc[_tutoCount];
		_nextText.text = "Next";
	}

	public void SizeUP(GameObject button)
    {
        button.SizeUp();
    }

    public void SizeDown(GameObject button)
    {
        button.SizeDown();
    }
}
