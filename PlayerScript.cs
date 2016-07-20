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

    Camera cam;
    float camHeight;
    float camWidth;

    public float speed = 10f;
    public float startLife = 50f;
    public float rbSpeed = 500;

    Touch touch;

    enum Scene { scene1, scene2Touch, scene2Gyro };
    Scene myScene;

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

        //cam sizes
        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        aninPlayer = GetComponentInChildren<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        waiterInFront = false;
        //aninPlayer.SetBool("down", false);

    }

    void Update()
    {
        //CUTSCENE
        if (myScene == Scene.scene1)
        {
            if (waiterInFront)
            {
                aninPlayer.SetTrigger("Down");
                Waiter.waiterinstance.fallWaiter();
                GM.gmInstance.StartGame();
                print("isgyro: " + isGyro);
                if (isGyro)
                    myScene = Scene.scene2Gyro;
                else
                    myScene = Scene.scene2Touch;
            }
        }
    }

    void FixedUpdate()
    {
        //INPUT CONTROLLERS --------------------------------------------------------------------
        //if the player has chosen the tpouch mode this will be the input used,
        isGameOver = GM.gmInstance.GetGamOver();
        if (myScene == Scene.scene2Touch && !isGameOver)
        {
            if (Input.GetButton("Fire1"))
            {
                aninPlayer.SetBool("walking", true);
                float pos = Input.mousePosition.x;

                if (pos < Screen.width / 2)
                {
                    if (transform.position.x > -camWidth + 1f)
                    {
                        transform.Translate(Vector3.left * speed * Time.deltaTime);
                    }
                }

                else if (pos > Screen.width / 2)
                {
                    if (transform.position.x < camWidth - 1f)
                    {
                        transform.Translate(Vector3.right * speed * Time.deltaTime);
                    }
                }
            }
        }

        //change the controlers of the game for using gyroscope 
        else if (myScene == Scene.scene2Gyro && !isGameOver)
        {
            aninPlayer.SetBool("walking", true);

            Input.gyro.enabled = true;
            float gyroH = Input.gyro.gravity.x;

            if (gyroH < -0.1)
            {
                if (transform.position.x > -camWidth + 1f)
                {
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                }
            }
            else if (gyroH > 0.1)
            {
                if (transform.position.x < camWidth - 1f)
                {
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                }
            }
        }
    }
    //COLLISIONS --------------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Waiter"))
        {
            waiterInFront = true;
        }
        else if (other.gameObject.CompareTag("Food"))
        {
            aninPlayer.SetTrigger("Eating");
            GM.gmInstance.setScore(5, 5);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("BadThing"))
        {
            aninPlayer.SetTrigger("ThrowUp");
            vomit.SetActive(true);
            GM.gmInstance.setScore(0, -20);
            Destroy(other.gameObject);
        }

        //compare with especial food that gives the player some power
        else if (other.gameObject.CompareTag("ChilliPower"))
        {
            // adicionar animacao  chilli
            aninPlayer.SetTrigger("Eating");
            GM.gmInstance.setScore(15, 10);
            GM.gmInstance.StartCoroutine("PowerUpChilli");
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("ApplePower"))
        {
            // adicionar animacao  apple
            aninPlayer.SetTrigger("Eating");
            GM.gmInstance.setScore(15, 10);
            GM.gmInstance.StartCoroutine("PowerUpApple");
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("Coin"))
        {
            // adicionar animacao  apple
            aninPlayer.SetTrigger("Eating");
            Destroy(other.gameObject);
        }
    }

    // The cutscene that happens before the game begins 
    public void GoCutScene(bool boolGyro)
    {
        isGyro = boolGyro;

        if (waiterInFront)
        {
            aninPlayer.SetTrigger("Down");
            Waiter.waiterinstance.SendMessage("fallWaiter");
            GM.gmInstance.SendMessage("setScene2");
            print("isgyro: " + isGyro);
            if (isGyro)
                myScene = Scene.scene2Gyro;
            else
                myScene = Scene.scene2Touch;
        }
    }
    public void setDead()
    {
        aninPlayer.SetTrigger("Dead");
    }
    public void openMouth()
    {
        aninPlayer.SetBool("openMouth", true); // set animation of opening mouth
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }

    public float GetSpeed()
    {
        return speed;
    }

    //---------------------------------------
}