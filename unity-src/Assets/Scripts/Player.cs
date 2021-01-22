using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerLocation : int
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

    public float avatarPosition;
    public PlayerLocation curLocation;

    BoxCollider collider;
    HighlightTiles highlightTiles;
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
        highlightTiles = GameObject.Find("HighlightTiles").GetComponent<HighlightTiles>();
        animator = GetComponent<Animator>();
        collider = gameObject.GetComponent<BoxCollider>();
        avatarPosition = 0;
    }
    void Update()
    {
        if (GameManager.instance.GetGameState() == GameState.Game)
        {
            HandleInput();
            //kinectstate 가 true 면 HandleKinectPlayer를 사ㅇ
            if (GameManager.instance.GetKinectState())
            {
                HandleKinectPlayer();
            }
            //게임의 상태가 변화하면 속도를 업데이트
            SpeedUpdate(speed);
            HandlePlayerAction();   
            }
            else if (GameManager.instance.GetGameState() == GameState.Pause)
            {
                //현재 속도를 저장하기 위해서 speed 변수를 초기화하지 않음
                SpeedUpdate(0);
                InitialJumpState();
                InitialPunchState();
                InitialStumbleState();
            }
            else
            {
                InitialValues();
                SpeedUpdate(speed);
            }
    }
    void HandleKinectPlayer()
    {
        avatarPosition = (Avatar.userPosition.x * ((ConstInfo.lineWidth * 3) / 1920) + ConstInfo.tileX);
        if (avatarPosition < 107)
        {
            HandlePlayerLocation(PlayerLocation.Left);
        }
        else if (avatarPosition > 121)
        {
            HandlePlayerLocation(PlayerLocation.Right);
        }
        else
        {
            HandlePlayerLocation(PlayerLocation.Center);
        }
    }
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && speed < 60)
            speed += 5;
        else if (Input.GetKeyDown(KeyCode.DownArrow) && speed > 0)
            speed -= 5;
        else if ((Input.GetKeyDown(KeyCode.LeftArrow) && curLocation != PlayerLocation.Left))
            HandlePlayerLocation(PlayerLocation.Left);
        else if ((Input.GetKeyDown(KeyCode.RightArrow) && curLocation != PlayerLocation.Right))
            HandlePlayerLocation(PlayerLocation.Right);
        else if (Input.GetKeyDown(KeyCode.F) && curLocation != PlayerLocation.Center)
            HandlePlayerLocation(PlayerLocation.Center);
        else if (Input.GetKeyDown(KeyCode.Space) && !IsPlayerActing())
            isJumping = true;
        else if (Input.GetKeyDown(KeyCode.LeftControl) && !IsPlayerActing())
            isPunching = true;
    }

    void SpeedUpdate(float speed)
    {
        animator.SetFloat("speed", speed);
    }

    bool IsPlayerActing()
    {
        if (isStumbling || isJumping || isPunching)
            return true;
        return false;
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
    }



    // 플레이어 점프 애니메이션 (점프 중 펀치 가능)
    void HandlePlayerJumping()
    {
        animator.SetBool("isJumping", true);
        collider.center = (new Vector3(ConstInfo.originalColliderX, ConstInfo.jumpingColliderY, ConstInfo.originalColliderZ));
    }

    // 플레이어 발걸림 애니메이션 (발걸림 도중 다른 동작 불가)
    public void HandlePlayerStumbling()
    {
        animator.SetBool("isStumbling", true);
        speed = 5;
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
            curLocation = PlayerLocation.Left;
            transform.position = new Vector3(100, ConstInfo.tileY, ConstInfo.tileZ);
        }
        else if (MovedLocation == PlayerLocation.Center)
        {
            curLocation = PlayerLocation.Center;
            transform.position = new Vector3(114, ConstInfo.tileY, ConstInfo.tileZ);
        }
        else
        {
            curLocation = PlayerLocation.Right;
            transform.position = new Vector3(128, ConstInfo.tileY, ConstInfo.tileZ);
        }
        highlightTiles.Highlight(curLocation);
    }
    // 점프 상태 초기화 (초기화 시 펀치 상태도 초기화)
    public void InitialJumpState()
    {
        isJumping = false;
        isPunching = false;
        jumpTimer = 0;
        collider.center = (new Vector3(ConstInfo.originalColliderX, ConstInfo.originalColliderY, ConstInfo.originalColliderZ));
        animator.SetBool("isJumping", false);
    }

    // 발걸림 상태 초기화
    public void InitialStumbleState()
    {
        isStumbling = false;
        stumbleTimer = 0;
        animator.SetBool("isStumbling", false);
    }

    // 펀치 상태 초기화
    public void InitialPunchState()
    {
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
            if (jumpTimer >= ConstInfo.actionTimer)
                InitialJumpState();
        }
        if (isStumbling)
        {
            stumbleTimer += Time.deltaTime;
            if (stumbleTimer >= ConstInfo.actionTimer)
                InitialStumbleState();
        }
        if (isPunching)
        {
            punchTimer += Time.deltaTime;
            if (punchTimer >= ConstInfo.actionTimer)
                InitialPunchState();
        }
    }
    
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Heart Tile")
        {
            hp++;
        }
        else if (col.gameObject.tag == "Hurdle Tile" || col.gameObject.tag == "Trap Tile")
        {
            isStumbling = true;
            combo = 0;
            hp -= 10;
            HandlePlayerStumbling();
        }
        else if(col.gameObject.tag == "Balloon Tile")
        {
            if (isPunching)
            {
                col.gameObject.GetComponent<Balloon>().GoAway();
            }
        }
    }
}