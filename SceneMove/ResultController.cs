using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResultController : MonoBehaviour {
    [SerializeField] private List<GameObject> _players = new List<GameObject>();
    [SerializeField] private List<Sprite> _winner = new List<Sprite>();
    [SerializeField] private Image _winImage;
    [SerializeField] private GameObject _winBack;
    [SerializeField] private GameObject[] _buttons;
    [SerializeField] private AudioMixerGroup _mixer;
    private int _winNum;
    private int _winChar;
    private string _winName;
    private BestScore _bs;
    private int _winScore;
    private int _dailyScore;
    private int _bestScore;
    void Start () {
        _winNum = PlayerPrefs.GetInt("winNum",5) - 1;
        _winName = PlayerPrefs.GetString("winName", "Draw");
		_winChar = PlayerPrefs.GetInt(_winName, 6);
        _winScore = PlayerPrefs.GetInt("score", 0);
        _bs = Load(BestScore.FilePath);
        _dailyScore = _bs.deily;
        _bestScore = _bs.bestScore;
        StartCoroutine(ResultView());
    }

    BestScore Load(string path) {
        if(!File.Exists(path)) {
            Debug.Log("kitenai");
            return new BestScore();
        }
        using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
            using(StreamReader sr = new StreamReader(fs)) {
                BestScore b = JsonUtility.FromJson<BestScore>(sr.ReadToEnd());
                if(b == null) return new BestScore();
                return b;
            }
        }
    }

    void Save() {
        _dailyScore = _winScore > _dailyScore ? _winScore : _dailyScore;
        _bestScore = _dailyScore > _bestScore ? _dailyScore : _bestScore;
        _bs.deily = _dailyScore;
        _bs.bestScore = _bestScore;
        using(FileStream fs = new FileStream(BestScore.FilePath, FileMode.Create, FileAccess.Write)) {
            using(StreamWriter sw = new StreamWriter(fs)) {
                sw.WriteLine(JsonUtility.ToJson(_bs));
            }
        }
    }
	
	IEnumerator ResultView(){
        yield return new WaitForSecondsRealtime(1f);
        if(_winChar != 6) {
            _players[_winChar].SetActive(true);
            SeManager.Instance.audioSource.outputAudioMixerGroup = _mixer;
            SeManager.Instance.Fanfare();
        }
        _winImage.sprite = _winner[_winNum];
        _winBack.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        foreach(var button in _buttons)
            button.SetActive(true);
        _buttons[1].GetComponent<Button>().Select();
        yield break;
    }

	public void BackTItle()
	{
        Save();
		SceneChange.ChangeScene("Title");
	}

	public void BackSelect()
	{
        Save();
		SceneChange.ChangeScene("Select");
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
[System.Serializable]
public class BestScore {
    private static string filePath = Application.dataPath + "/bestScore.json";
    public static string FilePath {
        get { return filePath; }
    }

    public int deily;
    public int bestScore;
}
