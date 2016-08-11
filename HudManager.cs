using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenusManager : MonoBehaviour
{
    //class responsable for Buttons and Hud   
    [HideInInspector]
    public static MenusManager hudInstance;
    [SerializeField]
    Button btnContinue;
    [SerializeField]
    Button btnPause;
    [SerializeField]
    Button btnTryAgain;
    [SerializeField]
    Button btnTryAgainGO;
    [SerializeField]
    Button btnSetTouch;
    [SerializeField]
    Button btnSetGyro;
    [SerializeField]
    Button btnStartGame;
    [SerializeField]
    Button btnGoMainMenu;
    [SerializeField]
    Button btnSettings;
    [SerializeField]
    Slider slider;
    [SerializeField]
    float sliderSize;
    [SerializeField]
    GameObject canvasGamveOver;
    [SerializeField]
    GameObject canvasChooseMode;
    [SerializeField]
    GameObject canvasPause;
    [SerializeField]
    GameObject canvasStartMenu;
    [SerializeField]
    GameObject canvasSettings;
    [SerializeField]
    GameObject canvasHud;
    //[SerializeField] GameObject _slider;
    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text coinText;
    [SerializeField]
    Animator anim;

    void Start()
    {
        // btnPause.onClick.AddListener(() => { PauseMenu(); }); // if needed to accept any arguments 
        hudInstance = this;
        btnPause.onClick.AddListener(PauseMenu);
        btnContinue.onClick.AddListener(ContinueGame);
        btnTryAgain.onClick.AddListener(TryAgainBtn);
        btnTryAgainGO.onClick.AddListener(TryAgainBtn);
        // btnStartGame.onClick.AddListener(StartGame);
        btnGoMainMenu.onClick.AddListener(GoMainMenu);
        canvasStartMenu.SetActive(true);
        canvasSettings.SetActive(true);
        canvasHud.SetActive(false);
        canvasChooseMode.SetActive(false);
        canvasPause.SetActive(false);
        SoundsAndMusicMng.soundsIntance.SetMusicMenu();
        // _slider.SetActive(false);
    }
    /// <summary>
    /// Open or close The settings menu in the main menu
    /// </summary>
    public void OpenCloseSettings()
    {
        if (anim.GetBool("Come").Equals(false))
        {
            anim.SetBool("Come", true);
            anim.SetBool("Go", false);
            btnStartGame.interactable = false;
        }

        else if (anim.GetBool("Come").Equals(true))
        {
            anim.SetBool("Go", true);
            anim.SetBool("Come", false);
            btnStartGame.interactable = true;
        }
    }
    /// <summary>
    /// Write the num in the HUD
    /// </summary>
    /// <param name="num"></param>
    public void SetLife(float num)
    {
        slider.value += num;
    }
    /// <summary>
    /// Enable the slider  and starts updating 
    /// </summary>
    public void StartSlider()
    {
        InvokeRepeating("LifeTime", 0.1f, 0.1f);
    }
    /// <summary>
    /// Decrease life by time
    /// </summary>
    void LifeTime()
    {
        slider.value -= 0.5f;
    }
    /// <summary>
    /// When the player touchs in the screen and the game manager is called to start the game
    /// </summary>
    public void StartGame()
    {
        anim.SetBool("StartGame", true);
        SoundsAndMusicMng.soundsIntance.StartCoroutine("StartMusicIntro");
        GM.gmInstance.SetGameIntro();
        canvasHud.SetActive(true);
        // _slider.SetActive(true);
        InvokeRepeating("LifeTime", 0f, 0.1f);
    }
    /// <summary>
    /// Called to reset the game and go to main menu
    /// </summary>
    public void GoMainMenu()
    {
        Time.timeScale = 1f;
        anim.SetBool("StartGame", false);
        SoundsAndMusicMng.soundsIntance.SetMusicMenu();
        SceneManager.UnloadScene("Game");
        ScenesManager.scenesMgInstance.StartCoroutine("SetStartMenu");
    }
    /// <summary>
    /// Set the choose mode menu, so the player will choose between the touch or gyroscope mode, this is saved as preference 
    /// </summary>
    public void setChooseMode()
    {
        //gamePart = GameParts.chooseMode;
        canvasStartMenu.SetActive(false);
        canvasChooseMode.SetActive(true);
        canvasSettings.SetActive(false);
        //playedFirst = true;
    }
    /// <summary>
    /// Pause the game match and calls the music manager to play the menu music
    /// </summary>
    public void PauseMenu()
    {
        SoundsAndMusicMng.soundsIntance.SetMusicMenu();
        Time.timeScale = 0f;
    }
    /// <summary>
    ///  Continue the game match and call the music manager to play the game music
    /// </summary>
    void ContinueGame()
    {
        Time.timeScale = 1f;
    }
    /// <summary>
    /// Call the game manager to reset the game 
    /// </summary>
    public void TryAgainBtn()
    {
        Time.timeScale = 1f;
        GM.gmInstance.SetGameIntro();
        SoundsAndMusicMng.soundsIntance.StartCoroutine("StartMusicIntro");
        SceneManager.UnloadScene("Game");
        ScenesManager.scenesMgInstance.StartCoroutine("TryAgain");
    }
    /// <summary>
    /// Sets the gyroscope mode and start the game
    /// </summary> 
    public void SetGyro()
    {
        GM.isGyro = true;
        GM.gmInstance.SetGameIntro();
        PlayerScript.playerInstance.SetGyro();
    }
    /// <summary>
    ///  Sets the touch mode and start the game
    /// </summary>
    public void SetTouch()
    {
        GM.isGyro = false;
        GM.gmInstance.SetGameIntro();
        PlayerScript.playerInstance.SetTouch();
    }
    /// <summary>
    /// Set the slider value 
    /// </summary>
    /// <param name="value"></param>
    public void UpdateSlider(float value)
    {
        //Game Analytics values of slider 
        if (slider.value < 30)
        {
            GM.gmInstance.sliderLess30 += Time.deltaTime;
        }
        else if (slider.value > 40 && slider.value < 70)
        {
            GM.gmInstance.sliderBtw40n70 += Time.deltaTime;
        }
        else if (slider.value > 100)
        {
            GM.gmInstance.sliderOver100 += Time.deltaTime;
        }
    }
    /// <summary>
    /// Set the game over menu on
    /// </summary>
    public void GameOverMenu()
    {
        canvasGamveOver.SetActive(true);
        canvasHud.SetActive(false);
    }
    /// <summary>
    /// Update the score and lifes value
    /// </summary>
    /// <param name="valuescore"></param>
    /// <param name="valuelife"></param>
    public void SetScoreLife(int valuescore, float valuelife) //SET SCORE
    {
        scoreText.text = valuescore.ToString("000");
        UpdateSlider(valuelife);

    }
    //void OnApplicationPause(bool pauseStatus)
    //{
    //    PauseMenu();
    //    canvasPause.SetActive(true);
    //}
    /// <summary>
    /// Write the number in coins count in the HUD
    /// </summary>
    public void WriteCoins(int num)
    {
        coinText.text = num.ToString("000");
    }

}
