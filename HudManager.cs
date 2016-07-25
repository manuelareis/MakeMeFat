using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HudManager : MonoBehaviour
{
    //class responsable for Buttons and Hud

    [HideInInspector]
    public static HudManager hudInstance;
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
    Slider slider;
    [SerializeField]
    float sliderSize;

    void Start()
    {
        // btnPause.onClick.AddListener(() => { PauseMenu(); }); // if needed to accept any arguments 
        hudInstance = this;

        btnPause.onClick.AddListener(PauseMenu);
        btnContinue.onClick.AddListener(ContinueGame);
        btnTryAgain.onClick.AddListener(TryAgainBtn);
        btnTryAgainGO.onClick.AddListener(TryAgainBtn);
    }
    // Update is called once per frame
    void PauseMenu()
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
        SceneManager.LoadScene("Game");
    }

    public void SetGyro()
    {
        GM.gmInstance.setScene1(true);
    }

    public void SetTouch()
    {
        GM.gmInstance.setScene1(false);
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
