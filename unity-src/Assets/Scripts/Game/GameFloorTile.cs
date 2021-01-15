    using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;

public class GameFloorTile : MonoBehaviour
{
    // 바닥 UI 타일
    public GameObject leftFloorTile;
    public GameObject centerFloorTile;
    public GameObject rightFloorTile;

    public static GameObject centerButton;
    public GameObject newGameButtonPause;
    public GameObject newGameButtonResult;
    public GameObject toMenuButtonPause;
    public GameObject toMenuButtonResult;
    public GameObject nextPageButton;

    // 발위치 원 변수
    public GameObject leftFootPrint;
    public GameObject rightFootPrint;
    public float footPrintLerpT;

    // 유저 상태 변수
    public static bool isJumping;
    public static bool isPunching;
    //kick motion 추가
    public static bool isKicking;
    public static bool stepSide;
    public static float stepRecordTime;
    public static float decreaseSpeedTimer;
    public static List<float> steps;

    public static Vector3 lastPositionLeftFoot;
    public static Vector3 lastPositionRightFoot;
    public static Vector3 lastPositionHead;

    // 버튼 타이머 변수 - 0: newgame pause, 1: to menu pause, 2: next page, 3: new game result, 4: to menu result
    public float[] uiTimer;
    public Text timerText;
    public GameObject timerBox;

    public UIBarScript barHp;
    public GameObject barHpBox;

    void Start()
    {
        GameManager.instance.SetGameState(GameState.Game);
        InitialValues();
    }

    // 변수 초기화
    void InitialValues()
    {
        // 상태변수 초기화
        isJumping = false;
        isPunching = false;
        stepSide = false;

        // 걸음 관련 변수 초기화
        stepRecordTime = 0;
        decreaseSpeedTimer = 0;
        InitialStepRecords();

        // UI 타이머 배열 초기화
        uiTimer = new float[5] { 0, 0, 0, 0, 0 };

        footPrintLerpT = 0;
        centerButton = GameObject.Find("CenterButton");

        lastPositionLeftFoot = Vector3.zero;
        lastPositionRightFoot = Vector3.zero;
        lastPositionHead = Vector3.zero;
    }

    // 걸음 시간 리스트 초기화 (0)
    public static void InitialStepRecords()
    {
        steps = Enumerable.Repeat<float>(0, 3).ToList();
    }

    // 걸음 시간 측정 (+ fixedDeltaTime)
    private void FixedUpdate()
    {
        stepRecordTime += Time.fixedDeltaTime;
        decreaseSpeedTimer += Time.fixedDeltaTime;
    }
    
    void Update()
    {
        HandleButtonActive();
        HandleFloorUI();
        HandleKeyboard();

    }

    // 바닥 UI 타일 보여주기, 감추기
    void HandleButtonActive()
    {
        if (GameManager.instance.GetGameState() == GameState.Game)
        {
            leftFloorTile.SetActive(true);
            centerFloorTile.SetActive(true);
            rightFloorTile.SetActive(true);

            centerButton.SetActive(false);

            newGameButtonPause.SetActive(false);
            toMenuButtonPause.SetActive(false);
            nextPageButton.SetActive(false);
            newGameButtonResult.SetActive(false);
            toMenuButtonResult.SetActive(false);

            timerBox.SetActive(true);
            barHpBox.SetActive(true);
        }
        else if (GameManager.instance.GetGameState() == GameState.Pause)
        {
            leftFloorTile.SetActive(false);
            centerFloorTile.SetActive(false);
            rightFloorTile.SetActive(false);

            centerButton.SetActive(true);

            newGameButtonPause.SetActive(true);
            toMenuButtonPause.SetActive(true);
            nextPageButton.SetActive(false);
            newGameButtonResult.SetActive(false);
            toMenuButtonResult.SetActive(false);

            timerBox.SetActive(true);
            barHpBox.SetActive(true);
        }
        else if (GameManager.instance.GetGameState() == GameState.Result)
        {
            leftFloorTile.SetActive(false);
            centerFloorTile.SetActive(false);
            rightFloorTile.SetActive(false);

            centerButton.SetActive(true);

            newGameButtonPause.SetActive(false);
            toMenuButtonPause.SetActive(false);
            nextPageButton.SetActive(true);
            newGameButtonResult.SetActive(false);
            toMenuButtonResult.SetActive(false);

            timerBox.SetActive(false);
            barHpBox.SetActive(false);
        }
        else if (GameManager.instance.GetGameState() == GameState.MyRank)
        {
            leftFloorTile.SetActive(false);
            centerFloorTile.SetActive(false);
            rightFloorTile.SetActive(false);

            centerButton.SetActive(true);

            newGameButtonPause.SetActive(false);
            toMenuButtonPause.SetActive(false);
            nextPageButton.SetActive(false);
            newGameButtonResult.SetActive(true);
            toMenuButtonResult.SetActive(true);

            timerBox.SetActive(false);
            barHpBox.SetActive(false);
        }
    }

