﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleDamage : MonoBehaviour
{
    GameObject player;
    GameObject monster;

    public bool monsterAttack;
    public bool playerAttack;
    public Canvas blood;

    Animator animator;
    Animator anim;

    public float timer = 0;

    public GameObject camera;

    public GameObject punchNode;
    public GameObject kickNode;
    public GameObject canvas;

    public float punchTime;
    public float kickTime;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        monster = GameObject.Find("ScareCrow01");
        blood.gameObject.SetActive(false);


        monsterAttack = false;
        playerAttack = false;

        animator = player.GetComponent<Animator>();
        anim = monster.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        monsterAttack = MonsterAnim.GetMonsterState();
        playerAttack = PlayerAnim.GetPlayerState();
        timer += Time.deltaTime;


        punchTime = punchNode.GetComponent<RectTransform>().rect.width;
        kickTime = kickNode.GetComponent<RectTransform>().rect.width;
        

        if (timer > 1)
        {
            AttackHandler();
        }
    }
    public bool ComparePosition()
    {
        if (player.transform.position.x == monster.transform.position.x)
            return true;
        else
            return false;
    }
    void AttackHandler()
    {
        if (monsterAttack)
        {
            if (ComparePosition())
            {
                Debug.Log("Monster Attack!");
                animator.SetTrigger("hit");
                StartCoroutine(EffectHanlder());
                PlayerHealthbarHandler.SetHealthBarValue(PlayerHealthbarHandler.GetHealthBarValue() - 0.1f);
                StartCoroutine(HandleHitAnim());
            }

            timer = 0;
        }
        if (playerAttack)
        {
            if (ComparePosition())
            {
                Debug.Log("Player Attack!");
                if (PlayerAnim.KillMonster() == true)
                {
                    HealthBarHandler.SetHealthBarValue(0);
                }
                if (PlayerAnim.GetPunchState() == true)
                {

                    if (punchTime <= 200)
                    {
                        HealthBarHandler.SetHealthBarValue(HealthBarHandler.GetHealthBarValue() - 0.1f);
                        HealthBarHandler.SetHealthBarValue(HealthBarHandler.GetHealthBarValue() - 0.1f);
                        anim.SetTrigger("damaged");
                    }
                    else if (200 < punchTime && punchTime <= 240)
                    {
                        HealthBarHandler.SetHealthBarValue(HealthBarHandler.GetHealthBarValue() - 0.1f);
                        anim.SetTrigger("damaged");
                    }
                }
                if (PlayerAnim.GetKickState() == true)
                {

                    if (kickTime <= 200)
                    {
                        HealthBarHandler.SetHealthBarValue(HealthBarHandler.GetHealthBarValue() - 0.1f);
                        HealthBarHandler.SetHealthBarValue(HealthBarHandler.GetHealthBarValue() - 0.1f);
                        anim.SetTrigger("damaged");


                    }
                    else if (200 < kickTime && kickTime <= 240)
                    {
                        HealthBarHandler.SetHealthBarValue(HealthBarHandler.GetHealthBarValue() - 0.1f);
                        anim.SetTrigger("damaged");
                    }

                }
            }
            StartCoroutine(HandleHitAnim());

            timer = 0;
        }
    }
    private IEnumerator HandleHitAnim()
    {
        yield return new WaitForSeconds(1f);
        animator.ResetTrigger("hit");
        anim.ResetTrigger("damaged");
    }
    private IEnumerator EffectHanlder()
    {
        blood.gameObject.SetActive(true);
        camera.GetComponent<ShakeBehavior>().TriggerShake();
        yield return new WaitForSeconds(2f);
        blood.gameObject.SetActive(false);

    }


}
