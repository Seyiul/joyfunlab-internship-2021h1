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
    public RuntimeAnimatorController animRun;
    public RuntimeAnimatorController animWalk;
    public RuntimeAnimatorController animSprint;
    public RuntimeAnimatorController animJump;
    public RuntimeAnimatorController animFinish;
    public RuntimeAnimatorController animStumble;
    public RuntimeAnimatorController animPunch;


    // 행동 관련 번수 선언
    public bool isJumping;
    public float jumpTimer;

    public bool isStumbling;
    public float stumbleTimer;

    public bool isPunching;
    public float punchTimer;

    // 콤보, 체력 관련 변수, 상수 선언
    public int maxCombo;
    public int combo;
    public int point;
    public int hp;



    // 인스턴스 설정
    private void Awake() { instance = this; }

    void Start() { InitialValues(); }

    // 변수 초기화
    void InitialValues()
    {
        isJumping = false;
        jumpTimer = 0;
        isStumbling = false;
        stumbleTimer = 0;
        isPunching = false;
        punchTimer = 0;
        point = 0;
        combo = 0;
        maxCombo = 0;
        hp = ConstInfo.InitialHp;

        animator = GetComponent<Animator>();
        GetComponent<Animation>().wrapMode = WrapMode.Loop;
    }


    void Update()
    {
        if (GameManager.instance.GetGameState() == GameState.Game)
            HandleGame(GameUI.instance.timer);
        else if (GameManager.instance.GetGameState() == GameState.Pause)
            animator.runtimeAnimatorController = Setting.GetCurrentAnimationState() == AnimationState.Animation ? animIdle : null;
    }

    // 시간, 체력에 따른 게임 동작 설정
    void HandleGame(float timer)
    {
        if ((timer == 0 && Setting.GetCurrentTimeState() == TimeState.Normal))
        {
            animator.runtimeAnimatorController = Setting.GetCurrentAnimationState() == AnimationState.Animation ? animFinish : null;
            GameEnd();
        }
        else if (hp == 0)
        {
            animator.runtimeAnimatorController = Setting.GetCurrentAnimationState() == AnimationState.Animation ? animIdle : null;
            GameEnd();
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

    // 플레이어 상태 업데이트 (체력, 위치)
    void HandlePlayerState()
    {
        if (Setting.GetCurrentHpState() == HpState.Immortal)
            hp = 100;
        if (GameManager.instance.GetKinectState())
            HandlePlayerPosition();
    }

    // 플레이어 동작 업데이트
    void HandlePlayerAction()
    {
        if (isJumping || isStumbling || isPunching)
        {
            if (isStumbling)
                HandlePlayerStumbling();
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
            HandlePlayerMoving(Setting.GetCurrentAnimationState());
    }



    // 플레이어 점프 애니메이션 (점프 중 펀치 가능)
    void HandlePlayerJumping()
    {
        animator.runtimeAnimatorController = animJump;
        if (GameFloorTile.isPunching)
            isPunching = GameFloorTile.isPunching;
    }

    // 플레이어 발걸림 애니메이션 (발걸림 도중 다른 동작 불가)
    public void HandlePlayerStumbling() { animator.runtimeAnimatorController = animStumble; }

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

    // 발걸림 상태 초기화
    public void InitialStumbleState()
    {
        if (Setting.GetCurrentAnimationState() == AnimationState.Kinect)
            animator.runtimeAnimatorController = null;
        isStumbling = false;
        stumbleTimer = 0;
    }

    // 펀치 상태 초기화
    public void InitialPunchState()
    {
        if (Setting.GetCurrentAnimationState() == AnimationState.Kinect)
            animator.runtimeAnimatorController = null;
        isPunching = false;
        punchTimer = 0;
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
        if (isStumbling)
        {
            stumbleTimer += Time.deltaTime;
            if (stumbleTimer >= ConstInfo.stumblingTime)
                InitialStumbleState();
        }
        if (isPunching)
        {
            punchTimer += Time.deltaTime;
            if (punchTimer >= ConstInfo.punchingTime)
                InitialPunchState();
        }
    }

    // 플레이어 이동 상태 설정
    void HandlePlayerMoving(AnimationState state)
    {
        if (state == AnimationState.Kinect)
            animator.runtimeAnimatorController = null;
        else if (state == AnimationState.Animation)
            HandlePlayerRuntimeAnimatorController(Tile.actualSpeed);
        isJumping = GameFloorTile.isJumping;
        isPunching = GameFloorTile.isPunching;
    }

    // 플레이어 달리기 애니메이션 설정 (걷기, 달리기, 전력질주)
    void HandlePlayerRuntimeAnimatorController(float speed)
    {
        if (speed <= 40)
            animator.runtimeAnimatorController = animWalk as RuntimeAnimatorController;
        else if (speed > 40 && speed <= 60)
            animator.runtimeAnimatorController = animRun as RuntimeAnimatorController;
        else
            animator.runtimeAnimatorController = animSprint as RuntimeAnimatorController;
    }

    // 아바타 위치로 플레이어 위치 고정
    public void HandlePlayerPosition()
    {
        transform.position = new Vector3(Avatar.userPosition.x * (ConstInfo.runningTrackWidth / ConstInfo.floorUICanvasWidth) + ConstInfo.center,
            ConstInfo.playerInitialPositionY, ConstInfo.playerInitialPositionZ);
    }



    // 하트에 충돌 시
    public void HeartCollision(int i)
    {
        GameUI.instance.ShowHpIncreaseText();
        hp += ConstInfo.heartTileHpIncrease;
        if (hp > ConstInfo.maxHp)
            hp = ConstInfo.maxHp;
    }

    // 풍선에 닿을 시
    public void BalloonCollision()
    {
        combo += 1;
        GameUI.instance.ChangeCombo(combo);

        GameUI.instance.timer += ConstInfo.balloonTimeIncrease;
        if (GameUI.instance.timer > ConstInfo.gameTime)
            GameUI.instance.timer = ConstInfo.gameTime;

        GameUI.instance.ShowTimeIncreaseText();
    }

    // 장애물에 층돌 시
    public void ObstacleCollision()
    {
        isStumbling = true;
        Tile.userSpeed = 0;
        int comboWeight = (combo - 1) / 10;
        point += (int)(combo * ((comboWeight / 10f) + 1));
        combo = 0;
        PlayerDamaged();
    }

    // 빈 칸을 지날 시
    public void EmptyCollision()
    {
        combo += 1;
        GameUI.instance.ChangeCombo(combo);
    }



    // 데미지 판정 알고리즘
    public void PlayerDamaged()
    {
        int hpDecrease = (hp / 2) > 10 ? (hp / 2) : 10;
        if (Setting.GetCurrentHpState() == HpState.Normal)
            hp -= hpDecrease;
        if (hp < 0)
            hp = 0;
        GameUI.instance.DamageEffectTrigger();
    }



    // 게임 종료 알고리즘
    public void GameEnd() { GameUI.instance.HandleGameEnd(maxCombo, point); }
}
