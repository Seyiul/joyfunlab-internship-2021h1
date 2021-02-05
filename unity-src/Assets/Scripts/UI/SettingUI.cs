using System.Collections;
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
    // 현재 버튼을 몬스터 체력 설정(맨위)버튼으로 설정, 하이라이트
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
    // 현재 버튼을 하이라이트
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
    // 모든 버튼을 언하이라이트
    void Unselected()
    {
        monsterHpBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        maxHpBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        startHpBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        timeBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        backBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
    }
    // 세팅 변수 인풋 핸들링
    public void MenuHandle()
    {
        // 윗방향키 입력시
        if (Input.GetKeyDown(KeyCode.UpArrow) || Floor.isUp)
        {
            Floor.isUp = false;
            // 현재 버튼이 맨위가 아니면 한칸씩 위로
            if (curBtn > settingBtn.monsterHp)
                curBtn--;
            // 현재 버튼이 맨위면 맨아래 버튼으로
            else if (curBtn == settingBtn.monsterHp)
                curBtn = settingBtn.back;
            Selected();
        }
        // 아랫방향키 입력시
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Floor.isDown)
        {
            Floor.isDown = false;
            // 현재 버튼이 맨아래가 아니면 한칸씩 밑으로
            if (curBtn < settingBtn.back)
                curBtn++;
            // 현재 버튼이 맨아래면 맨위로
            else if (curBtn == settingBtn.back)
                curBtn = settingBtn.monsterHp;
            Selected();
        }
        // 왼방향키 입력시
        else if (Input.GetKeyDown(KeyCode.LeftArrow)||Floor.isLeft)
        {
            Floor.isLeft = false;
            //ConstInfo에 있는 설정값들을 조절
            if (curBtn == settingBtn.monsterHp)
            {
                //몬스터 체력 하향 코드,텍스트 변환 코드
            }
            else if (curBtn == settingBtn.maxHp)
            {
                // 최대 체력은 50 미만, 현재 체력 미만으로 설정 불가
                if (ConstInfo.maxHp > 50 && ConstInfo.maxHp > ConstInfo.startHp)
                {
                    ConstInfo.maxHp -= 10;
                    maxHpText.text = ConstInfo.maxHp.ToString();
                }
            }
            else if (curBtn == settingBtn.startHp)
            {
                // 현재 체력은 10 미만으로 설정 불가
                if (ConstInfo.startHp > 10)
                {
                    ConstInfo.startHp -= 10;
                    startHpText.text = ConstInfo.startHp.ToString();
                }
            }
            else if (curBtn == settingBtn.time)
            {
                // 시간은 10 미만으로 설정 불가
                if (ConstInfo.time > 10)
                {
                    ConstInfo.time -= 10;
                    timeText.text = ConstInfo.time.ToString();
                }
            }
            // ConstInfo에 세팅된 변수들로 Player의 변수들을 초기화하고 화면 UI에도 나타냄
            Player.instance.InitialSetting();
            gameCanvas.DisplayTime();
            gameCanvas.DisplayHp();
        }
        // 오른방향키 입력시
        else if (Input.GetKeyDown(KeyCode.RightArrow)||Floor.isRight)
        {
            Floor.isRight = false;
            // ConstInfo에 있는 설정값들을 조정
            if (curBtn == settingBtn.monsterHp)
            {
                //몬스터 체력 상향 코드,텍스트 변환 코드
            }
            else if (curBtn == settingBtn.maxHp)
            {
                // 최대체력은 150 초과로 설정 불가
                if (ConstInfo.maxHp < 150)
                {
                    ConstInfo.maxHp += 10;
                    maxHpText.text = ConstInfo.maxHp.ToString();
                }
            }
            else if (curBtn == settingBtn.startHp)
            {
                // 현재 체력은 150 초과, 최대 체력 초과로 설정 불가
                if (ConstInfo.startHp < 150 && ConstInfo.maxHp > ConstInfo.startHp)
                {
                    ConstInfo.startHp += 10;
                    startHpText.text = ConstInfo.startHp.ToString();
                }
            }
            else if (curBtn == settingBtn.time)
            {
                // 시간은 120 초과로 설정 불가
                if (ConstInfo.time < 120)
                {
                    ConstInfo.time += 10;
                    timeText.text = ConstInfo.time.ToString();
                }
            }
            // ConstInfo에 세팅된 변수들로 Player의 변수들을 초기화하고 화면 UI에도 나타냄
            Player.instance.InitialSetting();
            gameCanvas.DisplayHp();
            gameCanvas.DisplayTime();
        }
        // 엔터키 입력시
        else if(Input.GetKeyDown(KeyCode.Return)||Floor.isEnter)
        {
            if(curBtn == settingBtn.back)
            {
                Floor.isEnter = false;
                // 게임을 메뉴 상태로
                GameManager.instance.SetGameState(GameState.Menu);
                GameManager.instance.SetStateChanged(true);
            }
        }
    }
}
