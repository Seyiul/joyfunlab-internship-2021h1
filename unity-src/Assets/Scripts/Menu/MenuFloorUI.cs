using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MenuFloorUI : MonoBehaviour
{

    // 바닥 UI 타일
    public GameObject centerButton;

    public GameObject upArrowButton;
    public GameObject downArrowButton;

    public GameObject leftArrowButton;
    public GameObject rightArrowButton;

    public GameObject confirmButton;
    public GameObject cancelButton;

    // 발위치 원 변수
    public GameObject leftFootPrint;
    public GameObject rightFootPrint;

    // Lerp 변수
    public float footPrintLerpT;

    // 버튼 타이머 변수 - 0: up, 1: down, 2: left, 3: right, 4: confirm, 5: cancel
    public float[] uiTimer;
    public float buttonDelayTimer;


    void Start()
    {
        GameManager.instance.SetGameState(GameState.Menu);
        InitialValues();
    }

    void InitialValues()
    {
        uiTimer = new float[6] { 0, 0, 0, 0, 0, 0 };
        buttonDelayTimer = 0;
        footPrintLerpT = 0;
    }

    void Update()
    {
        HandleButtonActive();
        HandleFloorUI();
        HandlePlayerPref();
    }

    // 바닥 UI 버튼 보여주기, 감추기
    void HandleButtonActive()
    {
        if (GameManager.instance.GetGameState() == GameState.Menu)
        {
            upArrowButton.SetActive(true);
            downArrowButton.SetActive(true);
            leftArrowButton.SetActive(false);
            rightArrowButton.SetActive(false);
            confirmButton.SetActive(true);
            cancelButton.SetActive(false);
        }
        else if (GameManager.instance.GetGameState() == GameState.Ranking)
        {
            upArrowButton.SetActive(false);
            downArrowButton.SetActive(false);
            leftArrowButton.SetActive(false);
            rightArrowButton.SetActive(false);
            confirmButton.SetActive(false);
            cancelButton.SetActive(true);
        }
        else if (GameManager.instance.GetGameState() == GameState.Setting)
        {
            upArrowButton.SetActive(true);
            downArrowButton.SetActive(true);
            leftArrowButton.SetActive(true);
            rightArrowButton.SetActive(true);
            confirmButton.SetActive(false);
            cancelButton.SetActive(true);
        }
    }

    void HandleFloorUI()
    {
        if (GameManager.instance.GetKinectState())
            HandleFootPrint();
        else
        {
            leftFootPrint.transform.localScale = Vector3.zero;
            rightFootPrint.transform.localScale = Vector3.zero;
        }
        HandleMenuButtons();
    }

    // 발 위치 원 설정
    void HandleFootPrint()
    {
        HandleFootPrintPosition();
        HandleFootPrintSize();
    }

    // 발 위치 원 좌표 변경
    void HandleFootPrintPosition()
    {
        leftFootPrint.transform.localPosition = Vector3.Lerp(leftFootPrint.transform.localPosition,
            new Vector3(Avatar.userPositionLeftFoot.x, Avatar.userPositionLeftFoot.z, 0), footPrintLerpT);
        rightFootPrint.transform.localPosition = Vector3.Lerp(rightFootPrint.transform.localPosition,
            new Vector3(Avatar.userPositionRightFoot.x, Avatar.userPositionRightFoot.z, 0), footPrintLerpT);

        footPrintLerpT += ConstInfo.footPrintSpeed * Time.deltaTime;
        if (footPrintLerpT > 1)
            footPrintLerpT = 0;
    }

    // 발 위치 원 크기 변경
    void HandleFootPrintSize()
    {
        float newLeftFootPrintSize = Avatar.HandleFootprintSize(Avatar.userPositionLeftFoot.y);
        float newRightFootPrintSize = Avatar.HandleFootprintSize(Avatar.userPositionRightFoot.y);
        leftFootPrint.transform.localScale = new Vector3(newLeftFootPrintSize, newLeftFootPrintSize, newLeftFootPrintSize);
        rightFootPrint.transform.localScale = new Vector3(newRightFootPrintSize, newRightFootPrintSize, newRightFootPrintSize);
    }



    // 메뉴 바닥타일과 유저 간의 상호작용
    void HandleMenuButtons()
    {
        HandleCenterButton();

        if (buttonDelayTimer <= 0)
        {
            if ((Avatar.OneFootOverlaps(leftFootPrint, rightFootPrint, upArrowButton) || Input.GetKey(KeyCode.UpArrow)) && upArrowButton.activeSelf)
                HandleUpArrowButton();
            else
                InitialButtonTexture(upArrowButton, FloorTexture.UpArrowButton, 0);

            if ((Avatar.OneFootOverlaps(leftFootPrint, rightFootPrint, downArrowButton) || Input.GetKey(KeyCode.DownArrow)) && downArrowButton.activeSelf)
                HandleDownArrowButton();
            else
                InitialButtonTexture(downArrowButton, FloorTexture.DownArrowButton, 1);

            if ((Avatar.OneFootOverlaps(leftFootPrint, rightFootPrint, leftArrowButton) || Input.GetKey(KeyCode.LeftArrow)) && leftArrowButton.activeSelf)
                HandleLeftArrowButton();
            else
                InitialButtonTexture(leftArrowButton, FloorTexture.LeftArrowButton, 2);

            if ((Avatar.OneFootOverlaps(leftFootPrint, rightFootPrint, rightArrowButton) || Input.GetKey(KeyCode.RightArrow)) && rightArrowButton.activeSelf)
                HandleRightArrowButton();
            else
                InitialButtonTexture(rightArrowButton, FloorTexture.RightArrowButton, 3);

            if ((Avatar.OneFootOverlaps(leftFootPrint, rightFootPrint, confirmButton) || Input.GetKey(KeyCode.Return)) && confirmButton.activeSelf)
                HandleConfirmButton();
            else
                InitialButtonTexture(confirmButton, FloorTexture.RightButton, 4);

            if ((Avatar.OneFootOverlaps(leftFootPrint, rightFootPrint, cancelButton) || Input.GetKey(KeyCode.Backspace)) && cancelButton.activeSelf)
                HandleCancelButton();
            else
                InitialButtonTexture(cancelButton, FloorTexture.LeftButton, 5);
        }
        else
            buttonDelayTimer -= Time.deltaTime;
    }

    // 중앙 발판 텍스처 설정
    void HandleCenterButton()
    {
        if (Avatar.GetUserValid())
            if (Avatar.VectorInside(Avatar.userPositionLeftFoot, centerButton)
                && Avatar.VectorInside(Avatar.userPositionRightFoot, centerButton))
                FloorTexture.setButtonTexture(centerButton, FloorTexture.PositionButtonBlue);
            else
                FloorTexture.setButtonTexture(centerButton, FloorTexture.PositionButton);
        else
            FloorTexture.setButtonTexture(centerButton, FloorTexture.PositionDisabled);
    }



    // 버튼 누르는 효과 초기화
    void InitialButtonTexture(GameObject button, Texture texture, int timerIndex)
    {
        FloorTexture.setButtonTexture(button, texture);
        uiTimer[timerIndex] = 0;
        FloorTexture.ProgressDelayTexture(button, 0);
        FloorTexture.MoveAllChildTexture(button, false);
    }

    // 버튼 누르는 효과 설정
    void HandleButtonTexture(GameObject button, Texture texture, int timerIndex)
    {
        FloorTexture.setButtonTexture(button, texture);
        uiTimer[timerIndex] += Time.deltaTime;
        FloorTexture.MoveAllChildTexture(button, true);
        FloorTexture.ProgressDelayTexture(button, uiTimer[timerIndex] / ConstInfo.buttonPushTime);
    }



    // 위 방향을 누른 경우
    void HandleUpArrowButton()
    {
        HandleButtonTexture(upArrowButton, FloorTexture.UpArrowButtonPress, 0);
        if (uiTimer[0] > ConstInfo.buttonPushTime)
        {
            HandleUpArrow();
            InitialButtonTexture(upArrowButton, FloorTexture.UpArrowButton, 0);
            buttonDelayTimer = ConstInfo.buttonDelayTime;
        }
    }

    // 아래 방향을 누른 경우
    void HandleDownArrowButton()
    {
        HandleButtonTexture(downArrowButton, FloorTexture.DownArrowButtonPress, 1);
        if (uiTimer[1] > ConstInfo.buttonPushTime)
        {
            HandleDownArrow();
            InitialButtonTexture(downArrowButton, FloorTexture.DownArrowButton, 1);
            buttonDelayTimer = ConstInfo.buttonDelayTime;
        }
    }

    // 왼쪽 방향을 누른 경우
    void HandleLeftArrowButton()
    {
        HandleButtonTexture(leftArrowButton, FloorTexture.LeftArrowButtonPress, 2);
        if (uiTimer[2] > ConstInfo.buttonPushTime)
        {
            SettingUI.instance.HandleLeft();
            InitialButtonTexture(leftArrowButton, FloorTexture.LeftArrowButton, 2);
            buttonDelayTimer = ConstInfo.buttonDelayTime;
        }
    }

    // 오른쪽 방향을 누른 경우
    void HandleRightArrowButton()
    {
        HandleButtonTexture(rightArrowButton, FloorTexture.RightArrowButtonPress, 3);
        if (uiTimer[3] > ConstInfo.buttonPushTime)
        {
            SettingUI.instance.HandleRight();
            InitialButtonTexture(rightArrowButton, FloorTexture.RightArrowButton, 3);
            buttonDelayTimer = ConstInfo.buttonDelayTime;
        }
    }

    // 확인을 누른 경우
    void HandleConfirmButton()
    {
        HandleButtonTexture(confirmButton, FloorTexture.RightButtonPress, 4);
        if (uiTimer[4] > ConstInfo.buttonPushTime)
        {
            MenuUI.instance.HandleConfirm();
            InitialButtonTexture(confirmButton, FloorTexture.RightButton, 4);
            buttonDelayTimer = ConstInfo.buttonDelayTime;
        }
    }

    // 취소를 누른 경우
    void HandleCancelButton()
    {
        HandleButtonTexture(cancelButton, FloorTexture.LeftButtonPress, 5);
        if (uiTimer[5] > ConstInfo.buttonPushTime)
        {
            HandleCancel();
            InitialButtonTexture(cancelButton, FloorTexture.LeftButton, 5);
            buttonDelayTimer = ConstInfo.buttonDelayTime;
        }
    }



    // 게임 상태에 따른 위 방향키 동작구분
    void HandleUpArrow()
    {
        if (GameManager.instance.GetGameState() == GameState.Menu)
            MenuUI.instance.HandleUp();
        else if (GameManager.instance.GetGameState() == GameState.Setting)
            SettingUI.instance.HandleUp();
    }

    // 게임 상태에 따른 아래 방향키 동작구분
    void HandleDownArrow()
    {
        if (GameManager.instance.GetGameState() == GameState.Menu)
            MenuUI.instance.HandleDown();
        else if (GameManager.instance.GetGameState() == GameState.Setting)
            SettingUI.instance.HandleDown();
    }

    // 게임 상태에 따른 취소 동작구분
    void HandleCancel()
    {
        if (GameManager.instance.GetGameState() == GameState.Ranking)
            RankingUI.instance.HandleCancel();
        else if (GameManager.instance.GetGameState() == GameState.Setting)
            SettingUI.instance.HandleCancel();
    }

    void HandlePlayerPref() { 
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.F8))
            PlayerPrefs.DeleteAll();
    }
}