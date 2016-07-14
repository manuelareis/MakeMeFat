using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    [HideInInspector]public static PlayerScript playerInstance;
    [SerializeField]GameObject vomit;
    Transform playerTransf;
    Rigidbody2D rb2d;
    Animator aninPlayer;

    Camera cam;
    float camHeight;
    float camWidth;

    //static public PlayerScript player;
    public float speed = 10f; // speed of chracter
    public float startLife = 50f;
    public float rbSpeed = 500;

    Touch touch;

    enum Scene { scene1, scene2Touch, scene2Gyro }; //Parts of the game 
    Scene myScene;

    bool waiterInFront;
    [SerializeField]bool isGyro = false;

    void Start()
    {
        if (playerInstance == null)
            playerInstance = this;
        else
            Destroy(gameObject);


        //cam sizes-------------------------
        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
        print("Camheight " + camHeight);

        //get components--------------------
        aninPlayer = GetComponentInChildren<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        waiterInFront = false;
        //aninPlayer.SetBool("down", false);

    }

    void Update()
    {
        if (myScene == Scene.scene1)  //CUTSCENE
        {
            if (waiterInFront) {
                aninPlayer.SetTrigger("Down");
                Waiter.waiter.SendMessage("fallWaiter");
                GM.gmInstance.SendMessage("setScene2");
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
        //INPUT --------------------------------------------------------------------
         if (myScene == Scene.scene2Touch) // WALK WITH TOUCH
        {
            if (Input.GetButton("Fire1"))
            {
                aninPlayer.SetBool("walking", true); 
                float pos = Input.mousePosition.x;

                if (pos < Screen.width / 2){
                    if (transform.position.x > -camWidth + 1f) {
                        transform.Translate(Vector3.left * speed * Time.deltaTime);
                    }
                }

                else if (pos > Screen.width / 2){
                    if (transform.position.x < camWidth - 1f) {
                        transform.Translate(Vector3.right * speed * Time.deltaTime);
                    }           
                }
            }
        }

        else if (myScene == Scene.scene2Gyro)     // WALK WITH GYRO
        {
            aninPlayer.SetBool("walking", true); 

            Input.gyro.enabled = true;
            float gyroH = Input.gyro.gravity.x;

            print("pos: " + transform.position.x);
            if (gyroH < -0.1){
                if (transform.position.x > -camWidth + 1f){
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                }
            }
            else if (gyroH > 0.1){
                if (transform.position.x < camWidth - 1f){
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                }
            }
        }
    //---------------------------------------------------------------------------------
    }



    //COLLISIONS --------------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        print("colidiu " + other);
        if (other.gameObject.CompareTag("Waiter"))  // check collision with the waiter
        {
            waiterInFront = true;
        }


        else if (other.gameObject.CompareTag("Food")) // check collision with food 
        {
            aninPlayer.SetTrigger("Eating");
            GM.gmInstance.setScore(10, 5);
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("BadThing")) // check collision with food 
        {
            aninPlayer.SetTrigger("ThrowUp");
            vomit.SetActive(true);
            GM.gmInstance.setScore(0, -10);
            Destroy(other.gameObject);
        }
    }
    // ------------------------------------

    //SET SCENES --------------------------
    //public void startGame(bool isGyroScene2)
    //{
    //    isGyro = isGyroScene2;
    //    myScene = Scene.scene1;
    //    print("gyrofunc:" + isGyro);

    //}

    //MY FUNCTIONS ---------------------------
    public void GoCutScene(bool boolGyro)
    {
        isGyro = boolGyro;
        
        if (waiterInFront)
        {
            aninPlayer.SetTrigger("Down");
            Waiter.waiter.SendMessage("fallWaiter");
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
    //---------------------------------------
}