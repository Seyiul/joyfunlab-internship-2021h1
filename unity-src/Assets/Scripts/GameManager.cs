using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.LowLevel.PlayerLoop;


public enum GameState: int
{
    Menu = 1,
    Setting = 2,
    Rank = 3,
    Game = 4,
    Pause = 5,
    Result = 6,
    Battle = 7
}

public class GameManager : MonoBehaviour
{
    // 인스턴스, 게임상태, 키넥트 연결 상태 변수 선언
    public static GameManager instance;
    private GameState curGameState;
    private bool kinectState;

    Player player;
    MenuUI menu;

    public GameObject curTile;
    public GameObject nextTile;

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
        kinectState = false;
        curGameState = GameState.Menu;
        player = GameObject.Find("Player").GetComponent<Player>();
        menu = GameObject.Find("MenuUI").GetComponent<MenuUI>();
    }

    void Update()
    {
        Debug.Log(curGameState);

        if (curGameState == GameState.Menu || curGameState == GameState.Pause)
            menu.MenuHandle();
        //게임 중에 일시정지 상태로 변경하면(esc 누르면)
        if (Input.GetKeyDown(KeyCode.Escape) && curGameState == GameState.Game)
            curGameState = GameState.Pause;

    }

    // 화면 설정 (디스플레이가 하나일 경우 전면 UI만 출력, 두개 이상일 경우 바닥 UI 출력)
    public void DisplaySetting()
    {
        if (Display.displays.Length > 1)
            Display.displays[1].Activate();
        if (Display.displays.Length > 2)
            Display.displays[2].Activate();
    }

    // 키넥트 연결 여부 변수 Getter & Setter
    public bool GetKinectState() { return kinectState; }
    public void SetKinectState(bool newKinectState) { kinectState = newKinectState; }



    // 게임상태 변수 Getter & Setter
    public GameState GetGameState() { return curGameState; }
    public void SetGameState(GameState newGameState) { curGameState = newGameState; }

}