﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum menuBtn
{
    start = 0,
    setting = 1,
    ranking = 2,
    quit = 3
}

public class MenuUI : MonoBehaviour
{
    public GameObject startBtn;
    public GameObject settingBtn;
    public GameObject rankingBtn;
    public GameObject quitBtn;
    menuBtn curBtn;
    public Sprite buttonSelected;
    public Sprite buttonUnselected;
    // 초기엔 현재 선택된 버튼을 게임 시작 버튼(첫 번째)으로 설정
    void Start()
    {
        curBtn = menuBtn.start;
        Selected();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // 현재 버튼의 위치에 따라 버튼 UI를 하이라이트
    void Selected()
    {
        Unselected();
        switch (curBtn)
        {
            case menuBtn.start:
                startBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
            case menuBtn.setting:
                settingBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
            case menuBtn.ranking:
                rankingBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
            case menuBtn.quit:
                quitBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
        }

    }

    // 모든 버튼 UI를 언하이라이트
    void Unselected()
    {
        startBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        settingBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        rankingBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        quitBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
    }

    // 메뉴창에서의 입력 이벤트 핸들링
    public void MenuHandle()
    {
        // 윗방향키 입력시
        if (Input.GetKeyDown(KeyCode.UpArrow)||Floor.isUp)
        {
            Floor.isUp = false;
            // 현재 버튼이 게임 시작이 아니면 위로 한칸씩
            if (curBtn > menuBtn.start)
                curBtn--;
            // 현재 버튼이 게임 시작이면 게임 종료로
            else
                curBtn = menuBtn.quit;
            // 변경한 현재 버튼에 하이라이트
            Selected();

        }

        // 아랫방향키 입력시
        else if (Input.GetKeyDown(KeyCode.DownArrow)||Floor.isDown)
        {
            Floor.isDown = false;
            // 현재 버튼이 게임 종료가 아니면 아래로 한칸씩
            if (curBtn < menuBtn.quit)
                curBtn++;
            // 현재 버튼이 게임 종료면 게임 시작으로
            else if (curBtn == menuBtn.quit)
                curBtn = menuBtn.start;
            // 변경한 현재 버튼에 하이라이트
            Selected();
        }

        // 엔터키 입력시
        else if (Input.GetKeyDown(KeyCode.Return)||Floor.isEnter)
        {
            Floor.isEnter = false;
            // 현재 버튼이 게임 시작이면
            if (curBtn == menuBtn.start)
            {
                // 게임 상태로 변경 
                GameManager.instance.SetGameState(GameState.Game);

                // 타일을 초기화(첫 타일은 아무것도 없고 두번째 타일부터 장애물이 나오도록)
                Player.instance.InitialTile();
            }
            else if (curBtn == menuBtn.setting)
                GameManager.instance.SetGameState(GameState.Setting);
            else if (curBtn == menuBtn.ranking)
                GameManager.instance.SetGameState(GameState.Rank);
            else if (curBtn == menuBtn.quit)
                GameManager.instance.SetGameState(GameState.Quit);
            GameManager.instance.SetStateChanged(true);
        }
    }
}
