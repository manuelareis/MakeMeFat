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

    //cam sizes
    Camera cam;
    float camWidth;
    float camHeight;

    bool boolgameOver = false;
    bool bonusExtra = false;
    bool isGyro = false;

    enum Scene { scene1, scene2 };
    Scene scene;

    void Start()
    {       
        gmInstance = this;

        //float widthInInches = Screen.width / Screen.dpi;
        Screen.orientation = ScreenOrientation.Landscape;

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
        if (scene == Scene.scene2 && life > 0)
        {
            life -= Time.deltaTime * 3;
        }
        else if (life <= 0)
        {
            GameOver();
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
        Instantiate(waiter, waiter.transform.position, Quaternion.identity);
        playerScript = player.GetComponent<PlayerScript>();

        playerScript.GoCutScene(scene2GyroTrueOrFalse);
    }

    void setScene2()
    {
        scene = Scene.scene2;
        InvokeRepeating("CreatFood", intFoodDelay, intFoodDelay);
        InvokeRepeating("CreatFood", intFoodDelay, 0.2f);

    }
    //-----------------------------------

    public void CreatFood() //CREAT FOOD
    {
        _slider.SetActive(true);
        
        
        if (life > 0)
        {
            int ranFood = Random.Range(0, listFood.Length);
            int ranXposition = Random.Range(-(int)camWidth, (int)camWidth);
            Vector3 snackPosition = new Vector3(ranXposition, camHeight, 0f);
            Instantiate(listFood[ranFood], snackPosition, Quaternion.identity);
        }
        else
            GameOver();
    }

    public void setScore(int score, int life) //SET SCORE
    {
        if (life < 100 || boolgameOver == false)
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

    public void LoseLife(int num) //LOOSE LIFE
    {
        life -= num;
    }

    public void SetBonus()
    {
        bonusExtra = true;
    }

    public void GameOver()
    {
        boolgameOver = true;
        Time.timeScale = 0.5f;              //slowmotion

        playerScript.setDead();

        menuGamveOver.SetActive(true);
    }


}
