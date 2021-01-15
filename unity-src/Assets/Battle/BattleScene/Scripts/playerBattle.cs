using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBattle : MonoBehaviour
{
    public static playerBattle instance; 
    // 오브젝트 변수 선언
    public GameObject highlight;

    // 애니매이터 변수 선언
    Animator animator;

    // 애니메이터 컨트롤러 변수 선언
    public RuntimeAnimatorController animIdle;
    public RuntimeAnimatorController animJump;
    public RuntimeAnimatorController animFinish;
    public RuntimeAnimatorController animPunch;

    public RuntimeAnimatorController animKick;

    // 행동 관련 번수 선언
    public bool isJumping;
    public float jumpTimer;

    public bool isKicking;
    public float kickTimer;

    public bool isPunching;
    public float punchTimer;


    public float hp;



    // 인스턴스 설정
    private void Awake() { instance = this; }

    void Start() {
        InitialValues();
    }

    // 변수 초기화
    void InitialValues()
    {
        isJumping = false;
        jumpTimer = 0;
        isPunching = false;
        punchTimer = 0;

        isKicking = false;
        kickTimer = 0;
        

        animator = GetComponent<Animator>();
        GetComponent<Animation>().wrapMode = WrapMode.Loop;
    }


    void Update()
    {
        hp = (float)PlayerHealthbarHandler.GetHealthBarValue() * 100;

        if (GameManager.instance.GetGameState() == GameState.Game)
            HandleGame(GameUI.instance.timer);
        else if (GameManager.instance.GetGameState() == GameState.Pause)
            animator.runtimeAnimatorController = Setting.GetCurrentAnimationState() == AnimationState.Animation ? animIdle : null;
    }

    // 시간, 체력에 따른 게임 동작 설정
    void HandleGame(float timer)
    {
        if (hp == 0)
        {
            animator.runtimeAnimatorController = animFinish;

        }
        else
            HandlePlayer();
    }



    // 플레이어 점프, 이동 설정 알고리즘
    void HandlePlayer()
    {
        HandlePlayerState();
        HandlePlayerAction();
    }

    // 플레이어 상태 업데이트 (위치)
    void HandlePlayerState()
    {
        if (GameManager.instance.GetKinectState())
            HandlePlayerPosition();
    }

    // 플레이어 동작 업데이트
    void HandlePlayerAction()
    {
        if (isJumping ||  isPunching || isKicking)
        {
            if (isJumping)
                HandlePlayerJumping();
            else if (isPunching)
                HandlePlayerPunching();
            else if (isKicking)
                HandlePlayerKicking();
            HandlePlayerActionTimer();
        }
        else
            HandlePlayerMoving(Setting.GetCurrentAnimationState());
    }

    void HandlePlayerKicking()
    {
        animator.runtimeAnimatorController = animKick;
        if (GameFloorTile.isKicking)
            isKicking= GameFloorTile.isKicking;
    }

    // 플레이어 점프 애니메이션 (점프 중 펀치 가능)
    void HandlePlayerJumping()
    {
        animator.runtimeAnimatorController = animJump;
        if (GameFloorTile.isPunching)
            isPunching = GameFloorTile.isPunching;
    }

    // 플레이어 펀치 애니메이션 (펀치 도중 점프 가능)
    public void HandlePlayerPunching()
    {
        animator.runtimeAnimatorController = animPunch as RuntimeAnimatorController;
        if (GameFloorTile.isJumping)
            isJumping = GameFloorTile.isJumping;
    }

    // 점프 상태 초기화 (초기화 시 펀치 상태도 초기화)
    public void InitialJumpState()
    {
        if (Setting.GetCurrentAnimationState() == AnimationState.Kinect)
            animator.runtimeAnimatorController = null;
        isJumping = false;
        isPunching = false;
        jumpTimer = 0;
    }


    // 펀치 상태 초기화
    public void InitialPunchState()
    {
        if (Setting.GetCurrentAnimationState() == AnimationState.Kinect)
            animator.runtimeAnimatorController = null;
        isPunching = false;
        punchTimer = 0;
    }

    public void InitialKickState()
    {
        if(Setting.GetCurrentAnimationState() == AnimationState.Kinect)
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
            if (jumpTimer >= ConstInfo.jumpingTime)
                InitialJumpState();
        }
        if (isPunching)
        {
            punchTimer += Time.deltaTime;
            if (punchTimer >= ConstInfo.punchingTime)
                InitialPunchState();
        }
        if (isKicking)
        {
            kickTimer += Time.deltaTime;
            if (kickTimer >= ConstInfo.kickingTime)
                InitialKickState();
        }
    }

    // 플레이어 이동 상태 설정
    void HandlePlayerMoving(AnimationState state)
    {
        if (state == AnimationState.Kinect)
            animator.runtimeAnimatorController = null;
        else if (state == AnimationState.Animation)
            HandlePlayerRuntimeAnimatorController();
        isJumping = GameFloorTile.isJumping;
        isPunching = GameFloorTile.isPunching;
        isKicking = GameFloorTile.isKicking;
    }
    
    void HandlePlayerRuntimeAnimatorController()
    {
        animator.runtimeAnimatorController = animIdle as RuntimeAnimatorController;
    }

    // 아바타 위치로 플레이어 위치 고정
    public void HandlePlayerPosition()
    {
        transform.position = new Vector3(Avatar.userPosition.x * (ConstInfo.runningTrackWidth / ConstInfo.floorUICanvasWidth) + ConstInfo.center,
            ConstInfo.playerInitialPositionY, ConstInfo.playerInitialPositionZ);
    }




}
