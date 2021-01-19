using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public static MonsterMovement instance;

    Animator animator;

    public RuntimeAnimatorController animDie;
    public RuntimeAnimatorController animWin;
    public RuntimeAnimatorController animIdle;
    public RuntimeAnimatorController animPunch;
    public RuntimeAnimatorController animKick;


    public float hp;
    public float playerHp;
    public int randomNumber;

    public float timer = 0;

    public bool isPunching;
    public float punchTimer;

    public bool isKicking;
    public float kickTimer;


    void Awake() { instance = this; }
    // Start is called before the first frame update
    void Start()
    {
        isPunching = false;
        punchTimer = 0;
        animator = GetComponent<Animator>();
        GetComponent<Animation>().wrapMode = WrapMode.Loop;

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        playerHp = (float)PlayerHealthbarHandler.GetHealthBarValue() * 100;
        hp = (float)HealthBarHandler.GetHealthBarValue() * 100;

        HandleGame(playerHp, hp);


    }
    void HandleGame(float playerHp, float hp)
    {
        if (hp == 0)
        {
            animator.runtimeAnimatorController = animDie;

        }
        else if (playerHp == 0)
        {
            animator.runtimeAnimatorController = animWin;
        }
        else
        {
            if (timer > 1.3f)
                {
                    HandleMonster();
                    HandleMonsterAction();
                }
        }

    }
    void HandleMonsterAction()
    {
        int rn = Random.Range(1, 4);
        switch (rn)
        {
            case 1:
                isPunching = true;
                isKicking = false;
                break;
            case 2:
                isKicking = true;
                isPunching = false;
                break;
            default:
                isPunching = false;
                isKicking = false;
                break;
        }
        MonsterAction();
    }
    void HandleMonster()
    {
        HandleMonsterPosition();
    }
    void HandleMonsterPosition()
    {

        randomNumber = Random.Range(1, 4);
        switch (randomNumber)
        {
            case 1:
                transform.position = new Vector3(101, transform.position.y, transform.position.z);
                break;
            case 2:
                transform.position = new Vector3(114, transform.position.y, transform.position.z);
                break;
            default:
                transform.position = new Vector3(127, transform.position.y, transform.position.z);
                break;
        }
        timer = 0;


    }
    void MonsterAction()
    {
        if (isPunching || isKicking)
        {
            if(isPunching)
                animator.runtimeAnimatorController = animPunch as RuntimeAnimatorController;
            else
            {
                if(isKicking)
                    animator.runtimeAnimatorController = animKick;
            }
            HandlePlayerActionTimer();
        }
        else
            animator.runtimeAnimatorController = animIdle as RuntimeAnimatorController;
       
    }
    void HandlePlayerActionTimer()
    {
        if (isPunching)
        {
            punchTimer += Time.deltaTime;
            if (punchTimer >= 1.3f)
                InitialPunchState();
        }
        if (isKicking)
        {
            kickTimer += Time.deltaTime;
            if (kickTimer >= 1.2f)
                InitialKickState();
        }
    }
    public void InitialKickState()
    {
        animator.runtimeAnimatorController = null;
        isKicking = false;
        kickTimer = 0;
    }
    public void InitialPunchState()
    {
        animator.runtimeAnimatorController = null;
        isPunching = false;
        punchTimer = 0;
    }
    
}
