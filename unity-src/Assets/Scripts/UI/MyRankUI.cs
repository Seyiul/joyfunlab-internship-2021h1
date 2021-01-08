using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyRankUI : MonoBehaviour
{
    // 인스턴스 변수 선언
    public static MyRankUI instance;

    // 내 등수 표시 관련 변수
    public Text myRankText;
    public Text rankingText;
    public Text pointText;

    void Awake() { instance = this; }
    void Start() { transform.gameObject.SetActive(false); }

    // 보이도록 설정
    public void Show(string point, string myRank)
    {
        GameManager.instance.SetGameState(GameState.MyRank);
        UpdateRanking();
        SetText(point, myRank);
        transform.gameObject.SetActive(true);
        GetComponent<Animation>().Play("ShowGuide");
    }

    public void SetText(string point, string myRank) 
    {
        pointText.text = point;
        myRankText.text = myRank;
    }



    // 재시작 버튼을 누른 경우
    public void HandleRetry() { SceneManager.LoadScene("Game"); }

    // 메뉴로 버튼을 누른 경우
    public void HandleToMenu() { SceneManager.LoadScene("Menu"); }



    // 랭킹을 업데이트
    public void UpdateRanking()
    {
        rankingText.text = "";
        for (int i = 0; i < 5; i++)
            rankingText.text += (i + 1) + ".                        " + PlayerPrefs.GetInt(i.ToString("####0")) + "\n\n";
    }
}