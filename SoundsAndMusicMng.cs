using UnityEngine;
using System.Collections;

[RequireComponent(typeof (AudioSource))]
public class SoundsAndMusicMng : MonoBehaviour {

    [SerializeField]  AudioSource musicIntro;
    [SerializeField]  AudioSource musicMenu;
    [SerializeField]  AudioSource musicLoop;

    void Start() { 
    
        StartCoroutine(PlayIntro()); 
    }
    public IEnumerator PlayIntro()
    {
        musicIntro.Play();
        yield return new WaitForSeconds(musicIntro.clip.length);
        musicLoop.Play();
        musicLoop.loop = true;

    }
}

