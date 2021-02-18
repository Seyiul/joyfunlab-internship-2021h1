using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MyRankUI : MonoBehaviour
{
    private float score;
    private float[] rankScore = new float[5];
    public Text scoreBox;
    private float changeScore;
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }
    // 랭크 UI에서의 입력 핸들
    public void MenuHandle()
    {
        score=ResultUI.finalScore;
        rankScore = RankDB.RankReader(GameManager.rankpath);

        for (int i = 0; i < 5; i++)
        {
            if (rankScore[i] < score)
            {
                changeScore = rankScore[i];
                rankScore[i] = score;
                score = changeScore;
            }
        }
        scoreBox.text = "1. " + rankScore[0] + "\n" +
                        "2. " + rankScore[1] + "\n" +
                        "3. " + rankScore[2] + "\n" +
                        "4. " + rankScore[3] + "\n" +
                        "5. " + rankScore[4]; 
        RankDB.RankWriter(GameManager.rankpath, rankScore);

        // 엔터 키 누르면(뒤로가기 버튼 하나밖에 없음)
        if (Input.GetKeyDown(KeyCode.Return)||Floor.isEnter)
        {
            Floor.isEnter = false;
            // 메뉴 상태로 변경
            GameManager.instance.SetGameState(GameState.Menu);
            GameManager.instance.SetStateChanged(true);
        }
    }
}
