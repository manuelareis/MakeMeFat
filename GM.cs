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
    public Text coinText;
    public GameObject _slider;
    public GameObject[] listFood;
    public GameObject[] listBadThings;
    public GameObject[] listPowers;
    public GameObject coin;


    public GameObject menuGamveOver;
    public GameObject menuChooseMode;
    public GameObject menuPause;
    public GameObject canvasMenu;

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
    int coins = 0;

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

    bool score1 = true;
    bool score2 = false;
    bool score3 = false;
    bool score4 = false;
    bool score5 = false;
    void Start()
    {
        if (gmInstance != null)
            Destroy(gameObject);
        else
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
        canvasMenu.SetActive(true);

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
        waiter = (GameObject)Instantiate(waiter, new Vector3(-camWidth, -1.5f, 0f), Quaternion.identity);
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
    //FOOD GENERATORS
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

    IEnumerator CreatCoins()
    {
        while (!b_gameOver)
        {
            yield return new WaitForSeconds(30);
            int ranXposition = Random.Range(-(int)camWidth, (int)camWidth);
            Vector3 initalPosition = new Vector3(ranXposition, camHeight, 0f);
            int ranFood = Random.Range(0, listPowers.Length);
            GameObject instace = (GameObject)Instantiate(coin, initalPosition, Quaternion.identity);
        }
    }

    //
    void IncreaseDifficulty()
    {
        if (score >= 100 && score1 == true)
        {
            //1 -vel food,  2 -vel BadThings, 3- velPower, 4- Qt Food, 5- Qt BadThings and 6- level number
            AddVel(1f, 1f, 1f, 0.2f, 0.5f, 1);
            score1 = false;
            score2 = true;
        }

        else if (score >= 400 && score2 == true)
        {
            AddVel(0.5f, 0.5f, 0.5f, 0.2f, 0.5f, 2);
            score2 = false;
            score3 = true;
        }

        else if (score >= 800 && score3 == true)
        {
            AddVel(0.5f, 0.5f, 0.5f, 0.1f, 0.5f, 3);
            score3 = false;
            score4 = true;
        }

        else if (score >= 1200 && score4 == true)
        {
            AddVel(0.5f, 0.5f, 0.5f, 0.1f, 0.5f, 4);
            score4 = false;
            score5 = true;
        }

        else if (score >= 2000 && score5 == true)
        {
            AddVel(0.5f, 0.5f, 0.5f, 0.1f, 0.5f, 5);
            score5 = false;
        }

    }

    //function used inside Increase difficulty
    private void AddVel(float velFood, float velBadT, float velPower, float QtFood, float QtBadT, int numLevel)
    {
        speedFood += velFood;
        speedBadT += velBadT;
        speedPower += velPower;
        foodQuant -= QtFood;
        badThingsQuant -= QtBadT;
        print(numLevel + " e qtBt: " + badThingsQuant);
    }


    public void setScore(int valuescore, int valuelife) //SET SCORE
    {

        CheckGameOver();
        IncreaseDifficulty();

        if (life < 100 || b_gameOver == false)
        {
            if (bonusExtra) //Set life and Bonus with Extra
            {
                score += valuescore + 10;               //Bonus extra of 10 points
                scoreText.text = (score.ToString("000"));
                life += valuelife;
            }
            else
            {
                score += valuescore;                    //normal score and life
                scoreText.text = (score.ToString("000"));
                life += valuelife;
            }
        }
        else if (life >= 100)
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

    public void Coins()
    {

    }

    private void CheckGameOver()
    {
        if (life > 0)
        {
            b_gameOver = false;
        }

        else
        {
            b_gameOver = true;
            Time.timeScale = 0.5f;              //slowmotion

            playerScript.setDead();

            menuGamveOver.SetActive(true);
        }
    }

    public bool GetGamOver()
    {
        if (b_gameOver)
            return true;

        else
            return false;
    }

}
