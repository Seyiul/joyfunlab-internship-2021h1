using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.PlayerLoop;

public enum GameState: int
{
    Menu = 1,
    Setting = 2,
    Ranking = 3,
    Game = 4,
    Pause = 5,
    Result = 6,
    MyRank = 7
}

public class GameManager : MonoBehaviour
{
    // 인스턴스, 게임상태, 키넥트 연결 상태 변수 선언
    public static GameManager instance;
    private GameState currentGameState;
    private bool kinectState;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        DisplaySetting();
        HandleFrameRate();
        kinectState = false;
        Time.timeScale = 1;
    }

    // 화면 설정 (디스플레이가 하나일 경우 전면 UI만 출력, 두개 이상일 경우 바닥 UI 출력)
    public void DisplaySetting()
    {
        Screen.SetResolution(ConstInfo.floorUICanvasWidth, ConstInfo.floorUICanvasHeight, true);
        if (Display.displays.Length > 1)
            Display.displays[1].Activate();
        if (Display.displays.Length > 2)
            Display.displays[2].Activate();
    }

    // 프레임 설정
    public void HandleFrameRate()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = ConstInfo.gameFrameRate;
    }



    // 키넥트 연결 여부 변수 Getter & Setter
    public bool GetKinectState() { return kinectState; }
    public void SetKinectState(bool newKinectState) { kinectState = newKinectState; }



    // 게임상태 변수 Getter & Setter
    public GameState GetGameState() { return currentGameState; }
    public void SetGameState(GameState newGameState) { currentGameState = newGameState; }
}