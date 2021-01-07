using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RankingUI : MonoBehaviour
{
    // 인스턴스 변수 선언
    public static RankingUI instance;

    // 랭킹 텍스트 변수
    public Text rankingText;

    void Awake() { instance = this; }

    // 시작시 안보이도록 설정
    void Start() { transform.gameObject.SetActive(false); }

    // 보이도록 설정
    public void Show()
    {
        GameManager.instance.SetGameState(GameState.Ranking);
        UpdateRanking();
        transform.gameObject.SetActive(true);
        GetComponent<Animation>().Play("ShowGuide");
    }



    // 취소 버튼을 누른 경우
    public void HandleCancel()
    {
        transform.gameObject.SetActive(false);
        MenuUI.instance.Show();
    }

    // 랭킹을 업데이트
    public void UpdateRanking()
    {
        rankingText.text = "";
        for (int i = 0; i < 5; i++)
            rankingText.text += (i + 1) + ".                          " + PlayerPrefs.GetInt(i.ToString("####0")) + "\n\n";
    }


}


