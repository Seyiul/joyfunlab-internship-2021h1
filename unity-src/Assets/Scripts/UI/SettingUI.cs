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
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (curBtn > settingBtn.monsterHp)
                curBtn--;
            else if (curBtn == settingBtn.monsterHp)
                curBtn = settingBtn.back;
            Selected();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (curBtn < settingBtn.back)
                curBtn++;
            else if (curBtn == settingBtn.back)
                curBtn = settingBtn.monsterHp;
            Selected();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (curBtn == settingBtn.monsterHp)
            {
                //몬스터 체력 하향 코드,텍스트 변환 코드
            }
            else if (curBtn == settingBtn.maxHp)
            {
                if (Player.instance.maxHp > 50)
                {
                    Player.instance.maxHp -= 10;
                    maxHpText.text = Player.instance.maxHp.ToString();
                }
            }
            else if (curBtn == settingBtn.startHp)
            {
                if (Player.instance.hp > 10)
                {
                    Player.instance.hp -= 10;
                    startHpText.text = Player.instance.hp.ToString();
                }
            }
            else if (curBtn == settingBtn.time)
            {
                if (Player.instance.time > 10)
                {
                    Player.instance.time -= 10;
                    timeText.text = Player.instance.time.ToString();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (curBtn == settingBtn.monsterHp)
            {
                //몬스터 체력 상향 코드,텍스트 변환 코드
            }
            else if (curBtn == settingBtn.maxHp)
            {
                if (Player.instance.maxHp < 150)
                {
                    Player.instance.maxHp += 10;
                    maxHpText.text = Player.instance.maxHp.ToString();
                }
            }
            else if (curBtn == settingBtn.startHp)
            {
                if (Player.instance.hp < 150)
                {
                    Player.instance.hp += 10;
                    startHpText.text = Player.instance.hp.ToString();
                }
            }
            else if (curBtn == settingBtn.time)
            {
                if (Player.instance.time < 120)
                {
                    Player.instance.time += 10;
                    timeText.text = Player.instance.time.ToString();
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.instance.SetGameState(GameState.Menu);
            GameManager.instance.SetStateChanged(true);
        }
    }
}
