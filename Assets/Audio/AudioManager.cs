using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource masterSource;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource effectSource;

    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip walk;
    public AudioClip dash;
    public AudioClip starCollect;
    public AudioClip death;
    public AudioClip levelWin;
    public AudioClip enterBossRoom;
    public AudioClip bossBackground;

    public void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

}
