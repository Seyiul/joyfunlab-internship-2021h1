﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
enum settingBtn
{
    monsterHp = 0,
    maxHp = 1,
    startHp = 2,
    time = 3,
    back = 4
}

public class SettingUI : MonoBehaviour
{
    public GameObject monsterHpBtn;
    public GameObject maxHpBtn;
    public GameObject startHpBtn;
    public GameObject timeBtn;
    public GameObject backBtn;
    GameCanvas gameCanvas;
    public Text monsterHpText;
    public Text maxHpText;
    public Text startHpText;
    public Text timeText;
    settingBtn curBtn;
    public Sprite buttonSelected;
    public Sprite buttonUnselected;
    // Start is called before the first frame update
    void Start()
    {
        curBtn = settingBtn.monsterHp;
        Selected();
        gameCanvas = GameObject.Find("GameCanvas").GetComponent<GameCanvas>();
    }
    // Update is called once per frame
    void Update()
    {
    }
    void Selected()
    {
        Unselected();
        switch (curBtn)
        {
            case settingBtn.monsterHp:
                monsterHpBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
            case settingBtn.maxHp:
                maxHpBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
            case settingBtn.startHp:
                startHpBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
            case settingBtn.time:
                timeBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
            case settingBtn.back:
                backBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
        }

    }
    void Unselected()
    {
        monsterHpBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        maxHpBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        startHpBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        timeBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        backBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
    }
    public void MenuHandle()
    {
        Debug.Log(GameManager.instance.GetGameState());
        if (Input.GetKeyDown(KeyCode.UpArrow) || Floor.isUp)
        {
            Floor.isUp = false;
            if (curBtn > settingBtn.monsterHp)
                curBtn--;
            else if (curBtn == settingBtn.monsterHp)
                curBtn = settingBtn.back;
            Selected();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Floor.isDown)
        {
            Floor.isDown = false;
            if (curBtn < settingBtn.back)
                curBtn++;
            else if (curBtn == settingBtn.back)
                curBtn = settingBtn.monsterHp;
            Selected();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)||Floor.isLeft)
        {
            Floor.isLeft = false;
            if (curBtn == settingBtn.monsterHp)
            {
                //몬스터 체력 하향 코드,텍스트 변환 코드
            }
            else if (curBtn == settingBtn.maxHp)
            {
                if (ConstInfo.maxHp > 50 && ConstInfo.maxHp > ConstInfo.startHp)
                {
                    ConstInfo.maxHp -= 10;
                    maxHpText.text = ConstInfo.maxHp.ToString();
                }
            }
            else if (curBtn == settingBtn.startHp)
            {
                if (ConstInfo.startHp > 10)
                {
                    ConstInfo.startHp -= 10;
                    startHpText.text = ConstInfo.startHp.ToString();
                }
            }
            else if (curBtn == settingBtn.time)
            {
                if (ConstInfo.time > 10)
                {
                    ConstInfo.time -= 10;
                    timeText.text = ConstInfo.time.ToString();
                }
            }
            Player.instance.InitialSetting();
            gameCanvas.DisplayTime();
            gameCanvas.DisplayHp();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)||Floor.isRight)
        {
            Floor.isRight = false;
            if (curBtn == settingBtn.monsterHp)
            {
                //몬스터 체력 상향 코드,텍스트 변환 코드
            }
            else if (curBtn == settingBtn.maxHp)
            {
                if (ConstInfo.maxHp < 150)
                {
                    ConstInfo.maxHp += 10;
                    maxHpText.text = ConstInfo.maxHp.ToString();
                }
            }
            else if (curBtn == settingBtn.startHp)
            {
                if (ConstInfo.startHp < 150 && ConstInfo.maxHp > ConstInfo.startHp)
                {
                    ConstInfo.startHp += 10;
                    startHpText.text = ConstInfo.startHp.ToString();
                }
            }
            else if (curBtn == settingBtn.time)
            {
                if (ConstInfo.time < 120)
                {
                    ConstInfo.time += 10;
                    timeText.text = ConstInfo.time.ToString();
                }
            }
            Player.instance.InitialSetting();
            gameCanvas.DisplayHp();
            gameCanvas.DisplayTime();
        }
        else if(Input.GetKeyDown(KeyCode.Return)||Floor.isEnter)
        {
            Floor.isEnter =false ;
            GameManager.instance.SetGameState(GameState.Menu);
            GameManager.instance.SetStateChanged(true);
        }
    }
}
