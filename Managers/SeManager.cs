using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SeManager : SingletonMono<SeManager> {
    private AudioSource _audioSouce;
    [SerializeField]
    List<AudioClip> _titleCall = new List<AudioClip>();
    [SerializeField]
    AudioClip _pushedButton;
    [SerializeField]
    AudioClip _scroll;
    [SerializeField]
    AudioClip _startBeep;
    [SerializeField]
    AudioClip _startBuzzer;
    [SerializeField]
    AudioClip _endWhisle;
    [SerializeField]
    AudioClip _resultBeep;
    [SerializeField]
    AudioClip _resultBuzzer;
    [SerializeField]
    AudioClip _lift;
    [SerializeField]
    AudioClip _throwSe;
    [SerializeField]
    AudioClip _fanfare;
    [SerializeField]
    List<AudioSource> _playerSource = new List<AudioSource>();
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
    void Start()
    {
        _audioSouce = GetComponent<AudioSource>();
    }

    public void PushedButton()
    {
        _audioSouce.clip = _pushedButton;
        _audioSouce.Play();
    }

    public void Scroll()
    {
        _audioSouce.clip = _scroll;
        _audioSouce.Play();
    }

    public void StartBeep()
    {
        _audioSouce.clip = _startBeep;
        _audioSouce.Play();
    }

    public void StartBuzzer()
    {
        _audioSouce.clip = _startBuzzer;
        _audioSouce.Play();
    }

    public void ResultBeep()
    {
        _audioSouce.clip = _resultBeep;
        _audioSouce.Play();
    }

    public void ResultBuzzer()
    {
        _audioSouce.clip = _resultBuzzer;
        _audioSouce.Play();
    }
    // seKind 1が持ち上げ、2が投げる
    public void PlayerSe(int playerNum, int seKind)
    {
        playerNum--;
        switch(seKind)
        {
            case 1:
                _playerSource[playerNum].clip = _lift;
                break;
            case 2:
                _playerSource[playerNum].clip = _throwSe;
                break;
            default:
                break;
        }
        _playerSource[playerNum].Play();
    }

    public void Fanfare() {
        _audioSouce.clip = _fanfare;
        _audioSouce.Play();
    }

    public IEnumerator TitleCall()
    {
        int r = Random.Range(0, _titleCall.Count);
        _audioSouce.clip = _titleCall[r];
        _audioSouce.Play();
        yield return new WaitWhile(() => _audioSouce.isPlaying);
    }

    public IEnumerator EndWhisle()
    {
        _audioSouce.clip = _endWhisle;
        _audioSouce.Play();
        yield return new WaitWhile(() => _audioSouce.isPlaying);
    }

    public AudioSource audioSource {
        get { return _audioSouce; }
    }
}
