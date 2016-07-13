using UnityEngine;
using System.Collections;

public class Foods : MonoBehaviour
{

    public float timeFade = 1f;
    public float fadespeed = 1f;
    public float fade;
    public float speed = 5f;
    
    //cam sizes
    Camera cam;
    float camWidth;
    float camHeight;

    SpriteRenderer srFood;

    bool move = true;
    bool fadeout = false;
    bool lostFood = false;
    float startTime;
    float duration = 5f;

    void Start()
    {
        //cam sizes
        cam = Camera.main;
        camHeight =  cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        //get render component to change alpha
        srFood = GetComponent<SpriteRenderer>();      
        startTime = Time.time;
    }
    void Update () {

        //move down
        if (move)
        {
            transform.position -= new Vector3(0f, 1f * speed* Time.deltaTime,0f);
            transform.Rotate(Vector3.back * 50 * Time.deltaTime);

        }
        
        // in the bottom the food stop and fade out
        if(transform.position.y < -camHeight){
            move = false; 
            fadeout = true;
            lostFood = true;  
        }

        //fade out and destroy
        if (fadeout) {
            float t = (Time.time - startTime) / duration;
            srFood.material.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1f, 0.0f, t));
            Destroy(gameObject, 5f);
        }
    }

   

}