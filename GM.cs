using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GM : MonoBehaviour
//Class responsable for game cycle 
{
    [HideInInspector]
    public int contFoodsEaten, contFoodsLost, contBadThingsEaten, contBadthingsLost, contPowersEaten, contPowerLost, contCoinsEaten, contCoinsLost; //game analytics variables
    [HideInInspector]
    public float averageSliderLifevalue, sliderLess30, sliderBtw40n70, sliderOver100; // game analytics variables 
    [HideInInspector]
    static public GM gmInstance;
    [SerializeField]
    GameObject player, waiter;
    [SerializeField]
    GameObject[] listFood;
    [SerializeField]
    GameObject[] listBadThings;
    [SerializeField]
    GameObject[] listPowers;
    [SerializeField]
    GameObject coin;
    [SerializeField]
    float intFoodDelay = 0.01f;
    [SerializeField]
    float timeCnt = 0.0f;
    BoxCollider2D footCollider;
    int score = 0;
    int highScore = 0;
    int coins = 0;
    float life = 50f;
    float foodQuant = 1f;
    float badThingsQuant = 3f;
    float powersQuant = 15f;
    float speedFood = 4f;
    float speedPower = 6f;
    float speedBadT = 5f;
    bool hitWaiter = false;
    bool creatSnacks = false;
    public static bool isGyro;
    bool playedFirst = false;
    bool b_gameOver = false;
    bool bonusExtra = false;
    bool startGame = false;
    bool score1 = true;
    bool score2, score3, score4, score5;
    enum GameParts { introGame, gameScreen, startMenu, settingsMenu, chooseMode };
    GameParts gamePart;

    void Start()
    {
        if (gmInstance != null)
            Destroy(gameObject);
        else
            gmInstance = this;

        DontDestroyOnLoad(transform.gameObject);


        //get script of player and set moves of scene 1
        footCollider = player.GetComponent<BoxCollider2D>();
        StartMenu();
    }

    //----GameParts------------------------------------
    /// <summary>
    /// Set the menu with the logo, the player with menu animation, and the settings menu tab aside
    /// </summary>
    public void StartMenu()
    {
        player = (GameObject)Instantiate(player, player.transform.position, Quaternion.identity);
        waiter = (GameObject)Instantiate(waiter, new Vector3(-CameraScript.camWidth, -1.5f, 0f), Quaternion.identity);
    }


    public void ResetGame()
    {
        gamePart = GameParts.introGame;
        StopAllCoroutines();
        score = 0;
        life = 50f;
        foodQuant = 1f;
        badThingsQuant = 3f;
        powersQuant = 15f;
        speedFood = 4f;
        speedPower = 6f;
        speedBadT = 5f;
        SetGameIntro();
        PlayerScript.playerInstance.ResetPlayer();
        hitWaiter = false;
        creatSnacks = false;
        b_gameOver = false;
        bonusExtra = false;
        startGame = false;
        score1 = true;
        score2 = false;
        score3 = false;
        score4 = false;
        score5 = false;
    }

    /// <summary>
    /// Set the firt part of game: the small introduction of waiter passing and the boy waiting 
    /// </summary>
    public void SetGameIntro() //receive from Hud if is Gyro Or Not
    {
        SoundsAndMusicMng.soundsIntance.StartCoroutine("StartMusicIntro");
        Time.timeScale = 1f;
        gamePart = GameParts.introGame;
        footCollider.enabled = false;
        //player = Instantiate(Resources.Load("Prefabs/Player", typeof(GameObject))) as GameObject;
        Waiter.waiterinstance.StartWalk();
    }

    /// <summary>
    /// The food start falling, and the game really starts
    /// </summary>
    public void StartGame()
    {
        gamePart = GameParts.gameScreen;

        if (isGyro)
            PlayerScript.playerInstance.SetGyro();
        else
            PlayerScript.playerInstance.SetTouch();

        StartCoroutine(CreatFood());
        StartCoroutine(CreatPowers());
        StartCoroutine(CreatBadThings());
        print("started game");
    }

    public void PushMenuSettings()
    {
        Rect windowRect = new Rect(20, 20, 120, 50);
    }
    //-----------------------------------
    IEnumerator CreatFood()
    {
        while (!b_gameOver)
        {
            yield return new WaitForSeconds(foodQuant);
            int ranXposition = UnityEngine.Random.Range(-(int)CameraScript.camWidth, (int)CameraScript.camWidth);
            Vector3 initalPosition = new Vector3(ranXposition, CameraScript.camHeight, 0f);
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
            int ranXposition = UnityEngine.Random.Range(-(int)CameraScript.camWidth, (int)CameraScript.camWidth);
            Vector3 initalPosition = new Vector3(ranXposition, CameraScript.camHeight, 0f);
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
            int ranXposition = UnityEngine.Random.Range(-(int)CameraScript.camWidth, (int)CameraScript.camWidth);
            Vector3 initalPosition = new Vector3(ranXposition, CameraScript.camHeight, 0f);
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
            int ranXposition = UnityEngine.Random.Range(-(int)CameraScript.camWidth, (int)CameraScript.camWidth);
            Vector3 initalPosition = new Vector3(ranXposition, CameraScript.camHeight, 0f);
            int ranFood = UnityEngine.Random.Range(0, listPowers.Length);
            GameObject instace = (GameObject)Instantiate(coin, initalPosition, Quaternion.identity);
        }
    }
    /// <summary>
    /// Increase the Difficulty accord with the score
    /// </summary>
    void IncreaseDifficulty()
    {
        if (score >= 100 && score1 == true)
        {
            //1 -vel food,  2 -vel BadThings, 3- velPower, 4- Qt Food, 5- Qt BadThings and 6- level number
            AddVel(1f, 1f, 1f, 0.2f, 0.5f, 1);
            score1 = false;
            score2 = true;
        }

        else if (score >= 200 && score2 == true)
        {
            AddVel(0.5f, 0.5f, 0.5f, 0.2f, 0.8f, 2);
            score2 = false;
            score3 = true;
        }

        else if (score >= 800 && score3 == true)
        {
            AddVel(0.5f, 0.5f, 0.5f, 0.1f, 1.0f, 3);
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

    /// <summary>
    /// Add difficulty for the game for each type of elements falling. Needed the new velocity of each type and number to decrease time of respawn
    /// </summary>
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
                //scoreText.text = (score.ToString("000"));
                life += valuelife;
            }
            else
            {
                score += valuescore;                    //normal score and life
                //scoreText.text = (score.ToString("000"));
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
        if (highScore > score)
        {
            highScore = score;
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

    /// <summary>
    /// Check if the game is over set slowmotion, the dead animation of player, and the game over menu
    /// </summary>
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

            PlayerScript.playerInstance.setDead();

            // canvasGamveOver.SetActive(true);
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

    /// <summary>
    /// Save the high score, if is not the first match, and the play mode: gyroscope or touch 
    /// </summary>
    public void Save()
    {
        BinaryFormatter binaryf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.data");
        PlayerData data = new PlayerData();
        data.scoreRecord = highScore;
        data.isGyro = isGyro;
        data.playedFirst = playedFirst;
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
            highScore = data.scoreRecord;
            isGyro = data.isGyro;
        }
    }

    void OnApplicationQuit()
    {
        Save();
        MyAnalyticsManager.PublishMatchStoppedPerformance(timeCnt, contFoodsEaten, contBadThingsEaten, contPowersEaten, contCoinsEaten, contCoinsLost, contPowerLost, contFoodsLost, contBadthingsLost, averageSliderLifevalue, sliderOver100, sliderBtw40n70, sliderLess30);
    }

    void OnApplicationPause(bool pauseStatus)
    {
        MenusManager.hudInstance.PauseMenu();
        // canvasPause.SetActive(true);
    }
}

[Serializable]
class PlayerData
{
    public bool playedFirst;
    public int scoreRecord;
    public bool isGyro;

}
