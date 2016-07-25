using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GM : MonoBehaviour
//Class responsable for game cycle 
{
    public int contFoodsEaten, contFoodsLost, contBadThingsEaten, contBadthingsLost, contPowersEaten, contPowerLost, contCoinsEaten, contCoinsLost; //game analytics variables
    public float averageSliderLifevalue, sliderLess30, sliderBtw40n70, sliderOver100; // game analytics variables 

    [HideInInspector]
    static public GM gmInstance;
    [SerializeField]
    GameObject player, waiter;
    PlayerScript playerScript;

    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text coinText;
    [SerializeField]
    GameObject _slider;
    [SerializeField]
    GameObject[] listFood;
    [SerializeField]
    GameObject[] listBadThings;
    [SerializeField]
    GameObject[] listPowers;
    [SerializeField]
    GameObject coin;
    [SerializeField]
    GameObject menuGamveOver;
    [SerializeField]
    GameObject menuChooseMode;
    [SerializeField]
    GameObject menuPause;
    [SerializeField]
    GameObject canvasMenu;
    [SerializeField]
    float intFoodDelay = 0.01f;
    [SerializeField]
    float timeCnt = 0.0f;

    BoxCollider2D footCollider;
    bool hitWaiter = false;
    bool creatSnacks = false;

    int score = 0;
    int scoreRecord = 0;
    float life = 50f;

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
            timeCnt += Time.deltaTime;
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
        isGyro = scene2GyroTrueOrFalse;
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
            int ranXposition = UnityEngine.Random.Range(-(int)camWidth, (int)camWidth);
            Vector3 initalPosition = new Vector3(ranXposition, camHeight, 0f);
            int ranFood = UnityEngine.Random.Range(0, listFood.Length);
            GameObject instace = (GameObject)Instantiate(listFood[ranFood], initalPosition, Quaternion.identity);
            instace.GetComponent<Foods>().setSpeed(speedFood);
        }
    }
    IEnumerator CreatBadThings()
    {
        while (!b_gameOver)
        {
            yield return new WaitForSeconds(badThingsQuant);
            int ranXposition = UnityEngine.Random.Range(-(int)camWidth, (int)camWidth);
            Vector3 initalPosition = new Vector3(ranXposition, camHeight, 0f);
            int ranFood = UnityEngine.Random.Range(0, listBadThings.Length);
            GameObject instace = (GameObject)Instantiate(listBadThings[ranFood], initalPosition, Quaternion.identity);
            instace.GetComponent<Foods>().setSpeed(speedBadT);
        }
    }
    IEnumerator CreatPowers()
    {
        while (!b_gameOver)
        {
            yield return new WaitForSeconds(powersQuant);
            int ranXposition = UnityEngine.Random.Range(-(int)camWidth, (int)camWidth);
            Vector3 initalPosition = new Vector3(ranXposition, camHeight, 0f);
            int ranFood = UnityEngine.Random.Range(0, listPowers.Length);
            GameObject instace = (GameObject)Instantiate(listPowers[ranFood], initalPosition, Quaternion.identity);
            instace.GetComponent<Foods>().setSpeed(speedPower);
        }
    }

    IEnumerator CreatCoins()
    {
        while (!b_gameOver)
        {
            yield return new WaitForSeconds(30);
            int ranXposition = UnityEngine.Random.Range(-(int)camWidth, (int)camWidth);
            Vector3 initalPosition = new Vector3(ranXposition, camHeight, 0f);
            int ranFood = UnityEngine.Random.Range(0, listPowers.Length);
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
        CheckRecord();
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

    public void CheckRecord()
    {
        if (scoreRecord > score)
        {
            scoreRecord = score;
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
            //Send performance to game analytics
            MyAnalyticsManager.PublishMatchPerformance(timeCnt, contFoodsEaten, contBadThingsEaten, contPowersEaten, contCoinsEaten, contCoinsLost, contPowerLost, contFoodsLost, contBadthingsLost, averageSliderLifevalue, sliderOver100, sliderBtw40n70, sliderLess30);

        }
    }

    public bool GetGamOver()
    {
        if (b_gameOver)
            return true;

        else
            return false;
    }

    public void Save()
    {
        BinaryFormatter binaryf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.data");

        PlayerData data = new PlayerData();
        data.scoreRecord = scoreRecord;
        data.isGyro = isGyro;

        binaryf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter binaryf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)binaryf.Deserialize(file);
            file.Close();
            scoreRecord = data.scoreRecord;
            isGyro = data.isGyro;

        }
    }

    void OnApplicationQuit()
    {
        Save();
        MyAnalyticsManager.PublishMatchStoppedPerformance(timeCnt, contFoodsEaten, contBadThingsEaten, contPowersEaten, contCoinsEaten, contCoinsLost, contPowerLost, contFoodsLost, contBadthingsLost, averageSliderLifevalue, sliderOver100, sliderBtw40n70, sliderLess30);
    }

}

[Serializable]
class PlayerData
{
    public int scoreRecord;
    public bool isGyro;

}
