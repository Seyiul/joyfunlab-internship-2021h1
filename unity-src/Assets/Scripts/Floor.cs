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

    private Vector3 handlePositionLeftFoot;
    private Vector3 handlePositionRightFoot;
    private float timeTimer;
    private bool press;
    //게임정보
    public Text timer;
    public Text hp;
    public Text pauseMenu;

    // Start is called before the first frame update
    void Start()
    {

        timeTimer = 0;
        press = false;
    }

    // Update is called once per frame
    void Update()
    {

        timeTimer += Player.instance.time;
        if (timeTimer > 1.2)
        {
            press = true;
        }
        if (GameManager.instance.GetKinectState())
        {
            MarkerMove();
            HandleMenu();
            HandleKinectClick();
        }
        //현재 시간을 floor canvas 에 보여줌
        timer.text = (Mathf.Floor(Player.instance.time * 10) * 0.1f).ToString();
        //현재 체력을 floor canvas 에 보여줌
        hp.text = Player.instance.hp.ToString() + "/" + Player.instance.maxHp.ToString();
    }
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

        }
        else if (GameManager.instance.GetGameState() == GameState.Setting)
        {
            centerButton.SetActive(true);
            rightMarker.SetActive(true);
            leftMarker.SetActive(true);
            enterButton.SetActive(true);
            upButton.SetActive(true);
            downButton.SetActive(true);
            pauseButton.SetActive(true);
            rightButton.SetActive(true);
            leftButton.SetActive(true);

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

            pauseMenu.text = "일시정지";
        }
    }
    public static Vector3 HandleKinectPosition(Vector3 kinectPosition)
    {
        return new Vector3(kinectPosition.x * 711, kinectPosition.y * 720, (kinectPosition.z - 1.45f) * -720);
    }

    void MarkerMove()
    {
        leftMarker.transform.localPosition = new Vector3(Avatar.userPositionLeftFoot.x, Avatar.userPositionLeftFoot.z, 0);
        rightMarker.transform.localPosition = new Vector3(Avatar.userPositionRightFoot.x, Avatar.userPositionRightFoot.z, 0);
    }
    void HandleKinectClick()
    {
        if (((Vector2.Distance(new Vector2(downButton.transform.position.x, downButton.transform.position.y), new Vector2(Avatar.userPositionRightFoot.x, Avatar.userPositionRightFoot.z)) < 107) && press)
            && (GameManager.instance.GetGameState() != GameState.Game) && (GameManager.instance.GetGameState() != GameState.Battle))
        {
            press = false;
        }
        else if (((Vector2.Distance(new Vector2(upButton.transform.position.x, upButton.transform.position.y), new Vector2(Avatar.userPositionLeftFoot.x, Avatar.userPositionLeftFoot.z)) < 107) && press)
            && (GameManager.instance.GetGameState() != GameState.Game) && (GameManager.instance.GetGameState() != GameState.Battle))
        {
            press = false;
        }
        if ((Vector2.Distance(new Vector2(rightButton.transform.position.x, rightButton.transform.position.y), new Vector2(Avatar.userPositionRightFoot.x, Avatar.userPositionRightFoot.z)) < 107 && press)
            && GameManager.instance.GetGameState() == GameState.Setting)
        {
            press = false;
        }
        else if ((Vector2.Distance(new Vector2(leftButton.transform.position.x, leftButton.transform.position.y), new Vector2(Avatar.userPositionLeftFoot.x, Avatar.userPositionLeftFoot.z)) < 107 && press)
            && GameManager.instance.GetGameState() == GameState.Setting)
        {
            press = false;
        }
        if ((((Avatar.userPositionLeftFoot.x > enterButton.transform.position.x - 158 && Avatar.userPositionLeftFoot.x < enterButton.transform.position.x + 158) &&
            (Avatar.userPositionLeftFoot.z > enterButton.transform.position.y - 61 && Avatar.userPositionLeftFoot.z < enterButton.transform.position.y + 61)) ||
            ((Avatar.userPositionRightFoot.x > enterButton.transform.position.x - 158 && Avatar.userPositionRightFoot.x < enterButton.transform.position.x + 158) &&
            (Avatar.userPositionRightFoot.z > enterButton.transform.position.y - 61 && Avatar.userPositionRightFoot.z < enterButton.transform.position.y + 61)) && press)
            && (GameManager.instance.GetGameState() != GameState.Game) && (GameManager.instance.GetGameState() != GameState.Battle))
        { press = false; }

        if ((Vector2.Distance(new Vector2(pauseButton.transform.position.x, pauseButton.transform.position.y),
            new Vector2(Avatar.userPositionRightFoot.x, Avatar.userPositionRightFoot.z)) < 76) && press)
        {
            press = false;
        }
    }
}
