using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerLocation: int
{
    Left = -1,
    Center = 0,
    Right = 1
}

public class Player : MonoBehaviour
{
    // 인스턴스 변수 선언
    public static Player instance;

    // 애니매이터 변수 선언
    Animator animator;

    // 행동 관련 번수 선언
    public bool isJumping;
    public float jumpTimer;

    public bool isStumbling;
    public float stumbleTimer;

    public bool isPunching;
    public float punchTimer;

    public float speed;

    public bool isGameover;
    // 콤보, 체력 관련 변수, 상수 선언
    public int maxCombo;
    public int combo;
    public int point;
    public int hp;

    public PlayerLocation curLocation;


    // 인스턴스 설정
    private void Awake() { instance = this; }

    void Start() { InitialValues(); }

    // 변수 초기화
    public void InitialValues()
    {
        isJumping = false;
        jumpTimer = 0;
        isStumbling = false;
        stumbleTimer = 0;
        isPunching = false;
        punchTimer = 0;
        isGameover = false;
        speed = 0;
        point = 0;
        combo = 0;
        maxCombo = 0;
        hp = 50;
        curLocation = PlayerLocation.Center;

        animator = GetComponent<Animator>();
    }


    void Update()
    {

        if (GameManager.instance.GetGameState() == GameState.Game)
        {
            //게임의 상태가 변화하면 속도를 업데이트
            SpeedUpdate(speed);
            if (Input.GetKeyDown(KeyCode.UpArrow) && speed < 60)
                speed += 5;
            else if (Input.GetKeyDown(KeyCode.DownArrow) && speed > 0)
                speed -= 5;
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && curLocation != PlayerLocation.Left)
                HandlePlayerLocation(PlayerLocation.Left);
            else if (Input.GetKeyDown(KeyCode.RightArrow) && curLocation != PlayerLocation.Right)
                HandlePlayerLocation(PlayerLocation.Right);
        }
        else if (GameManager.instance.GetGameState() == GameState.Pause)
        {
            //현재 속도를 저장하기 위해서 speed 변수를 초기화하지 않음
            SpeedUpdate(0);
        }
        else
        {
            InitialValues();
            SpeedUpdate(speed);
        }
    }

    void SpeedUpdate(float speed)
    {
        animator.SetFloat("speed", speed);
    }


    // 플레이어 동작 업데이트
    void HandlePlayerAction()
    {
        if (isJumping || isStumbling || isPunching)
        {
            if (isStumbling)
                HandlePlayerStumbling();
            else {
                if (isJumping)
                    HandlePlayerJumping();
                else if (isPunching)
                    HandlePlayerPunching();
            }
            HandlePlayerActionTimer();
        }
    }



    // 플레이어 점프 애니메이션 (점프 중 펀치 가능)
    void HandlePlayerJumping()
    {
        animator.SetBool("isJumping", true);
    }

    // 플레이어 발걸림 애니메이션 (발걸림 도중 다른 동작 불가)
    public void HandlePlayerStumbling() {
        animator.SetBool("isStumbling", true);
    }

    // 플레이어 펀치 애니메이션 (펀치 도중 점프 가능)
    public void HandlePlayerPunching()
    {
        animator.SetBool("isPunching", true);
    }

    public void HandlePlayerLocation(PlayerLocation MovedLocation)
    {
        if (MovedLocation == PlayerLocation.Left)
        {
            transform.Translate(new Vector3(-14, 0, 0));
            curLocation--;
        }
        else
        {
            transform.Translate(new Vector3(14, 0, 0));
            curLocation++;
        }
    }
    // 점프 상태 초기화 (초기화 시 펀치 상태도 초기화)
    public void InitialJumpState()
    {
        isJumping = false;
        isPunching = false;
        jumpTimer = 0;
        animator.SetBool("isJumping", false);
    }

    // 발걸림 상태 초기화
    public void InitialStumbleState() {
        isStumbling = false;
        stumbleTimer = 0;
        animator.SetBool("isStumbling", false);
    }

    // 펀치 상태 초기화
    public void InitialPunchState() {
        isPunching = false;
        punchTimer = 0;
        animator.SetBool("isPunching", false);
    }

    // 플레이어 동작 타이머 설정
    void HandlePlayerActionTimer()
    {
        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= 0.7)
                InitialJumpState();
        }
        if (isStumbling)
        {
            stumbleTimer += Time.deltaTime;
            if (stumbleTimer >= 0.7)
                InitialStumbleState();
        }
        if (isPunching)
        {
            punchTimer += Time.deltaTime;
            if (punchTimer >= 0.7)
                InitialPunchState();
        }
    }
}
