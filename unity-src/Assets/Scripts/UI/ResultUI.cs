﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    public Text maxComboText;
    public Text playtimeText;
    public Text pointText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    // 결과창 출력
    public void ShowResult()
    {
        // 최대 콤보 출력
        maxComboText.text = Player.instance.maxCombo.ToString() + " 회";
        // 플레이 시간 출력 (= 총 시간 - 남은 시간)
        float playtime = Player.instance.playtime - Player.instance.time;
        playtimeText.text = (Mathf.Floor(playtime * 10) * 0.1f).ToString() + " 초";
        // 콤보 계수 = 1 + 콤보/100(소수점 첫번째 까지만) 
        float comboPoint = 1 + (float)((int)Player.instance.maxCombo / 10) / 10;
        // 점수 = 최대 콤보 * 콤보 계수 + 플레이 시간
        pointText.text = (Mathf.Round(Player.instance.maxCombo * comboPoint + playtime)).ToString() + " 점";
        // 엔터키 입력시
        if (Input.GetKeyDown(KeyCode.Return) || Floor.isRight == true)
        {
            // 게임을 메뉴 상태로
            GameManager.instance.SetGameState(GameState.Menu);
            GameManager.instance.SetStateChanged(true);
            Floor.isRight = false;
        }
    }
    public void ShowBattleResult()
    {
        maxComboText.text = PlayerPrefs.GetInt("maxCombo") + " 회";

        float playedTime = Mathf.Floor((PlayerPrefs.GetFloat("playtime") - PlayerPrefs.GetFloat("time")) * 10) * 0.1f;
        playtimeText.text = playedTime.ToString() + " 초";

        float comboPoint = 1 + (float)((int)PlayerPrefs.GetInt("maxCombo") / 10) / 10;

        pointText.text = (Mathf.Round(PlayerPrefs.GetInt("maxCombo") * comboPoint) + playedTime).ToString() + " 점";

        if (Input.GetKeyDown(KeyCode.Return) || Floor.isRight== true )
        {
            GameManager.instance.SetGameState(GameState.Game);
            PlayerPrefs.SetInt("afterBattle", 0);
            SceneManager.LoadScene("Game");
            Floor.isRight = false;
            
        }
    } 
}
