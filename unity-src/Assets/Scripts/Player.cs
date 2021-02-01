using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;

public enum PlayerLocation : int
{
    Left = -1,
    Center = 0,
    Right = 1
}

public class Player : MonoBehaviour
{
    public GameObject menu;

    // 인스턴스 변수 선언
    public static Player instance;

    // 애니매이터 변수 선언
    Animator animator;
    // 걸음 관련 변수 선언
    public bool leftUp, rightUp;
    public static float stepRecordTime;
    public static float decreaseSpeedTimer;
    public static List<float> steps;

    // 행동 관련 번수 선언
    public bool isJumping;
    public float jumpTimer;

    public bool isStumbling;
    public float stumbleTimer;

    public bool isPunching;
    public float punchTimer;

    public float speed;

    // 콤보, 체력 관련 변수, 상수 선언
    public int maxCombo;
    public int combo;
    public int hp;
    public int maxHp;
    public float time;
    public float playtime;
    public float avatarPosition;
    public PlayerLocation curLocation;

    public GameObject transition;
    public int afterBattle;

    BoxCollider collider;
    HighlightTiles highlightTiles;
    public GameCanvas gameCanvas;
    // 인스턴스 설정
    private void Awake() { instance = this;
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("afterBattle") == 1)
        {

            GameManager.instance.SetGameState(GameState.Game);

            highlightTiles = GameObject.Find("HighlightTiles").GetComponent<HighlightTiles>();
            animator = GetComponent<Animator>();
            collider = gameObject.GetComponent<BoxCollider>();
            time = PlayerPrefs.GetFloat("time");
            playtime = PlayerPrefs.GetFloat("playtime");
            hp = PlayerPrefs.GetInt("hp");
            maxHp = PlayerPrefs.GetInt("maxHp");
            speed = PlayerPrefs.GetFloat("speed");
            combo = PlayerPrefs.GetInt("combo");
            maxCombo = PlayerPrefs.GetInt("maxCombo");
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("afterBattle",0);

        }
        else
        {
            InitialValues();
            highlightTiles = GameObject.Find("HighlightTiles").GetComponent<HighlightTiles>();
            animator = GetComponent<Animator>();
            collider = gameObject.GetComponent<BoxCollider>();
            gameCanvas = GameObject.Find("GameCanvas").GetComponent<GameCanvas>();
        }
    }
    public void InitialAll()
    {
        InitialValues();
        if (GameManager.instance.curTile != null && GameManager.instance.nextTile != null)
        {
            Destroy(GameManager.instance.curTile);
            Destroy(GameManager.instance.nextTile);
        }
    }
    // 변수 초기화
    public void InitialValues()
    {
        PlayerPrefs.DeleteAll();

        leftUp = false;
        rightUp = false;

        stepRecordTime = 0;
        decreaseSpeedTimer = 0;

        isJumping = false;
        jumpTimer = 0;
        isStumbling = false;
        stumbleTimer = 0;
        isPunching = false;
        punchTimer = 0;
        speed = 0;
        combo = 0;
        maxCombo = 0;
        avatarPosition = 0;
        curLocation = PlayerLocation.Center;
        //PlayerPrefs.SetInt("afterBattle", 0);

        time = 60;
        hp = 50;
        maxHp = 100;
        playtime = time;
        InitialStepRecords();
    }
    void Ondestroy()
    {
        PlayerPrefs.SetInt("afterBattle", 0);

    }
    // 걸음 시간 리스트 초기화 (0)
    public static void InitialStepRecords()
    {
        steps = Enumerable.Repeat<float>(0, 3).ToList();
    }

    // 걸음 시간 측정 (+ fixedDeltaTime)
    private void FixedUpdate()
    {
        stepRecordTime += Time.fixedDeltaTime;
        decreaseSpeedTimer += Time.fixedDeltaTime;
    }
    void Update()
    {
        if (GameManager.instance.GetGameState() == GameState.Game)
        {
            time -= Time.deltaTime;
            HandleInput();
            //게임의 상태가 변화하면 속도를 업데이트
            SpeedUpdate(speed);
            HandlePlayerAction();
            gameCanvas.DisplayCombo();
            gameCanvas.DisplayTime();
            gameCanvas.DisplayHp();
            if (time <= 0 || hp <= 0)
            {
                GameManager.instance.SetGameState(GameState.Result);
                PlayerPrefs.DeleteAll();

            }
            if (combo > maxCombo)
                maxCombo = combo;

        }
        else
        {
            //현재 속도를 저장하기 위해서 speed 변수를 초기화하지 않음
            SpeedUpdate(0);
            InitialJumpState();
            InitialPunchState();
            InitialStumbleState();
        }
        


    }
    void HandleKinectPlayer()
    {
        avatarPosition = (Avatar.userPosition.x * ((ConstInfo.lineWidth * 3) / 1920) + ConstInfo.tileX);
        //ispunching
        if ((Avatar.userPositionLeftHand.z > Avatar.userPositionHead.z + Avatar.distanceHandElbow * 5 / 3) ||
             (Avatar.userPositionRightHand.z > Avatar.userPositionHead.z + Avatar.distanceHandElbow * 5 / 3))
        { isPunching = true; }
        else if ((Avatar.userPositionLeftFoot.y > ConstInfo.stepHeight) &&
            (Avatar.userPositionRightFoot.y > ConstInfo.stepHeight)&&Mathf.Abs(Avatar.userPositionLeftFoot.y-Avatar.userPositionRightFoot.y)<ConstInfo.jumpHeight)
        { isJumping = true; }
        /*
        //isKicking
        
        if((Avatar.userPositionLeftFoot.z > Avatar.userPositionHead.z + Avatar.distanceFootKnee*5/3) ||
             (Avatar.userPositionRightFoot.z > Avatar.userPositionHead.z + Avatar.distanceFootKnee * 5 / 3)) 
        { isKicking = true; }
         else { isKicking = false; }
        */
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
    // 결음 기록 조건 만족 시 함수 호출
    void HandleSteps()
    {
        if (decreaseSpeedTimer >= 2 && !Player.instance.isJumping)
        {
            decreaseSpeedTimer = 0;
            steps.Add(0);
            steps.RemoveAt(0);
        }
        //왼발 오른발 여부
        if ((Avatar.userPositionLeftFoot.y > ConstInfo.stepHeight && Avatar.userPositionRightFoot.y < ConstInfo.stepHeight) && stepRecordTime != 0)
            leftUp = true;
        else if ((Avatar.userPositionRightFoot.y > ConstInfo.stepHeight && Avatar.userPositionLeftFoot.y < ConstInfo.stepHeight) && stepRecordTime != 0)
            rightUp = true;
        //왼발과 오른발이 leftup rightup 상황에서만 step인식
        if (leftUp && rightUp)
        {
            HandleStep();
            leftUp = false;
            rightUp = false;
        }
        speed = 5 + steps.Average();
        if (speed > 60)
            speed = 60;
    }

    // 걸음시간 기록 및 초기화
    void HandleStep()
    {
        steps.Add(10/stepRecordTime);
        steps.RemoveAt(0);
        stepRecordTime = 0;
        decreaseSpeedTimer = 0;
    }
    void HandleInput()
    {
        if (GameManager.instance.GetKinectState() == true)
        {
            HandleKinectPlayer();
            HandleSteps();
        }
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
            transform.position = new Vector3(100, ConstInfo.playerY, ConstInfo.playerZ);
        }
        else if (MovedLocation == PlayerLocation.Center)
        {
            curLocation = PlayerLocation.Center;
            transform.position = new Vector3(114, ConstInfo.playerY, ConstInfo.playerZ);
        }
        else
        {
            curLocation = PlayerLocation.Right;
            transform.position = new Vector3(128, ConstInfo.playerY, ConstInfo.playerZ);
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
            if(hp < maxHp)
                hp++;
            gameCanvas.DisplayHpIncrease();
        }
        else if (col.gameObject.tag == "Hurdle Tile" || col.gameObject.tag == "Trap Tile")
        {
            isStumbling = true;
            combo = 0;
            if (hp / 2 > 10)
                hp -= (int)hp / 2;
            else
                hp -= 10;
            gameCanvas.DisplayHpDecrease();
            HandlePlayerStumbling();
        }
        else if (col.gameObject.tag == "Balloon Tile")
        {
            if (isPunching)
            {
                combo++;
                time += 3;
                playtime += 3;
                col.gameObject.GetComponent<Balloon>().GoAway();
                gameCanvas.DisplayTimeIncrease();
            }
        }
        else if (col.gameObject.tag == "Pass Tile")
        {
            combo++;
        }
        else if (col.gameObject.tag == "Battle Tile")
        {

            PlayerPrefs.SetFloat("time", time);
            PlayerPrefs.SetFloat("playtime", playtime);
            PlayerPrefs.SetInt("hp", hp);
            PlayerPrefs.SetInt("maxHp", maxHp);
            PlayerPrefs.SetFloat("speed", speed);
            PlayerPrefs.SetInt("combo", combo);
            PlayerPrefs.SetInt("maxCombo", maxCombo);
            PlayerPrefs.SetInt("afterBattle", 1);
            GameManager.instance.SetGameState(GameState.Battle);
            transition.GetComponent<Animator>().SetBool("animateIn", true);
            StartCoroutine(SceneLoad());
        }

    }
    IEnumerator SceneLoad()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("BattleScene");
    }
}

