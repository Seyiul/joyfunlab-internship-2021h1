using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattlePlayerLocation : int
{
    Left = -1,
    Center = 0,
    Right = 1
}
public class PlayerAnim : MonoBehaviour
{
    public static PlayerAnim instance;

    Animator animator;

    public float hp;
    public float monsterHp;

    public float timer = 0;

    public static bool attack;
    public static bool jump;
    public static bool kick;

    public BattlePlayerLocation curLocation;
    BattleHighlight highlightTiles;

    //Kinect
    public bool kinectState;
    public float AvatarPosition;


    void Awake() { instance = this; }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        attack = false;
        jump = false;
        kick = false;

        AvatarPosition = 0;

        kinectState = GameManager.instance.GetKinectState();
        curLocation = BattlePlayerLocation.Center;
        highlightTiles = GameObject.Find("HighlightTiles").GetComponent<BattleHighlight>();

    }

    // Update is called once per frame
    void Update()
    {
        hp = (float)PlayerHealthbarHandler.GetHealthBarValue() * 100;
        monsterHp = (float)HealthBarHandler.GetHealthBarValue() * 100;

        AvatarPosition = (Avatar.userPosition.x * ((ConstInfo.lineWidth * 3) / 1920) + ConstInfo.tileX);

        HandleGame(hp, monsterHp);


    }
    void HandleGame(float hp, float monsterHp)
    {
        if (monsterHp == 0)
        {
            animator.SetBool("victory", true);
        }
        else if (hp == 0)
        {
            animator.SetBool("defeat", true);

        }
        else
            HandlePlayer();
    }
    void HandlePlayer()
    {
        if (kinectState)
        {
            HandlePlayerKinect();
        }
        else
        {
            HandlePlayerPosition();
            HandlePlayerAction();
        }

    }
    void HandlePlayerKinect()
    {
        if (AvatarPosition < -100)
        {
            HandlePlayerLocation(BattlePlayerLocation.Left);
        }
        else if (AvatarPosition > 121)
        {
            HandlePlayerLocation(BattlePlayerLocation.Right);

        }
        else
        {
            HandlePlayerLocation(BattlePlayerLocation.Center);

        }
    }
    void HandlePlayerPosition()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HandlePlayerLocation(BattlePlayerLocation.Left);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            HandlePlayerLocation(BattlePlayerLocation.Center);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HandlePlayerLocation(BattlePlayerLocation.Right);
        }
        highlightTiles.Highlight(curLocation);
    }
    public void HandlePlayerLocation(BattlePlayerLocation MovedLocation)
    {
        if (MovedLocation == BattlePlayerLocation.Left)
        {
            curLocation = BattlePlayerLocation.Left;
            transform.position = new Vector3(100, transform.position.y, transform.position.z);
        }
        else if (MovedLocation == BattlePlayerLocation.Center)
        {
            curLocation = BattlePlayerLocation.Center;
            transform.position = new Vector3(114, transform.position.y, transform.position.z);
        }
        else
        {
            curLocation = BattlePlayerLocation.Right;
            transform.position = new Vector3(128, transform.position.y, transform.position.z);
        }
        highlightTiles.Highlight(curLocation);
    }
    void HandlePlayerAction()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            animator.SetTrigger("kick");
            attack = true;
            kick = true;
            StartCoroutine(HandleAttackTimer());
        }


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger("punch");
            attack = true;
            StartCoroutine(HandleAttackTimer());
        }


        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            animator.SetTrigger("jump");
            jump = true;
            StartCoroutine(HandleJumpTimer());

        }

    }
    private IEnumerator HandleAttackTimer()
    {
        yield return new WaitForSeconds(1f);
        attack = false;
        kick = false;

    }
    private IEnumerator HandleJumpTimer()
    {
        yield return new WaitForSeconds(2f);
        jump = false;
    }
    public static bool GetPlayerState()
    {
        if (attack == true)
        {
            return true;
        }
        else
            return false;

    }
    public static bool GetPlayerJumpState()
    {
        if (jump == true)
        {
            return true;
        }
        else
            return false;
    }
    //kick 발동시, 1/10 확률로 즉사
    public static bool KillMonster()
    {
        if (kick == true)
        {
            int random = Random.Range(1, 11);
            if (random == 1)
            {
                Debug.Log("KIIILLLLLl");
                return true;
            }
            else
                return false;
        }
        else
            return false;

    }

}