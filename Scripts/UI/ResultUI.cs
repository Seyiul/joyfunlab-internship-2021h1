using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResultUI : MonoBehaviour
{
    // 인스턴스 변수 선언
    public static ResultUI instance;

    // 결과 텍스트 변수
    public Text maxComboText;
    public Text playTimeText;
    public Text pointText;

    string myRankText;

    void Awake() { instance = this; }
    void Start() { transform.gameObject.SetActive(false); }

    // 보이도록 설정
    public void Show(int maxCombo, int point, float playTime, string myRank)
    {
        GameManager.instance.SetGameState(GameState.Result);
        SetText(maxCombo, point, playTime, myRank);
        transform.gameObject.SetActive(true);
        GetComponent<Animation>().Play("ShowGuide");
    }

    // 텍스트 설정
    public void SetText(int maxCombo, int point, float playTime, string myRank) {
        maxComboText.text = maxCombo.ToString() + " 회";
        playTimeText.text = (Mathf.Round(playTime * 100) / 100) + " 초";
        if (Setting.GetCurrentTimeState() == TimeState.Infinite)
            playTimeText.text = "무제한";
        pointText.text = point.ToString();
        myRankText = myRank;
    }



    // 다음페이지 버튼을 누른 경우
    public void HandleNextPage()
    {
        transform.gameObject.SetActive(false);
        MyRankUI.instance.Show(pointText.text, myRankText);
    }
}