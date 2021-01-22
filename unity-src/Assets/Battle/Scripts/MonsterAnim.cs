﻿using System.Collections;
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
 
        //Landing time = 3.5f
        if (timer > 3.5)
        {
            HandleGame(playerHp, hp);
        }
        else
        {
            if (hp == 0)
            {
                animator.SetBool("die", true);
                attack = false;

            }
            else if (playerHp == 0)
            {
                animator.SetBool("victory", true);
                attack = false;
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
        StartCoroutine(MonsterActionInitialize());
        timer = 0;
    }
    IEnumerator MonsterActionInitialize()
    {
        yield return new WaitForSeconds(1f);
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