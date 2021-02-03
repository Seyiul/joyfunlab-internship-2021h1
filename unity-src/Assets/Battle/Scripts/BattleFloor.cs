using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleFloor : MonoBehaviour
{

    public GameObject rightMarker;
    public GameObject rightButton;
    public GameObject pauseButton;
    public GameObject enterButton;


    private Vector3 handlePositiontRightFoot;
    private bool press;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        press = false;
        timer = 0;

        rightButton.SetActive(false);
        pauseButton.SetActive(false);
        rightMarker.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        HandleMenu();


        if (timer > 1.2)
        {
            press = true;
        }
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
            enterButton.SetActive(false);
        }
        else if (GameManager.instance.GetGameState() == GameState.Result)
        {
            rightButton.SetActive(true);
            pauseButton.SetActive(false);
            rightMarker.SetActive(true);
            enterButton.SetActive(false);
        }
        else if(GameManager.instance.GetGameState() == GameState.Pause)
        {
            rightButton.SetActive(false);
            pauseButton.SetActive(false);
            rightMarker.SetActive(true);
            enterButton.SetActive(true);
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
        if ((Vector2.Distance(new Vector2(rightButton.transform.position.x, rightButton.transform.position.y), new Vector2(Avatar.userPositionRightFoot.x, Avatar.userPositionRightFoot.z)) < 107 && press)
       && GameManager.instance.GetGameState() == GameState.Result)
        {
            GameManager.instance.SetGameState(GameState.Game);
            SceneManager.LoadScene("Game");
            press = false;
        }

        if ((Vector2.Distance(new Vector2(pauseButton.transform.position.x, pauseButton.transform.position.y),
            new Vector2(Avatar.userPositionRightFoot.x, Avatar.userPositionRightFoot.z)) < 76) && press && GameManager.instance.GetGameState() == GameState.Battle)
        {
            GameManager.instance.SetGameState(GameState.Pause);
            press = false;
        }
        if ((((Avatar.userPositionLeftFoot.x > enterButton.transform.position.x - 158 && Avatar.userPositionLeftFoot.x < enterButton.transform.position.x + 158) &&
            (Avatar.userPositionLeftFoot.z > enterButton.transform.position.y - 61 && Avatar.userPositionLeftFoot.z < enterButton.transform.position.y + 61)) ||
            ((Avatar.userPositionRightFoot.x > enterButton.transform.position.x - 158 && Avatar.userPositionRightFoot.x < enterButton.transform.position.x + 158) &&
            (Avatar.userPositionRightFoot.z > enterButton.transform.position.y - 61 && Avatar.userPositionRightFoot.z < enterButton.transform.position.y + 61)) && press)
            && (GameManager.instance.GetGameState() != GameState.Pause))
        {
            GameManager.instance.SetGameState(GameState.Battle);
            press = false;
        }
    }
}
