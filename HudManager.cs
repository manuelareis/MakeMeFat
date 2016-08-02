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
    [SerializeField]
    GameObject _slider;
    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text coinText;

    void Awake()
    {
        // btnPause.onClick.AddListener(() => { PauseMenu(); }); // if needed to accept any arguments 
        hudInstance = this;
        DontDestroyOnLoad(transform.gameObject);
        btnPause.onClick.AddListener(PauseMenu);
        btnContinue.onClick.AddListener(ContinueGame);
        btnTryAgain.onClick.AddListener(TryAgainBtn);
        btnTryAgainGO.onClick.AddListener(TryAgainBtn);
        canvasStartMenu.SetActive(true);
        canvasSettings.SetActive(true);
        canvasHud.SetActive(false);
        canvasChooseMode.SetActive(false);
        canvasPause.SetActive(false);
        _slider.SetActive(false);

    }

    IEnumerator Start()
    {
        AsyncOperation async = Application.LoadLevelAdditiveAsync("Game");
        yield return async;
        Debug.Log("Loading complete");
    }
    public void SetScore(int num)
    {
        scoreText.text = (num.ToString("000"));
    }

    public void SetLife(float num)
    {
        slider.value += num;
    }

    public void StartSlider()
    {
        InvokeRepeating("LifeTime", 0.1f, 0.1f);
    }

    void LifeTime()
    {
        slider.value -= 0.1f;
    }

    public void StartGame()
    {
        GM.gmInstance.SetGameIntro();
        canvasHud.SetActive(true);
        _slider.SetActive(true);
    }

    public void setChooseMode()
    {
        // gamePart = GameParts.chooseMode;
        canvasStartMenu.SetActive(false);
        canvasChooseMode.SetActive(true);
        canvasSettings.SetActive(false);
        // playedFirst = true;
    }
    // Update is called once per frame
    public void PauseMenu()
    {
        Time.timeScale = 0f;
        print("PAUSE");
    }

    void ContinueGame()
    {
        Time.timeScale = 1f;
    }

    public void TryAgainBtn()
    {
        Time.timeScale = 1f;
        GM.gmInstance.ResetGame();
    }

    public void SetGyro()
    {
        GM.isGyro = true;
        GM.gmInstance.SetGameIntro();
        PlayerScript.playerInstance.SetGyro();
    }

    public void SetTouch()
    {
        GM.isGyro = false;
        GM.gmInstance.SetGameIntro();
        PlayerScript.playerInstance.SetTouch();
    }
    public void UpdateSlider(float value)
    {
        slider.value = value;

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
        if (slider.value == 0)
        {
            sliderSize = slider.GetComponent<RectTransform>().rect.width;
            sliderSize = sliderSize / (slider.maxValue - slider.minValue);
        }

        slider.fillRect.rotation = new Quaternion(0, 0, 0, 0);
        slider.fillRect.pivot = new Vector2(slider.fillRect.transform.parent.localPosition.x, slider.fillRect.pivot.y);

        if (slider.value > 0)
            slider.fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sliderSize * slider.value);

        else
        {
            slider.fillRect.Rotate(0, 0, 180);
            slider.fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, -1 * sliderSize * slider.value);
        }

        slider.fillRect.localPosition = new Vector3(0, 0, 0);

    }

}
