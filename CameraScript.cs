using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public GameObject character; //get reference of player 
    public float followDelay = 1f;
    public Vector3 movement;
    bool movecam = true;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        //camera follows the player
        movement = new Vector3(Mathf.Clamp(character.transform.position.x, -10, 10), transform.position.y, transform.position.z);
        transform.position = movement;
	}
}
