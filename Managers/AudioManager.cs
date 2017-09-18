using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : SingletonMono<AudioManager> {
    private AudioSource _audioSouce;
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

    public void BgmStart(AudioClip ac)
    {
        if (_audioSouce != null)
        {
            _audioSouce.loop = false;
            _audioSouce.clip = null;
        }
        _audioSouce.clip = ac;
        _audioSouce.Play();
        _audioSouce.loop = true;
    }

    public void BgmStop()
    {
        _audioSouce.Stop();
        _audioSouce.clip = null;
    }

    public AudioSource audioSource {
        get { return _audioSouce; }
    }
}
