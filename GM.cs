using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
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

    void Awake()
    {
        if (gmInstance != null)
            Destroy(gameObject);
        else
            gmInstance = this;
    }
    /// <summary>
    /// Instatiate the player and the waiter for the start menu
    /// </summary>
    public void StartMenu()
    {
        //player = Instantiate(Resources.Load("Prefabs/Player", typeof(GameObject))) as GameObject;
        player = (GameObject)Instantiate(player, player.transform.position, Quaternion.identity);
        waiter = (GameObject)Instantiate(waiter, new Vector3(-CameraScript.camWidth, -1.5f, 0f), Quaternion.identity);
    }
    /// <summary>
    /// Set the firt part of game: the small introduction of waiter passing and the boy waiting 
    /// </summary>
    public void SetGameIntro()
    {
        Time.timeScale = 1f;
        gamePart = GameParts.introGame;
        // footCollider.enabled = false;      
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
        StartCoroutine(CreatCoins());
        print("started game");
    }
    IEnumerator CreatFood()
    {
        while (!b_gameOver)
        {
            yield return new WaitForSeconds(foodQuant);
            int ranXposition = UnityEngine.Random.Range(-(int)CameraScript.camWidth, (int)CameraScript.camWidth);
            Vector3 initalPosition = new Vector3(ranXposition, CameraScript.camHeight + 2f, 0f);
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
            Vector3 initalPosition = new Vector3(ranXposition, CameraScript.camHeight + 2f, 0f);
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
            Vector3 initalPosition = new Vector3(ranXposition, CameraScript.camHeight + 2f, 0f);
            int ranFood = UnityEngine.Random.Range(0, listPowers.Length);
            GameObject instace = (GameObject)Instantiate(listPowers[ranFood], initalPosition, Quaternion.identity);
            instace.GetComponent<Foods>().setSpeed(speedPower);
        }
    }
    IEnumerator CreatCoins()
    {
        while (!b_gameOver)
        {
            yield return new WaitForSeconds(15);
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
    /// <summary>
    /// Update the score and the life, send the menus manager the new score to print in the screen
    /// </summary>
    /// <param name="valuescore"></param>
    /// <param name="valuelife"></param>
    public void setScore(int valuescore, int valuelife)
    {
        CheckRecord();
        CheckGameOver();
        IncreaseDifficulty();
        if (life < 100 || b_gameOver == false)
        {
            if (bonusExtra) //Set life and Bonus with Extra
            {
                score += valuescore + 10; //bonus
                life += valuelife;
                MenusManager.hudInstance.SetScoreLife(score, life);
            }
            else
            {
                score += valuescore;                    //normal score and life
                life += valuelife;
                MenusManager.hudInstance.SetScoreLife(score, life);
            }
        }
        else if (life >= 100)
        {
            SetBonus();
            print("BONUS!");
        }
    }
    /// <summary>
    /// Compare the actual score with the highscore to see if the player got a new highscore
    /// </summary>
    public void CheckRecord()
    {
        if (highScore > score)
        {
            highScore = score;
        }
    }
    /// <summary>
    /// allow the player to gain more 10 points bisides his normal score
    /// </summary>
    public void SetBonus()
    {
        bonusExtra = true;
    }
    /// <summary>
    /// Set the Apple Power Up where all the things are falling get slower for 7 sec
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// Enable the powerUp of chili where the player speed is increased for 7 sec
    /// </summary>
    /// <returns></returns>
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
            PlayerScript.playerInstance.setDead(); print("GameOver, comilao dead, gameOverMenu");
            MenusManager.hudInstance.GameOverMenu();
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
    /// <summary>
    /// Saves the game highscore, settings preferences and send the Analytics 
    /// </summary>
    void OnApplicationQuit()
    {
        Save();
        MyAnalyticsManager.PublishMatchStoppedPerformance(timeCnt, contFoodsEaten, contBadThingsEaten, contPowersEaten, contCoinsEaten, contCoinsLost, contPowerLost, contFoodsLost, contBadthingsLost, averageSliderLifevalue, sliderOver100, sliderBtw40n70, sliderLess30);
    }
}

[Serializable]
class PlayerData
{
    public bool playedFirst;
    public int scoreRecord;
    public bool isGyro;
}
