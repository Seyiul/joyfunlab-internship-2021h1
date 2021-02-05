using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Floor : MonoBehaviour
{
    //button
    public GameObject leftMarker;
    public GameObject rightMarker;
    public GameObject enterButton;
    public GameObject pauseButton;
    public GameObject rightButton;
    public GameObject leftButton;
    public GameObject upButton;
    public GameObject downButton;
    public GameObject centerButton;
    public GameObject pauseText;

    private Vector3 handlePositionLeftFoot;
    private Vector3 handlePositionRightFoot;
    private float timeTimer;
    private bool press;
    //게임정보
    public Text timer;
    public Text hp;
    public Text pauseMenu;

    public static bool isCenter;
    public static bool isUp;
    public static bool isDown;
    public static bool isRight;
    public static bool isLeft;
    public static bool isPause;
    public static bool isEnter;
    public static bool isGame;
    // Start is called before the first frame update
    void Start()
    {
        isUp = false;
        isDown = false;
        isRight = false;
        isLeft = false;
        isPause = false;
        isEnter = false;

        isGame = false;
        isCenter = false;

        timeTimer = 0;
        press = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        timeTimer += Time.deltaTime;
        //1초에 한번씩 press를 true로 변경
        if (timeTimer > 1.0)
        {
            press = true;
            timeTimer = 0;
        }
        HandleMenu();
        if (GameManager.instance.GetKinectState())
        {
            MarkerMove();
            HandleMenu();

            if (isCenter)
                HandleKinectClick();
            else if (!isCenter && isGame)
                HandleKinectPause();
            else
                HandleCenter();
        }
        //현재 시간을 floor canvas 에 보여줌
        timer.text = (Mathf.Floor(Player.instance.time * 10) * 0.1f).ToString();
        //현재 체력을 floor canvas 에 보여줌
        hp.text = Player.instance.hp.ToString() + "/" + Player.instance.maxHp.ToString();
    }
    void HandleCenter()
    {
        if ((Vector2.Distance(new Vector2(centerButton.transform.localPosition.x, centerButton.transform.localPosition.y),
            new Vector2(Avatar.userPositionRightFoot.x, Avatar.userPositionRightFoot.z)) < 140) &&
                (Vector2.Distance(new Vector2(centerButton.transform.localPosition.x, centerButton.transform.localPosition.y),
                new Vector2(Avatar.userPositionLeftFoot.x, Avatar.userPositionLeftFoot.z)) < 140))
            isCenter = true;
    }
    //바닥에 표시되는 버튼을 상황에따라 다르게 출려
    void HandleMenu()
    {
        if (GameManager.instance.GetGameState() == GameState.Menu)
        {
            centerButton.SetActive(true);
            rightMarker.SetActive(true);
            leftMarker.SetActive(true);
            enterButton.SetActive(true);
            upButton.SetActive(true);
            downButton.SetActive(true);
            pauseButton.SetActive(false);
            rightButton.SetActive(false);
            leftButton.SetActive(false);
            pauseText.SetActive(false);
            isGame = false;
        }
        else if (GameManager.instance.GetGameState() == GameState.Setting)
        {
            centerButton.SetActive(true);
            rightMarker.SetActive(true);
            leftMarker.SetActive(true);
            enterButton.SetActive(true);
            upButton.SetActive(true);
            downButton.SetActive(true);
            pauseButton.SetActive(false);
            rightButton.SetActive(true);
            leftButton.SetActive(true);
            pauseText.SetActive(false);

            isGame = false;
            pauseMenu.text = "뒤로가기";
        }
        else if (GameManager.instance.GetGameState() == GameState.Pause)
        {
            centerButton.SetActive(true);
            rightMarker.SetActive(true);
            leftMarker.SetActive(true);
            enterButton.SetActive(true);
            upButton.SetActive(true);
            downButton.SetActive(true);
            pauseButton.SetActive(false);
            rightButton.SetActive(false);
            leftButton.SetActive(false);
            pauseText.SetActive(true);
            isGame = false;
            
        }
        else if ((GameManager.instance.GetGameState() == GameState.Game) || (GameManager.instance.GetGameState() == GameState.Battle))
        {
            centerButton.SetActive(false);
            rightMarker.SetActive(false);
            leftMarker.SetActive(false);
            enterButton.SetActive(false);
            upButton.SetActive(false);
            downButton.SetActive(false);
            pauseButton.SetActive(true);
            rightButton.SetActive(false);
            leftButton.SetActive(false);
            pauseText.SetActive(false);

            isGame = true;
            isCenter = false;
            pauseMenu.text = "일시정지";
        }
        else if ((GameManager.instance.GetGameState() == GameState.Result))
        {
            centerButton.SetActive(true);
            rightMarker.SetActive(true);
            leftMarker.SetActive(true);
            enterButton.SetActive(false);
            upButton.SetActive(false);
            downButton.SetActive(false);
            pauseButton.SetActive(false);
            rightButton.SetActive(true);
            leftButton.SetActive(false);
            pauseText.SetActive(false);
            isGame = false;

        }
        else if ((GameManager.instance.GetGameState() == GameState.Rank))
        {
            centerButton.SetActive(true);
            rightMarker.SetActive(true);
            leftMarker.SetActive(true);
            enterButton.SetActive(true);
            upButton.SetActive(false);
            downButton.SetActive(false);
            pauseButton.SetActive(false);
            rightButton.SetActive(false);
            leftButton.SetActive(false);
            pauseText.SetActive(false);
        }
    }

    void MarkerMove()
    {
        leftMarker.transform.localPosition = new Vector3(Avatar.userPositionLeftFoot.x, Avatar.userPositionLeftFoot.z, 0);
        rightMarker.transform.localPosition = new Vector3(Avatar.userPositionRightFoot.x, Avatar.userPositionRightFoot.z, 0);
    }
    void HandleKinectClick()
    {
        if (((Vector2.Distance(new Vector2(downButton.transform.localPosition.x, downButton.transform.localPosition.y), new Vector2(Avatar.userPositionRightFoot.x, Avatar.userPositionRightFoot.z)) < 107) && press)
            && (GameManager.instance.GetGameState() != GameState.Game) && (GameManager.instance.GetGameState() != GameState.Battle))
        {
            isDown = true;
            press = false;
        }
        else if (((Vector2.Distance(new Vector2(upButton.transform.localPosition.x, upButton.transform.localPosition.y), new Vector2(Avatar.userPositionLeftFoot.x, Avatar.userPositionLeftFoot.z)) < 107) && press)
            && (GameManager.instance.GetGameState() != GameState.Game) && (GameManager.instance.GetGameState() != GameState.Battle))
        {
            isUp = true;
            press = false;
        }
        else if ((Vector2.Distance(new Vector2(rightButton.transform.localPosition.x, rightButton.transform.localPosition.y), new Vector2(Avatar.userPositionRightFoot.x, Avatar.userPositionRightFoot.z)) < 107 && press)
            && (GameManager.instance.GetGameState() == GameState.Setting||GameManager.instance.GetGameState() == GameState.Result))
        {
            isRight = true;
            press = false;
        }
        else if ((Vector2.Distance(new Vector2(leftButton.transform.localPosition.x, leftButton.transform.localPosition.y), new Vector2(Avatar.userPositionLeftFoot.x, Avatar.userPositionLeftFoot.z)) < 107 && press)
            && GameManager.instance.GetGameState() == GameState.Setting)
        {
            isLeft = true;
            press = false;
            
        }
        else if ((((Avatar.userPositionLeftFoot.x > enterButton.transform.localPosition.x - 158 && Avatar.userPositionLeftFoot.x < enterButton.transform.localPosition.x + 158) &&
            (Avatar.userPositionLeftFoot.z > enterButton.transform.localPosition.y - 61 && Avatar.userPositionLeftFoot.z < enterButton.transform.localPosition.y + 61))&&press ||
            ((Avatar.userPositionRightFoot.x > enterButton.transform.localPosition.x - 158 && Avatar.userPositionRightFoot.x < enterButton.transform.localPosition.x + 158) &&
            (Avatar.userPositionRightFoot.z > enterButton.transform.localPosition.y - 61 && Avatar.userPositionRightFoot.z < enterButton.transform.localPosition.y + 61)) && press)
            && (GameManager.instance.GetGameState() != GameState.Game) && (GameManager.instance.GetGameState() != GameState.Battle))
        {
            isEnter = true;
            press = false;
        }

        else if ((Vector2.Distance(new Vector2(pauseButton.transform.localPosition.x, pauseButton.transform.localPosition.y),
            new Vector2(Avatar.userPositionRightFoot.x, Avatar.userPositionRightFoot.z)) < 76) && press)
        {
            isPause = true;
            press = false;
        }
    }
    void HandleKinectPause()
    {
        if ((Vector2.Distance(new Vector2(pauseButton.transform.localPosition.x, pauseButton.transform.localPosition.y),
            new Vector2(Avatar.userPositionRightFoot.x, Avatar.userPositionRightFoot.z)) < 76) && press)
        {
            isPause = true;
            press = false;
        }
    }
}

