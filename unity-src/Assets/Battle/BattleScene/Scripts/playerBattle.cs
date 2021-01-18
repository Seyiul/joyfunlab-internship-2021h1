using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBattle : MonoBehaviour
{
    // 애니매이터 변수 선언
    Animator animator;

    // 애니메이터 컨트롤러 변수 선언
    public RuntimeAnimatorController animIdle;
    public RuntimeAnimatorController animJump;
    public RuntimeAnimatorController animFinish;
    public RuntimeAnimatorController animPunch;
    public RuntimeAnimatorController animKick;
    public RuntimeAnimatorController animWin;

    // 행동 관련 번수 선언
    public bool isJumping;
    public bool isKicking;
    public bool isPunching;

    public float monsterHp;
    public float hp;

    public float jumpTimer;
    public float punchTimer;
    public float kickTimer;




    void Start()
    {
        InitialValues();
    }

    // 변수 초기화
    void InitialValues()
    {
        isJumping = false;
        isPunching = false;
        isKicking = false;

        jumpTimer = 0;
        kickTimer = 0;
        punchTimer = 0;

        animator = GetComponent<Animator>();
        GetComponent<Animation>().wrapMode = WrapMode.Loop;
    }
    void Update()
    {

        hp = (float)PlayerHealthbarHandler.GetHealthBarValue() * 100;
        monsterHp = (float)HealthBarHandler.GetHealthBarValue() * 100;
        HandleGame(hp, monsterHp);
    }
    void HandleGame(float hp, float monsterHp)
    {
        if (monsterHp == 0)
        {
            animator.runtimeAnimatorController = animWin;
        }
        else if (hp == 0)
        {
            animator.runtimeAnimatorController = animFinish;

        }
        else
            HandlePlayer();
    }
    void HandlePlayer()
    {
        HandlePlayerPosition();
        HandlePlayerAction();
    }
    void HandlePlayerPosition()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = new Vector3(101, transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = new Vector3(114, transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = new Vector3(127, transform.position.y, transform.position.z);
        }
    }
    void HandlePlayerAction()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            isKicking = true;
        }


        if (Input.GetKey(KeyCode.LeftShift))
        {
            isPunching = true;
        }


        if (Input.GetKey(KeyCode.LeftAlt))
        {
            isJumping = true;
        }

        PlayerAction();
    }
    void PlayerAction()
    {
        if (isJumping || isKicking || isPunching)
        {
            if (isKicking)
                HandlePlayerKicking();
            else
            {
                if (isJumping)
                    HandlePlayerJumping();
                else if (isPunching)
                    HandlePlayerPunching();
            }
            HandlePlayerActionTimer();
        }
        else
            HandlePlayerRuntimeAnimatorController();
    }
    void HandlePlayerKicking()
    {
        animator.runtimeAnimatorController = animKick;
    }

    // 플레이어 점프 애니메이션 (점프 중 펀치 가능)
    void HandlePlayerJumping()
    {
        animator.runtimeAnimatorController = animJump;
    }

    // 플레이어 펀치 애니메이션 (펀치 도중 점프 가능)
    public void HandlePlayerPunching()
    {
        animator.runtimeAnimatorController = animPunch as RuntimeAnimatorController;
    }

    // 점프 상태 초기화 (초기화 시 펀치 상태도 초기화)
    public void InitialJumpState()
    {
        animator.runtimeAnimatorController = null;
        isJumping = false;
        isPunching = false;
        jumpTimer = 0;
    }


    // 펀치 상태 초기화
    public void InitialPunchState()
    {
        animator.runtimeAnimatorController = null;
        isPunching = false;
        punchTimer = 0;
    }

    public void InitialKickState()
    {
        animator.runtimeAnimatorController = null;
        isKicking = false;
        kickTimer = 0;
    }
    // 플레이어 동작 타이머 설정
    void HandlePlayerActionTimer()
    {
        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= 0.6f)
                InitialJumpState();
        }
        if (isKicking)
        {
            kickTimer += Time.deltaTime;
            if (kickTimer >= 1f)
                InitialKickState();
        }
        if (isPunching)
        {
            punchTimer += Time.deltaTime;
            if (punchTimer >= 1f)
                InitialPunchState();
        }
    }


    void HandlePlayerRuntimeAnimatorController()
    {
        animator.runtimeAnimatorController = animIdle as RuntimeAnimatorController;
    }


}