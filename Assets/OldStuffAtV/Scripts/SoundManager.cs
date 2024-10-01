using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{


    public static SoundManager sndMan;

    public AudioSource audioSrc;

    public AudioSource audioSrc2;

    private AudioClip[] dashSounds;

    private int randomDashSound;

    private AudioClip[] dashDashSounds;

    private int randomDashDashSound;

    private AudioClip[] shroomSounds;

    private int randomShroomSound;

    private AudioClip[] sprungSounds;

    private int randomSprungSound;

    private AudioClip[] deathSounds;

    private int randomDeathSound;

    private AudioClip[] respawnSounds;

    private int randomRespawnSound;

    private AudioClip[] collectedSounds;

    private int randomCollectedSound;

    private AudioClip[] colorSounds;

    private int randomColorSound;

    void Start()
    {
        sndMan = this;
        //audioSrc = GetComponent<AudioSource>();
        //audioSrc2 = GetComponent<AudioSource>();
        dashSounds = Resources.LoadAll<AudioClip>("DashSounds");
        dashDashSounds = Resources.LoadAll<AudioClip>("DashDashSounds");
        sprungSounds = Resources.LoadAll<AudioClip>("SprungSounds");
        deathSounds = Resources.LoadAll<AudioClip>("DeathSounds");
        respawnSounds = Resources.LoadAll<AudioClip>("SpawnSounds");
        collectedSounds = Resources.LoadAll<AudioClip>("CollectingSounds");
        shroomSounds = Resources.LoadAll<AudioClip>("ShroomSounds");
        colorSounds = Resources.LoadAll<AudioClip>("ColorSounds");
    }

    public void PlayDashSound()
    {
        randomDashSound = Random.Range(0, 8);
        audioSrc.PlayOneShot(dashSounds[randomDashSound]);
    }
    public void PlaySprungSound()
    {
        randomSprungSound = Random.Range(0, 11);
        audioSrc.PlayOneShot(sprungSounds[randomSprungSound]);
    }
    public void PlayDeathSound()
    {
        randomDeathSound = Random.Range(0, 7);
        audioSrc.PlayOneShot(deathSounds[randomDeathSound]);
    }
    public void PlayRespawnSound()
    {
        randomRespawnSound = Random.Range(0, 0);
        audioSrc.PlayOneShot(respawnSounds[randomRespawnSound]);
    }
    public void PlayCollectedSound()
    {
        randomCollectedSound = Random.Range(0, 7);
        audioSrc.PlayOneShot(collectedSounds[randomCollectedSound]);
    }
    public void PlayShroomSound()
    {
        randomShroomSound = Random.Range(0, 2);
        audioSrc2.PlayOneShot(shroomSounds[randomShroomSound]);
    }
    public void PlayDashDashSound()
    {
        randomDashDashSound = Random.Range(0, 2);
        audioSrc2.PlayOneShot(dashDashSounds[randomDashDashSound]);
    }

    public void PlayColorSound()
    {
        randomColorSound = Random.Range(0, 1);
        audioSrc2.PlayOneShot(colorSounds[randomColorSound]);
    }
}