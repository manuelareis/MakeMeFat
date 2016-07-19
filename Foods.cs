using UnityEngine;
using System.Collections;

public class Foods : MonoBehaviour
{
    public enum Type { Food, BadThing, Power, Coin }
    public Type type;

    float timeFade = 1f;
    float fadespeed = 1f;
    float fade;
    float speed = 1f;

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
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        //get render component to change alpha
        srFood = GetComponent<SpriteRenderer>();
        startTime = Time.time;

        /*switch(type)
        {
            case Type.Food:
                speed += 0f;
                break;

            case Type.BadThing:
                speed += 1f;
                break;

            case Type.Power:
                speed += 2f;
                break;
        }      */
    }
    void Update()
    {
        if (move)
        {
            transform.position -= new Vector3(0f, speed * Time.deltaTime, 0f);
            transform.Rotate(Vector3.back * 50 * Time.deltaTime);
        }

        if (transform.position.y < -camHeight && lostFood == false)
        {
            move = false;
            fadeout = true;
            if (type == Type.Food)
                LosePoint();
        }

        //fade out and destroy
        if (fadeout)
        {
            float t = (Time.time - startTime) / duration;
            if (type == Type.Food)
                srFood.material.color = new Color(1f, 0f, 0f, Mathf.SmoothStep(1f, 0.0f, t));
            else
                srFood.material.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1f, 0.0f, t));

            Destroy(gameObject, 5f);
        }
    }

    void LosePoint()
    {
        GM.gmInstance.setScore(0, -5);
        lostFood = true;
    }

    public void setSpeed(float vel)
    {
        speed = vel;
    }

    public Type getType()
    {
        return type;
    }
}