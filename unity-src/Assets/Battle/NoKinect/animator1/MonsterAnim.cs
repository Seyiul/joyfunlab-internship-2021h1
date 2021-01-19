using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnim : MonoBehaviour
{
    public static MonsterAnim instance;

    Animator animator;

    public float hp;
    public float playerHp;
    public int randomNumber;


    public float timer = 0;

    public static bool attack;


    void Awake() { instance = this; }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        attack = false;

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        playerHp = (float)PlayerHealthbarHandler.GetHealthBarValue() * 100;
        hp = (float)HealthBarHandler.GetHealthBarValue() * 100;
        //isPunching = animator.GetBool("punch");
        //isKicking = animator.GetBool("kick");
        if (timer > 3)
        {
            HandleGame(playerHp, hp);
        }
        else
        {
            if (hp == 0)
            {
                animator.SetBool("die", true);

            }
            else if (playerHp == 0)
            {
                animator.SetBool("victory", true);
            }
            else
            {
                if(timer>1)
                    MonsterActionInitialize();
            }
        }


    }
    void HandleGame(float playerHp, float hp)
    {
        if (hp == 0)
        {
            animator.SetBool("die", true);

        }
        else if (playerHp == 0)
        {
            animator.SetBool("victory", true);
        }
        else
        {
            HandleMonster();
            HandleMonsterAction();

        }

    }
    void HandleMonsterAction()
    {
        attack = true;
        int rn = Random.Range(1, 3);
        switch (rn)
        {
            case 1:
                animator.SetTrigger("kick");
                break;

            default:
                animator.SetTrigger("punch");
                break;
        }
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
    void MonsterActionInitialize()
    {
        animator.ResetTrigger("kick");
        animator.ResetTrigger("punch");
        attack = false;
    }

    public static bool GetMonsterState()
    {
        if (attack == true)
        {
            return true;
        }
        else
            return false;

    }


}