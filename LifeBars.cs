using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LifeBars : MonoBehaviour {
    [SerializeField] RectTransform yellowRect;
    [SerializeField] RectTransform redRect;
    [SerializeField][Range(-500,500)] float num = 60f;
    float fixedHeight = 36f;
    float maxWidth = 500f;
    Vector2 zeroBar;
	
    void Start(){
        // começa com ambas as barras zeradas
        zeroBar = new Vector2(0, fixedHeight);
        yellowRect.sizeDelta = zeroBar;
        redRect.sizeDelta = zeroBar;
    }
    void Update()
    {
        SetNewValue(num);
    }
    public void SetNewValue(float value) {
        //recebe de - 500 a 500 e tratado para entre 0 e 500 
        //necessario a vida do jogador
        // vida do jogador sempre ocmeça com ambas barras zeradas
        // quando jogador estiver ganhando pontos a barra amarela preenche e ele vai engordando e barra vermelha vazia
        // quando o jogador vai comendo coisas erradas a barra vermelha sobe e vai emagrecendo e barra amarela vazia
        float tempValueYellow = yellowRect.sizeDelta.x;
        float tempValueRed = redRect.sizeDelta.x;

        if (value > 0){
          // for(float i = tempValueYellow; i < value; i -= 0.1f ) { 
                yellowRect.sizeDelta = new Vector2(Mathf.Lerp(tempValueYellow, value, Time.smoothDeltaTime * 5f), fixedHeight );
                redRect.sizeDelta = new Vector2(Mathf.Lerp(tempValueRed, 0f, Time.smoothDeltaTime * 5f), fixedHeight);
                
        }
        else if (value < 0){
            //for (float i = 0; i < value; i += Time.smoothDeltaTime) {            
            redRect.sizeDelta = new Vector2(Mathf.Lerp(Mathf.Abs(tempValueRed), Mathf.Abs(value), Time.smoothDeltaTime * 5f), fixedHeight);
            yellowRect.sizeDelta = new Vector2(Mathf.Lerp(tempValueYellow, 0f, Time.smoothDeltaTime * 5f), fixedHeight);
            //yield return new WaitForEndOfFrame();
        }        
    }
}
