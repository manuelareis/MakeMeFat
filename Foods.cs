using UnityEngine;
using System.Collections;

public class Foods : MonoBehaviour
{
    [SerializeField]
    enum Type { Food, BadThing, Power, Coin }
    [SerializeField]
    Type type;

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
            switch (type)
            {
                case Type.Food:
                    GM.gmInstance.contFoodsLost += 1;
                    LosePoint();
                    break;

                case Type.BadThing:
                    GM.gmInstance.contBadthingsLost += 1;
                    break;

                case Type.Power:
                    GM.gmInstance.contPowerLost += 1;
                    LosePoint();
                    break;

                case Type.Coin:
                    GM.gmInstance.contCoinsLost += 1;
                    break;
            }
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
        GM.gmInstance.setScore(0, -10);
        lostFood = true;
    }

    public void setSpeed(float vel)
    {
        speed = vel;
    }

    public string getType()
    {
        return type.ToString();
    }
}