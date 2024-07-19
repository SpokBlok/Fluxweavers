using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //AudioSource
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    //AudioClip
    public AudioClip bgMusic;
    public AudioClip fluxSelect;
    public AudioClip fluxDrop;
    // Citrine voice lines
    public AudioClip citrineBasicAttack;
    public AudioClip citrineSkill;
    public AudioClip citrineUltActivation;
    public AudioClip citrineUltCast;
    public AudioClip citrineHurt;
    public AudioClip citrineDeath;

    // Dedra voice lines
    public AudioClip dedraBasicAttack;
    public AudioClip dedraEnhancedBasicAttack;
    public AudioClip dedraSkill;
    public AudioClip dedraUltActivation;
    public AudioClip dedraUltCast;
    public AudioClip dedraHurt;
    public AudioClip dedraDeath;

    // Maiko voice lines
    public AudioClip maikoBasicAttack;
    public AudioClip maikoSkill;
    public AudioClip maikoUltActivation;
    public AudioClip maikoUltCast;
    public AudioClip maikoHurt;
    public AudioClip maikoDeath;

    public AudioClip buttonClickSound;

    private void Start()
    {
        musicSource.clip = bgMusic;
        musicSource.Play();
        musicSource.loop = true;
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

}
