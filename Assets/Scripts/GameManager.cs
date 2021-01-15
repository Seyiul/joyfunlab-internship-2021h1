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
    private GameState currentGameState;
    private bool kinectState;
    
    Player player;
    MenuUI menu;
 
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
        currentGameState = GameState.Menu;
        player = GameObject.Find("Player").GetComponent<Player>();
        menu = GameObject.Find("MenuUI").GetComponent<MenuUI>();
//        tile = GameObject.Find("BGround").GetComponent<Tile>();
    }

    void Update()
    {
        Debug.Log(currentGameState);

        if (currentGameState == GameState.Menu || currentGameState == GameState.Pause)
            MenuHandle();
        //게임 중에 일시정지 상태로 변경하면(esc 누르면)
        else if (Input.GetKeyDown(KeyCode.Escape) && currentGameState == GameState.Game)
            currentGameState = GameState.Pause;
        //일시정지 중에 게임 상태로 변경하면(esc 누르면)
        else if (Input.GetKeyDown(KeyCode.Escape) && currentGameState == GameState.Pause)
            currentGameState = GameState.Game;

        //게임이 끝나면(1번 누르면)
        else if (Input.GetKeyDown(KeyCode.Keypad1))
            currentGameState = GameState.Result;

        //랭크 창으로 이동하면(2번 누르면)
        else if (Input.GetKeyDown(KeyCode.Keypad2))
            currentGameState = GameState.Rank;

        //세팅 상태가 되면(4번 누르면)
        else if (Input.GetKeyDown(KeyCode.Keypad4))
            currentGameState = GameState.Setting;

        //배틀 상태가 되면(5번 누르면)
        else if (Input.GetKeyDown(KeyCode.Keypad5))
            currentGameState = GameState.Battle;

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
    public GameState GetGameState() { return currentGameState; }
    public void SetGameState(GameState newGameState) { currentGameState = newGameState; }

    private void MenuHandle()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (menu.current > btn.start)
                menu.current--;
            else if (menu.current == btn.start)
                menu.current = btn.quit;
            menu.Selected();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (menu.current < btn.quit)
                menu.current++;
            else if (menu.current == btn.quit)
                menu.current = btn.start;
            menu.Selected();
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            if (menu.current == btn.start)
                currentGameState = GameState.Game;
            else if (menu.current == btn.setting)
                currentGameState = GameState.Setting;
            else if (menu.current == btn.ranking)
                currentGameState = GameState.Rank;
            else if (menu.current == btn.quit)
                currentGameState = GameState.Result;
        }
    }
}