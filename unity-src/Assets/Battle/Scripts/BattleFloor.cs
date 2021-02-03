using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleFloor : MonoBehaviour
{

    public GameObject rightMarker;
    public GameObject rightButton;
    public GameObject pauseButton;
    public GameObject centerButton;


    public static bool pause;
    public static bool next;
    public static bool regame;

    private Vector3 handlePositiontRightFoot;
    private bool press;

    private float timeTimer;

    // Start is called before the first frame update
    void Start()
    {
        timeTimer = 0;
        press = false;
        pause = false;
        next = false;
        regame = false;

        rightButton.SetActive(false);
        pauseButton.SetActive(false);
        rightMarker.SetActive(false);
        centerButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timeTimer += Time.deltaTime;
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
            HandleKinectClick();
        }
    }
    void HandleMenu()
    {
        if (GameManager.instance.GetGameState() == GameState.Battle)
        {
            rightButton.SetActive(false);
            pauseButton.SetActive(true);
            rightMarker.SetActive(true);
            centerButton.SetActive(false);
        }
        else if (GameManager.instance.GetGameState() == GameState.Result)
        {
            rightButton.SetActive(true);
            pauseButton.SetActive(false);
            rightMarker.SetActive(true);
            centerButton.SetActive(false);
        }
        else if(GameManager.instance.GetGameState() == GameState.Pause)
        {
            rightButton.SetActive(false);
            pauseButton.SetActive(true);
            rightMarker.SetActive(true);
            centerButton.SetActive(true);
        }


    }
    public static Vector3 HandleKinectPosition(Vector3 kinectPosition)
    {
        return new Vector3(kinectPosition.x * 711, kinectPosition.y * 720, (kinectPosition.z - 1.45f) * -720);
    }

    void MarkerMove()
    {
        rightMarker.transform.localPosition = new Vector3(Avatar.userPositionRightFoot.x, Avatar.userPositionRightFoot.z, 0);
    }
    void HandleKinectClick()
    {
        if ((Vector2.Distance(new Vector2(rightButton.transform.localPosition.x, rightButton.transform.localPosition.y), new Vector2(Avatar.userPositionRightFoot.x, Avatar.userPositionRightFoot.z)) < 107 && press)
            && GameManager.instance.GetGameState() == GameState.Result)
        {
            next = true;
            press = false;
        }

        if ((Vector2.Distance(new Vector2(pauseButton.transform.localPosition.x, pauseButton.transform.localPosition.y),
            new Vector2(Avatar.userPositionRightFoot.x, Avatar.userPositionRightFoot.z)) < 76) && press)
        {
            pause = true;
            press = false;
        }
        if ((Vector2.Distance(new Vector2(centerButton.transform.localPosition.x, centerButton.transform.localPosition.y),
              new Vector2(Avatar.userPositionRightFoot.x, Avatar.userPositionRightFoot.z)) < 140))
        {
            regame = true;
            press = false;

        }
    }
}
