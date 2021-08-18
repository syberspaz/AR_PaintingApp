using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectManager : MonoBehaviour
{
    [SerializeField]
    AudioClip PaletteMenuClickSound;

    [SerializeField]
    AudioClip RadialMenuClickSound;

    [SerializeField]
    AudioClip AppIntroSound;

    [SerializeField]
    AudioClip LessonCompleteSound;

    [SerializeField]
    AudioClip LessonStartedSound;



    public void PlayToolPaletteSound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = PaletteMenuClickSound;
        audio.Play();
    }

    public void PlayRadialMenuClickSound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = RadialMenuClickSound;
        audio.Play();
    }

    public void PlayAppIntroSound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = AppIntroSound;
        audio.Play();
    }

    public void PlayLessonCompleteSound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = LessonCompleteSound;
        audio.Play();
    }

    public void PlayLessonStartedSound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = LessonStartedSound;
        audio.Play();
    }


}
