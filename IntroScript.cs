using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class IntroScript : MonoBehaviour {

    // Use this for initialization
    IEnumerator Start () {
        AsyncOperation async = Application.LoadLevelAdditiveAsync("Menu");
        yield return async; 
    }
	
}
