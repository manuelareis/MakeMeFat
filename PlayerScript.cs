using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    [HideInInspector]
    public static PlayerScript playerInstance;
    [SerializeField]
    GameObject vomit;
    Transform playerTransf;
    Rigidbody2D rb2d;
    Animator aninPlayer;
    Touch touch;
    [SerializeField]
    float speed = 10f;
    [SerializeField]
    float rbSpeed = 500;
    float startLife = 50f;
    Vector3 initalposition = new Vector3(0f, -2.9f, 0f);

    enum PlayerControl { introGame, gameTouch, gameGyro };
    PlayerControl playerControl;

    bool waiterInFront;
    bool isGyro = false;
    bool isGameOver = false;

    void Start()
    {
        //singleton mode, to make this script unique
        if (playerInstance == null)
            playerInstance = this;
        else
            Destroy(gameObject);

        aninPlayer = GetComponentInChildren<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        waiterInFront = false;
        transform.position = initalposition;
        //aninPlayer.SetBool("down", false);
    }

    void FixedUpdate()
    {
        //INPUT CONTROLLERS --------------------------------------------------------------------
        //if the player has chosen the touch mode this will be the input used,
        isGameOver = GM.gmInstance.GetGamOver();
        if (playerControl == PlayerControl.gameTouch && !isGameOver)
        {
            if (Input.GetButton("Fire1"))
            {
                aninPlayer.SetBool("walking", true);
                float pos = Input.mousePosition.x;

                if (pos < Screen.width / 2)
                {
                    if (transform.position.x > -CameraScript.camWidth + 1f)
                    {
                        transform.Translate(Vector3.left * speed * Time.deltaTime);
                    }
                }
                else if (pos > Screen.width / 2)
                {
                    if (transform.position.x < CameraScript.camWidth - 1f)
                    {
                        transform.Translate(Vector3.right * speed * Time.deltaTime);
                    }
                }
            }
        }
        //change the controlers of the game for using gyroscope 
        else if (playerControl == PlayerControl.gameGyro && !isGameOver)
        {
            aninPlayer.SetBool("walking", true);

            Input.gyro.enabled = true;
            float gyroH = Input.gyro.gravity.x;

            if (gyroH < -0.1)
            {
                if (transform.position.x > -CameraScript.camWidth + 1f)
                {
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                }
            }
            else if (gyroH > 0.1)
            {
                if (transform.position.x < CameraScript.camWidth - 1f)
                {
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                }
            }
        }
    }
    //COLLISIONS -----------------------------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            aninPlayer.SetTrigger("Eating");
            GM.gmInstance.setScore(5, 5);
            Destroy(other.gameObject);
            GM.gmInstance.contFoodsEaten += 1;
        }
        else if (other.gameObject.CompareTag("BadThing"))
        {
            aninPlayer.SetTrigger("ThrowUp");
            vomit.SetActive(true);
            GM.gmInstance.setScore(0, -20);
            Destroy(other.gameObject);
            GM.gmInstance.contBadThingsEaten += 1;
        }

        //compare with especial food that gives the player some power
        else if (other.gameObject.CompareTag("ChilliPower"))
        {
            // adicionar animacao  chilli
            aninPlayer.SetTrigger("Eating");
            GM.gmInstance.setScore(15, 10);
            GM.gmInstance.StartCoroutine("PowerUpChilli");
            Destroy(other.gameObject);
            GM.gmInstance.contPowersEaten += 1;
        }

        else if (other.gameObject.CompareTag("ApplePower"))
        {
            // adicionar animacao  apple
            aninPlayer.SetTrigger("Eating");
            GM.gmInstance.setScore(15, 10);
            GM.gmInstance.StartCoroutine("PowerUpApple");
            Destroy(other.gameObject);
            GM.gmInstance.contPowersEaten += 1;
        }

        else if (other.gameObject.CompareTag("Coin"))
        {
            // adicionar animacao  apple
            aninPlayer.SetTrigger("Eating");
            Destroy(other.gameObject);
            GM.gmInstance.contCoinsEaten += 1;
        }
    }

    public void ResetPlayer()
    {
        aninPlayer.SetBool("Dead", false);
        aninPlayer.SetBool("walking", false);
        transform.position = initalposition;
        playerControl = PlayerControl.introGame;
    }
    /// <summary>
    /// Set Dead Animation of the player
    /// </summary>
    public void setDead()
    {
        aninPlayer.SetBool("Dead", true);
    }

    /// <summary>
    /// Set Animation waiting for food fall, with mouth opened
    /// </summary>
    public void openMouth()
    {
        aninPlayer.SetBool("openMouth", true); // set animation of opening mouth
    }

    /// <summary>
    /// Set the speed of player
    /// </summary>
    public void SetSpeed(float value)
    {
        speed = value;
    }

    /// <summary>
    /// Get the speed of the player
    /// </summary>
    public float GetSpeed()
    {
        return speed;
    }

    public void MoveDown()
    {
        aninPlayer.SetTrigger("Down");
    }

    public void SetIntro()
    {
        playerControl = PlayerControl.introGame;
    }
    public void SetGyro()
    {
        playerControl = PlayerControl.gameGyro;
    }
    public void SetTouch()
    {
        playerControl = PlayerControl.gameTouch;
    }

    //----------------------------------------------------
}