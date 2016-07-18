using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM : MonoBehaviour
//Class responsable for game cycle 
{
    [HideInInspector]
    static public GM gmInstance;
    [SerializeField]
    private GameObject player, waiter;
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
    [SerializeField]
    private float intFoodDelay = 0.01f;
    private int score = 0;
    private float life = 50f;

    float foodQuant = 1f;
    float badThingsQuant = 3f;
    float powersQuant = 15f;
    float speedFood = 4f;
    float speedPower = 6f;
    float speedBadT = 5f;

    //cam sizes
    Camera cam;
    float camWidth;
    float camHeight;

    bool b_gameOver = false;
    bool bonusExtra = false;
    bool isGyro = false;

    enum Scene { scene1, scene2 };
    Scene scene;

    bool startGame = false;

    void Start()
    {
        gmInstance = this;

        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;


        //get script of player and set moves of scene 1
        footCollider = player.GetComponent<BoxCollider2D>();

        //initial menus 
        menuGamveOver.SetActive(false);
        _slider.SetActive(false);
        menuChooseMode.SetActive(true);
        menuPause.SetActive(false);
        menuTutorial.SetActive(false);


    }

    void Update()
    {
        if (scene == Scene.scene2 && !b_gameOver)
        {
            life -= Time.deltaTime * 3;
            HudManager.hudInstance.UpdateSlider(life);
        }
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
    public void StartGame()
    {
        scene = Scene.scene2;
        _slider.SetActive(true);
        StartCoroutine(CreatFood());
        StartCoroutine(CreatPowers());
        StartCoroutine(CreatBadThings());
    }
    //-----------------------------------

    IEnumerator CreatFood()
    {
        while (!b_gameOver)
        {
            yield return new WaitForSeconds(foodQuant);
            int ranXposition = Random.Range(-(int)camWidth, (int)camWidth);
            Vector3 initalPosition = new Vector3(ranXposition, camHeight, 0f);
            int ranFood = Random.Range(0, listFood.Length);
            GameObject instace = (GameObject)Instantiate(listFood[ranFood], initalPosition, Quaternion.identity);
            instace.GetComponent<Foods>().setSpeed(speedFood);
        }
    }
    IEnumerator CreatBadThings()
    {
        while (!b_gameOver)
        {
            yield return new WaitForSeconds(badThingsQuant);
            int ranXposition = Random.Range(-(int)camWidth, (int)camWidth);
            Vector3 initalPosition = new Vector3(ranXposition, camHeight, 0f);
            int ranFood = Random.Range(0, listBadThings.Length);
            GameObject instace = (GameObject)Instantiate(listBadThings[ranFood], initalPosition, Quaternion.identity);
            instace.GetComponent<Foods>().setSpeed(speedBadT);
        }
    }
    IEnumerator CreatPowers()
    {
        while (!b_gameOver)
        {
            yield return new WaitForSeconds(powersQuant);
            int ranXposition = Random.Range(-(int)camWidth, (int)camWidth);
            Vector3 initalPosition = new Vector3(ranXposition, camHeight, 0f);
            int ranFood = Random.Range(0, listPowers.Length);
            GameObject instace = (GameObject)Instantiate(listPowers[ranFood], initalPosition, Quaternion.identity);
            instace.GetComponent<Foods>().setSpeed(speedPower);
        }
    }
    void IncreaseDifficulty()
    {
        if (score == 100)
        {
            speedFood += 1f;
            speedBadT += 1f;
            speedPower += 1f;
            foodQuant -= 0.2f;
            badThingsQuant -= 0.5f;
        }

        else if (score == 400)
        {
            speedFood += 0.5f;
            speedBadT += 0.5f;
            speedPower += 0.5f;
            foodQuant -= 0.2f;
            badThingsQuant -= 0.2f;

        }

        else if (score == 800)
        {
            speedFood += 0.5f;
            speedBadT += 0.5f;
            speedPower += 0.5f;
            foodQuant -= 0.1f;
            badThingsQuant -= 0.5f;

        }

        else if (score == 1200)
        {
            speedFood += 0.5f;
            speedBadT += 0.5f;
            speedPower += 0.5f;
            foodQuant -= 0.1f;
            badThingsQuant -= 0.5f;

        }

        else if (score == 2000)
        {
            speedFood += 1f;
            speedBadT += 1f;
            speedPower += 1f;
            foodQuant -= 0.1f;
            badThingsQuant -= 0.5f;
        }

    }

    public void setScore(int score, int life) //SET SCORE
    {

        CheckGameOver();
        IncreaseDifficulty();

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
        if (life >= 100)
        {
            SetBonus();
            print("BONUS!");
        }
    }
    public void SetBonus()
    {
        bonusExtra = true;
    }

    public IEnumerator PowerUpApple()
    {
        float TempspeedFood = speedFood;
        float TempspeedPower = speedPower;
        float TempspeedBadT = speedBadT;
        speedFood -= 3f;
        speedPower -= 3f;
        speedBadT -= 3f;

        yield return new WaitForSeconds(7);

        speedFood = TempspeedFood;
        speedPower = TempspeedPower;
        speedBadT = TempspeedBadT;

    }

    public IEnumerator PowerUpChilli()
    {
        float tempSpeed = PlayerScript.playerInstance.GetSpeed();
        PlayerScript.playerInstance.SetSpeed(20);
        yield return new WaitForSeconds(7);
        PlayerScript.playerInstance.SetSpeed(tempSpeed);
    }

    public void CheckGameOver()
    {
        if (life > 0)
            b_gameOver = false;

        else if (life < 0)
        {
            b_gameOver = true;
            b_gameOver = true;
            Time.timeScale = 0.5f;              //slowmotion

            playerScript.setDead();

            menuGamveOver.SetActive(true);
        }
    }

}
