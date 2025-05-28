using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneDoor : MonoBehaviour
{
    [SerializeField] private float initialFadeMusicDuration = 2f;
    [SerializeField] private AudioClip bossMusic;
    private AudioSource _source;
    private readonly int _closeHash = Animator.StringToHash("Close");

    public void Close()
    {
        GetComponent<Animator>().SetTrigger(_closeHash);
    }

    public void EnableGolem()
    {
        FindObjectOfType<Golem>().SetCutsceneHash();
    }
    
    public void FadeOutMusic()
    {
        _source = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        StartCoroutine(AudioFadeOut.FadeOut(_source, initialFadeMusicDuration));
    }

    public void PlayBossMusic()
    {
        _source.clip = bossMusic;
        _source.Play();
    }
}
