using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 환경설정 상태 (선택 하이라이트, 0: 애니메이션, 1: 체력제한, 2: 시간제한)
public enum SettingChoice: int
{
    Animation = 0,
    Hp = 1,
    Time = 2
}

public class SettingUI : MonoBehaviour
{
    // 인스턴스 및 현재 선택된 항목 변수 선언
    public static SettingUI instance;
    private SettingChoice currentSettingChoice;

    // 기본, 하이라이트 스프라이트, 텍스처
    public Sprite buttonSelected;
    public Sprite buttonUnselected;

    public Texture arrowLSelected;
    public Texture arrowLUnselected;

    public Texture arrowRSelected;
    public Texture arrowRUnselected;

    // 메뉴 버튼 변수
    public GameObject animationButton;
    public GameObject hpButton;
    public GameObject timeButton;

    void Awake() { instance = this; }

    void Start()
    {
        currentSettingChoice = SettingChoice.Animation;
        transform.gameObject.SetActive(false);
    }

    // 환경설정 UI 보여주기
    public void Show()
    {
        GameManager.instance.SetGameState(GameState.Setting);
        transform.gameObject.SetActive(true);
        GetComponent<Animation>().Play("ShowGuide");
    }

    void Update()
    {
        UnselectButtons();
        HandleSettingChoice();
        DisplaySettingText();
    }


    // 모든 버튼을 선택 해제
    void UnselectButtons()
    {
        Unselect(animationButton);
        Unselect(hpButton);
        Unselect(timeButton);
    }

    void Unselect(GameObject obj) {
        obj.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        obj.transform.GetChild(2).GetComponent<UnityEngine.UI.RawImage>().texture = arrowLUnselected;
        obj.transform.GetChild(3).GetComponent<UnityEngine.UI.RawImage>().texture = arrowRUnselected;
    }

    void Select(GameObject obj) {
        obj.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
        obj.transform.GetChild(2).GetComponent<UnityEngine.UI.RawImage>().texture = arrowLSelected;
        obj.transform.GetChild(3).GetComponent<UnityEngine.UI.RawImage>().texture = arrowRSelected;
    }

    // 환경설정 상태에 따라 버튼 선택 (색상 변경) 
    void HandleSettingChoice()
    {
        switch (currentSettingChoice)
        {
            case SettingChoice.Animation:
                Select(animationButton);
                break;
            case SettingChoice.Hp:
                Select(hpButton);
                break;
            case SettingChoice.Time:
                Select(timeButton);
                break;
        }
    }



    // 위 버튼을 누른 경우
    public void HandleUp() { currentSettingChoice = currentSettingChoice.Previous(); }

    // 아래 버튼을 누른 경우
    public void HandleDown() { currentSettingChoice = currentSettingChoice.Next(); }

    // 왼쪽 버튼을 누른 경우 
    public void HandleLeft()
    {
        switch (currentSettingChoice)
        {
            case SettingChoice.Animation:
                    Setting.SetCurrentAnimationState(Setting.GetCurrentAnimationState().Previous());
                break;
            case SettingChoice.Hp:
                Setting.SetCurrentHpState(Setting.GetCurrentHpState().Previous());
                break;
            case SettingChoice.Time:
                    Setting.SetCurrentTimeState(Setting.GetCurrentTimeState().Previous());
                break;
        }
    }

    // 오른쪽 버튼을 누른 경우
    public void HandleRight()
    {
        switch (currentSettingChoice)
        {
            case SettingChoice.Animation:
                    Setting.SetCurrentAnimationState(Setting.GetCurrentAnimationState().Next());
                break;
            case SettingChoice.Hp:
                    Setting.SetCurrentHpState(Setting.GetCurrentHpState().Next());
                break;
            case SettingChoice.Time:
                    Setting.SetCurrentTimeState(Setting.GetCurrentTimeState().Next());
                break;

        }
    }

    // 취소 버튼을 누른 경우
    public void HandleCancel()
    {
        transform.gameObject.SetActive(false);
        MenuUI.instance.Show();
    }



    // 설정된 환경설정 변수를 버튼에 출력
    public void DisplaySettingText()
    {
        animationButton.GetComponentInChildren<Text>().text = Setting.GetCurrentAnimationState().ToString();
        hpButton.GetComponentInChildren<Text>().text = Setting.GetCurrentHpState().ToString();
        timeButton.GetComponentInChildren<Text>().text = Setting.GetCurrentTimeState().ToString();
    }
}
