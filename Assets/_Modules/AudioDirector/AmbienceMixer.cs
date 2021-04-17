﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceMixer : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] ambienceClips;
    private bool keepFadingIn;
    private bool keepFadingOut;

    void Start()
    {
        source = this.GetComponent<AudioSource>();
        PlayClip(0);
        ChangeMusic(1,0.01f,0.5f);
    }

    public void FadeIn(int track, float speed, float maxVolume)
    {
        StartCoroutine(FadeInRoutine(track, speed, maxVolume));
    }

    public void FadeOut(float speed)
    {
        StartCoroutine(FadeOutRoutine(speed));
    }

    public void ChangeMusic(int newTrack, float speed, float maxVolume)
    {
        StartCoroutine(ChangeMusicRoutine(newTrack, speed, maxVolume));
    }

    public void PlayClip(int newTrack)
    {
        source.Stop();
        source.PlayOneShot(ambienceClips[newTrack]);
    }

    public void StopClip()
    {
        source.Stop();
    }

    private IEnumerator FadeInRoutine(int track, float speed, float maxVolume)
    {
        source.PlayOneShot(ambienceClips[track]);
        keepFadingIn = true;
        keepFadingOut = false;
        source.volume = 0;
        float audioVolume = source.volume;

        while (source.volume < maxVolume && keepFadingIn)
        {
            audioVolume += speed;
            source.volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }

        if(source.volume < speed && keepFadingOut)
        {
            source.Stop();
        }
    }
    
    private IEnumerator FadeOutRoutine(float speed)
    {
        keepFadingIn = false;
        keepFadingOut = true;
        float audioVolume = source.volume;

        while (source.volume >= speed && keepFadingOut)
        {
            audioVolume -= speed;
            source.volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator ChangeMusicRoutine(int newTrack, float speed, float maxVolume)
    {
        FadeOut(speed);

        while(source.volume >= speed)
        {
            yield return new WaitForSeconds(0.01f);
        }

        source.Stop();

        if (ambienceClips != null)
        {
            FadeIn(newTrack, speed, maxVolume);
        }
    }
}
