using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundsAndMusicMng : MonoBehaviour
{

    [SerializeField]
    public static SoundsAndMusicMng soundsIntance;
    [SerializeField]
    AudioSource _audioSource;
    [SerializeField]
    AudioClip clipIntro;
    [SerializeField]
    AudioClip clipLoop;
    [SerializeField]
    AudioClip clipMenu;


    void Awake()
    {
        if (soundsIntance == null)
            soundsIntance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    public IEnumerator StartMusicIntro()
    {
        print("Music Intro");
        _audioSource.clip = clipIntro;
        _audioSource.loop = false;
        _audioSource.Play();
        yield return new WaitForSeconds(clipIntro.length);
        _audioSource.clip = clipLoop;
        _audioSource.loop = true;
        _audioSource.Play();
        print("Music Loop");
    }

    public void SetMusicMenu()
    {
        _audioSource.clip = clipMenu;
        _audioSource.loop = true;
        _audioSource.Play();
        print("Music Menu");
    }

}