    // 바닥 UI 조정
    void HandleFloorUI()
    {
        if (GameManager.instance.GetGameState() == GameState.Game)
        {
            HandleFloorUIText();
            HandleFloorTileHighlight();
        }

        if (GameManager.instance.GetKinectState())
            HandleKinect();
        else
        {
            leftFootPrint.transform.localScale = Vector3.zero;
            rightFootPrint.transform.localScale = Vector3.zero;
        }
        HandleGameButtons();
    }

    void HandleFloorUIText()
    {
        if (GameUI.instance.timerText && timerBox.activeSelf)
            timerText.text = GameUI.instance.timerText.text.ToString();
        if (GameUI.instance.barHp && barHpBox.activeSelf)
            barHp.UpdateValue(Player.instance.hp, ConstInfo.maxHp);
    }

    void HandleFloorTileHighlight() {
        UnselectFloorTile();
        SelectFloorTile();
    }


    // 게임 바닥타일과 유저의 상호작용
    void HandleGameButtons()
    {
        HandleCenterButton();

        if ((Avatar.OneFootOverlaps(leftFootPrint, rightFootPrint, newGameButtonPause) || Input.GetKey(KeyCode.Alpha2)) && newGameButtonPause.activeSelf)
            HandleNewGameTilePause();
        else
            InitialButtonTexture(newGameButtonPause, FloorTexture.HomeButton, 0, false);

        if ((Avatar.OneFootOverlaps(leftFootPrint, rightFootPrint, toMenuButtonPause) || Input.GetKey(KeyCode.Alpha1)) && toMenuButtonPause.activeSelf)
            HandleToMenuTilePause();
        else
            InitialButtonTexture(toMenuButtonPause, FloorTexture.BackButton, 1, false);

        if ((Avatar.OneFootOverlaps(leftFootPrint, rightFootPrint, nextPageButton) || Input.GetKey(KeyCode.Return)) && nextPageButton.activeSelf)
            HandleNextPageTile();
        else
            InitialButtonTexture(nextPageButton, FloorTexture.RemeasurementButton, 2, true);

        if ((Avatar.OneFootOverlaps(leftFootPrint, rightFootPrint, newGameButtonResult) || Input.GetKey(KeyCode.Alpha2)) && newGameButtonResult.activeSelf)
            HandleNewGameTileResult();
        else
            InitialButtonTexture(newGameButtonResult, FloorTexture.RightButton, 3, true);

        if ((Avatar.OneFootOverlaps(leftFootPrint, rightFootPrint, toMenuButtonResult) || Input.GetKey(KeyCode.Alpha1)) && newGameButtonResult.activeSelf)
            HandleToMenuTileResult();
        else
            InitialButtonTexture(toMenuButtonResult, FloorTexture.LeftButton, 4, true);
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
    void InitialButtonTexture(GameObject button, Texture texture, int timerIndex, bool move)
    {
        FloorTexture.setButtonTexture(button, texture);
        FloorTexture.ProgressDelayTexture(button, 0);
        if (move)
            FloorTexture.MoveAllChildTexture(button, false);
        uiTimer[timerIndex] = 0;
    }

    // 버튼 누르는 효과 설정
    void HandleButtonTexture(GameObject button, Texture texture, int timerIndex, bool move)
    {
        FloorTexture.setButtonTexture(button, texture);
        uiTimer[timerIndex] += Time.deltaTime;
        if (move)
            FloorTexture.MoveAllChildTexture(button, true);
        FloorTexture.ProgressDelayTexture(button, uiTimer[timerIndex] / ConstInfo.buttonPushTime);
    }

    // 일시정지의 새 게임 버튼을 누른 경우
    void HandleNewGameTilePause()
    {
        HandleButtonTexture(newGameButtonPause, FloorTexture.HomeButtonPress, 0, false);
        if (uiTimer[0] > ConstInfo.buttonPushTime)
        {
            GameUI.instance.HandleNewGame();
            InitialButtonTexture(newGameButtonPause, FloorTexture.HomeButton, 0, false);
        }
    }

    // 게임결과의 다시하기 버튼을 누른 경우 (내 점수 화면)
    void HandleNewGameTileResult()
    {
        HandleButtonTexture(newGameButtonResult, FloorTexture.RightButtonPress, 3, true);
        if (uiTimer[3] > ConstInfo.buttonPushTime)
        {
            MyRankUI.instance.HandleRetry();
            InitialButtonTexture(newGameButtonResult, FloorTexture.RightButton, 3, true);
        }
    }

    // 일시정지의 메뉴 버튼을 누른 경우
    void HandleToMenuTilePause()
    {
        HandleButtonTexture(toMenuButtonPause, FloorTexture.BackButtonPress, 1, false);
        if (uiTimer[1] > ConstInfo.buttonPushTime)
        {
            GameUI.instance.HandleToMenu();
            InitialButtonTexture(toMenuButtonPause, FloorTexture.BackButton, 1, false);
        }    
    }

    // 게임결과의 메뉴버튼을 누른경우 (내 점수 화면)
    void HandleToMenuTileResult()
    {
        HandleButtonTexture(toMenuButtonResult, FloorTexture.LeftButtonPress, 4, true);
        if (uiTimer[4] > ConstInfo.buttonPushTime)
        {
            MyRankUI.instance.HandleToMenu();
            InitialButtonTexture(toMenuButtonResult, FloorTexture.LeftButton, 4, true);
        }
    }

    // 다음 페이지 버튼을 누른 경우 (결과 화면)
    void HandleNextPageTile()
    {
        HandleButtonTexture(nextPageButton, FloorTexture.RemeasurementButtonPress, 2, true);
        if (uiTimer[2] > ConstInfo.buttonPushTime)
        {
            ResultUI.instance.HandleNextPage();
            InitialButtonTexture(nextPageButton, FloorTexture.RemeasurementButton, 2, true);
        }
    }



    void HandleKinect() {
        HandleFootPrint();
        HandleUserAction();
        HandlePause();
        if (GameManager.instance.GetGameState() == GameState.Game)
            HandleHighlight();
    }



    // 발 위치 원 설정
    void HandleFootPrint()
    {
        HandleFootPrintPosition();
        HandleFootPrintSize();
    }

    // 바닥 스크린들의 밟은 판정   
    void HandleHighlight()
    {        
        HandleHighlightPosition(Player.instance.highlight, leftFloorTile, ConstInfo.left);
        HandleHighlightPosition(Player.instance.highlight, centerFloorTile, ConstInfo.center);
        HandleHighlightPosition(Player.instance.highlight, rightFloorTile, ConstInfo.right);
    }

    void UnselectFloorTile() {
        FloorTexture.setFloorTileTexture(leftFloorTile, FloorTexture.FloorTileUnSelected);
        FloorTexture.setFloorTileTexture(centerFloorTile, FloorTexture.FloorTileUnSelected);
        FloorTexture.setFloorTileTexture(rightFloorTile, FloorTexture.FloorTileUnSelected);
    }

    void SelectFloorTile() {
        switch (Player.instance.highlight.transform.position.x) {
            case ConstInfo.left:
                FloorTexture.setFloorTileTexture(leftFloorTile, FloorTexture.FloorTileSelected);
                break;
            case ConstInfo.center:
                FloorTexture.setFloorTileTexture(centerFloorTile, FloorTexture.FloorTileSelected);
                break;
            case ConstInfo.right:
                FloorTexture.setFloorTileTexture(rightFloorTile, FloorTexture.FloorTileSelected);
                break;
        }
    }

    // 두 발이 모두 타일 안에 있는 경우 하이라이트 위치 변경
    void HandleHighlightPosition(GameObject highlight, GameObject floorTile, float positionX)
    {
        if (Avatar.VectorInside(Avatar.userPosition, floorTile))
            highlight.transform.position = new Vector3(positionX, highlight.transform.position.y, highlight.transform.position.z);
    }

    // 발 위치 원 좌표 변경
    void HandleFootPrintPosition()
    {
        leftFootPrint.transform.localPosition = 
            Vector3.Lerp(leftFootPrint.transform.localPosition, new Vector3(Avatar.userPositionLeftFoot.x, Avatar.userPositionLeftFoot.z, 0), footPrintLerpT);
        rightFootPrint.transform.localPosition = 
            Vector3.Lerp(rightFootPrint.transform.localPosition, new Vector3(Avatar.userPositionRightFoot.x, Avatar.userPositionRightFoot.z, 0), footPrintLerpT);
        footPrintLerpT += ConstInfo.footPrintSpeed * Time.deltaTime;
        if (footPrintLerpT > 1.0f)
            footPrintLerpT = 0.0f;
    }

    // 발 위치 원 크기 변경
    void HandleFootPrintSize()
    {
        float newLeftFootPrintSize = Avatar.HandleFootprintSize(Avatar.userPositionLeftFoot.y);
        float newRightFootPrintSize = Avatar.HandleFootprintSize(Avatar.userPositionRightFoot.y);
        leftFootPrint.transform.localScale = new Vector3(newLeftFootPrintSize, newLeftFootPrintSize, newLeftFootPrintSize);
        rightFootPrint.transform.localScale = new Vector3(newRightFootPrintSize, newRightFootPrintSize, newRightFootPrintSize);
    }



    // 점프, 걸음, 펀치 판정
    void HandleUserAction() {
        HandleJump(); // 점프 판정 (버전 확인)
        HandleSteps();
        HandleAvatarPunch();
        HandleKick();
    }

    // 결음 기록 조건 만족 시 함수 호출
    void HandleSteps()
    {
        if (decreaseSpeedTimer >= 2 && !Player.instance.isJumping) {
            decreaseSpeedTimer = 0;
            steps.Add(0);
            steps.RemoveAt(0);
        }
            
        if (((stepSide == true && Avatar.userPositionLeftFoot.y > ConstInfo.stepHeight  && Avatar.userPositionRightFoot.y < ConstInfo.stepHeight)
            || (stepSide == false && Avatar.userPositionRightFoot.y > ConstInfo.stepHeight && Avatar.userPositionLeftFoot.y < ConstInfo.stepHeight))
            && stepRecordTime != 0)
            HandleStep();

        Tile.userSpeed = steps.Average();
    }

    // 걸음시간 기록 및 초기화, 결음 방향 변경
    void HandleStep()
    {
        stepSide = !stepSide;
        steps.Add(10 / stepRecordTime);
        steps.RemoveAt(0);
        stepRecordTime = 0;
        decreaseSpeedTimer = 0;
    }

    // 점프 조건 (이전 프레임보다 양 발 모두 jumpHeight 이상 증가 + 발높이 차가 0.3 이하 + 양발의 x변화량이 5 미만)
    void HandleJump() {
        isJumping = lastPositionLeftFoot.y + ConstInfo.jumpHeight < Avatar.userPositionLeftFoot.y
            && lastPositionRightFoot.y + ConstInfo.jumpHeight < Avatar.userPositionRightFoot.y
            && lastPositionHead.y + ConstInfo.jumpHeight < Avatar.userPositionHead.y
            && lastPositionLeftFoot.y != 0 && lastPositionRightFoot.y != 0
            && Mathf.Abs(lastPositionLeftFoot.y - lastPositionRightFoot.y) < ConstInfo.jumpFootHeightDifferenceLimit
            && Mathf.Abs(lastPositionLeftFoot.x - Avatar.userPositionLeftFoot.x) < ConstInfo.jumpFootPositionVariationLimit
            && Mathf.Abs(lastPositionRightFoot.x - Avatar.userPositionRightFoot.x) < ConstInfo.jumpFootPositionVariationLimit;
        lastPositionLeftFoot = Avatar.userPositionLeftFoot;
        lastPositionRightFoot = Avatar.userPositionRightFoot;
    }

    // 펀치 조건
    void HandleAvatarPunch()
    {
        if ((Avatar.userPositionLeftHand.z > Avatar.userPositionHead.z + Avatar.DistanceBetweenHandAndElbow)
            || (Avatar.userPositionRightHand.z > Avatar.userPositionHead.z + Avatar.DistanceBetweenHandAndElbow))
            isPunching = true;
        else
            isPunching = false;
    }
    //kick 판정 조건
    void HandleKick()
    {
        if ((Avatar.userPositionLeftFoot.z > Avatar.userPosition.z)
            || (Avatar.userPositionRightFoot.z > Avatar.userPosition.z))
            isKicking = true;
        else
            isKicking = false;
    }


    // 일시정지와 다시시작 조건
    void HandlePause()
    {
        if (Avatar.GetUserValid())
        {
            if ((Avatar.userPositionLeftHand.y > Avatar.userPositionHead.y
                && Avatar.userPositionRightHand.y > Avatar.userPositionHead.y)
                && Avatar.TwoFootOverlaps(leftFootPrint, rightFootPrint, centerButton) && GameManager.instance.GetGameState() == GameState.Pause)
                GameUI.instance.Pause();
        }
        else
        {
            if (GameManager.instance.GetGameState() == GameState.Game)
                GameUI.instance.Pause();
        }
    }



    // 키보드 입력
    void HandleKeyboard()
    {
        if (GameManager.instance.GetGameState() == GameState.Game)
            HandleKeyboardGame();

        if (Input.GetKeyDown(KeyCode.Backspace) 
            && (GameManager.instance.GetGameState() == GameState.Pause || GameManager.instance.GetGameState() == GameState.Game))
            GameUI.instance.Pause();
    }

    // 게임 화면의 키보드 상호작용
    void HandleKeyboardGame()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Player.instance.transform.position = new Vector3(ConstInfo.left, ConstInfo.playerInitialPositionY, ConstInfo.playerInitialPositionZ);
            Player.instance.highlight.transform.position = new Vector3(ConstInfo.left, ConstInfo.playerInitialPositionY, ConstInfo.playerInitialPositionZ);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Player.instance.transform.position = new Vector3(ConstInfo.center, ConstInfo.playerInitialPositionY, ConstInfo.playerInitialPositionZ);
            Player.instance.highlight.transform.position = new Vector3(ConstInfo.center, ConstInfo.playerInitialPositionY, ConstInfo.playerInitialPositionZ);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Player.instance.transform.position = new Vector3(ConstInfo.right, ConstInfo.playerInitialPositionY, ConstInfo.playerInitialPositionZ);
            Player.instance.highlight.transform.position = new Vector3(ConstInfo.right, ConstInfo.playerInitialPositionY, ConstInfo.playerInitialPositionZ);
        }

        if (Input.GetKey(KeyCode.LeftAlt))
            isJumping = true;
        else if (!GameManager.instance.GetKinectState())
            isJumping = false;

        if (Input.GetKey(KeyCode.LeftControl))
            Tile.userSpeed += ConstInfo.extraSpeedIncrease;

        if (Input.GetKey(KeyCode.LeftShift))
            isPunching = true;
        else if (!GameManager.instance.GetKinectState())
            isPunching = false;


        //Kick 키보드 설정
        if (Input.GetKey(KeyCode.Return))
            isKicking = true;
        else if (!GameManager.instance.GetKinectState())
            isKicking = false;
    }
}
