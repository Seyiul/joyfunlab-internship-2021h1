using System.Collections;
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

    public GameObject leftPunch;
    public GameObject rightPunch;
    public GameObject leftKick;
    public GameObject rightKick;



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

        leftPunch.SetActive(false);
        rightPunch.SetActive(false);
        leftKick.SetActive(false);
        rightKick.SetActive(false);

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
            StartCoroutine(ShowNode());
            timer = 0;
        }
        if (playerAttack)
        {
            if (ComparePosition())
            {

                //기본공격
                if (PlayerAnim.GetPunchState() == true)
                {
                    if (leftPunch.activeSelf || rightPunch.activeSelf)
                    {
                        if (punchTime <= 200)
                        {
                            HealthBarHandler.SetHealthBarValue(HealthBarHandler.GetHealthBarValue() - 0.1f);
                            HealthBarHandler.SetHealthBarValue(HealthBarHandler.GetHealthBarValue() - 0.1f);
                            anim.SetTrigger("damaged");
                        }
                        else if (200 < punchTime && punchTime <= 300)
                        {
                            HealthBarHandler.SetHealthBarValue(HealthBarHandler.GetHealthBarValue() - 0.1f);
                            anim.SetTrigger("damaged");
                        }

                    }


                }
                //회전공격
                if (PlayerAnim.GetKickState() == true)
                {
                    if (leftKick.activeSelf || rightKick.activeSelf)
                    {
                        if (PlayerAnim.KillMonster() == true)
                        {
                            HealthBarHandler.SetHealthBarValue(0);
                        }

                        if (kickTime <= 200)
                        {
                            HealthBarHandler.SetHealthBarValue(HealthBarHandler.GetHealthBarValue() - 0.1f);
                            HealthBarHandler.SetHealthBarValue(HealthBarHandler.GetHealthBarValue() - 0.1f);
                            anim.SetTrigger("damaged");


                        }
                        else if (200 < kickTime && kickTime <= 300)
                        {
                            HealthBarHandler.SetHealthBarValue(HealthBarHandler.GetHealthBarValue() - 0.1f);
                            anim.SetTrigger("damaged");
                        }

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
    private IEnumerator ShowNode()
    {
        yield return new WaitForSeconds(1f);
        int rn = Random.Range(1, 5);
        switch (rn)
        {
            case 1:
                leftPunch.SetActive(true);
                rightPunch.SetActive(false);
                leftKick.SetActive(false);
                rightKick.SetActive(false);
                break;
            case 2:
                leftPunch.SetActive(false);
                rightPunch.SetActive(false);
                leftKick.SetActive(true);
                rightKick.SetActive(false);
                break;
            case 3:
                leftPunch.SetActive(false);
                rightPunch.SetActive(false);
                leftKick.SetActive(false);
                rightKick.SetActive(true);
                break;
            default:
                leftPunch.SetActive(false);
                rightPunch.SetActive(true);
                leftKick.SetActive(false);
                rightKick.SetActive(false);
                break;

        }
    }


}
