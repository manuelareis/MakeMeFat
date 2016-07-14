using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM : MonoBehaviour
    //Class responsable for game cycle 
{
    [HideInInspector]static public GM gmInstance; 
    [SerializeField]private GameObject player, waiter;
    PlayerScript playerScript;

    public Text scoreText;
    public GameObject _slider;
    public GameObject[] listFood;
    public GameObject[] listBadThings;
    public GameObject[] listPowers;
     
    public GameObject menuGamveOver;
    public GameObject menuChooseMode;
    public GameObject menuPause;
    public GameObject menuTutorial;

    private BoxCollider2D footCollider;
    private bool hitWaiter = false;
    private bool creatSnacks = false; 
    [SerializeField]private float intFoodDelay = 0.01f;
    private int score = 0;
    private float life = 50f;
    float foodQuant = 0.5f;
    float badThingsQuant = 1f;
    float powersQuant = 5f;
    float speed = 1f;

    //cam sizes
    Camera cam;
    float camWidth;
    float camHeight;

    bool b_gameOver = false;
    bool bonusExtra = false;
    bool isGyro = false;

    enum Scene { scene1, scene2 };
    Scene scene;

    void Start()
    {       
        gmInstance = this;

        //float widthInInches = Screen.width / Screen.dpi;
        //Screen.orientation = ScreenOrientation.Landscape;

        //get cam hieght and cam width
        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;


        //get script of player and set moves of scene 1
        footCollider = player.GetComponent<BoxCollider2D>();

        //initial
        menuGamveOver.SetActive(false);
        _slider.SetActive(false);
        menuChooseMode.SetActive(true);
        menuPause.SetActive(false);
        menuTutorial.SetActive(false);


    }

    void Update()
    {
        if (scene == Scene.scene2 && !b_gameOver) {
            life -= Time.deltaTime * 3;
        }
        HudManager.hudInstance.UpdateSlider(life);
    }

    //----SCENES----------------------
    //Scene 1: cut scene |  Scene2: the game

    public void setScene1(bool scene2GyroTrueOrFalse) //receive from Hud if is Gyro Or Not
    {
        Time.timeScale = 1f;
        scene = Scene.scene1;
        footCollider.enabled = false;
        //player = Instantiate(Resources.Load("Prefabs/Player", typeof(GameObject))) as GameObject;
        player = (GameObject)Instantiate(player, player.transform.position, Quaternion.identity);
        waiter = (GameObject)Instantiate(waiter, waiter.transform.position, Quaternion.identity);
        playerScript = player.GetComponent<PlayerScript>();

        playerScript.GoCutScene(scene2GyroTrueOrFalse);
    }
    void setScene2()
    {
        scene = Scene.scene2;
        if (!b_gameOver) {
            StartCoroutine(StartCreating(listFood, foodQuant));
            StartCoroutine(StartCreating(listBadThings, badThingsQuant));
            StartCoroutine(StartCreating(listPowers, powersQuant));
            IncreaseDificulty();
            }
    }
    //-----------------------------------
    public void CheckGameOver()
    {
        if (life > 0)
            b_gameOver = false;

        else if (life < 0) { 
            b_gameOver = true;
            b_gameOver = true;
            Time.timeScale = 0.5f;              //slowmotion

            playerScript.setDead();

            menuGamveOver.SetActive(true);
        }
    }

    IEnumerator StartCreating(GameObject[] listToBeCreated, float timeQtFood){
        _slider.SetActive(true);        
                   
        while (!b_gameOver){
            CheckGameOver();
            yield return new WaitForSeconds(timeQtFood);
            int ranFood = Random.Range(0, listToBeCreated.Length );
            int ranXposition = Random.Range(-(int)camWidth, (int)camWidth);
            Vector3 initalPosition = new Vector3(ranXposition, camHeight, 0f);
            Instantiate(listToBeCreated[ranFood], initalPosition, Quaternion.identity);
            Foods.speed = speed;
        }
    }

    IEnumerator IncreaseDificulty()
    {
        float dificulty = 1;
        while (dificulty < 10)
        {
            yield return new WaitForSeconds(2);
            speed += 5;
            foodQuant -= 3f; //time
            badThingsQuant -= 10;
            dificulty += 1;
        }
    }
    public void setScore(int score, int life) //SET SCORE
    {
        if (life < 100 || b_gameOver == false)
        {
            if (bonusExtra) //Set life and Bonus with Extra
            {
                this.score += score + 10;               //Bonus extra of 10 points
                scoreText.text = ("S " + this.score);
                this.life += life;
            }

            else
            {
                this.score += score;                    //normal score and life
                scoreText.text = ("S " + this.score);
                this.life += life;
            }
        }
        else if (life >= 100)
            SetBonus();
    }
    public void SetBonus()
    {
        bonusExtra = true;
    }



}
