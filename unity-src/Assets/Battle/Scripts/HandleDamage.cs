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
                else
                {
                    anim.SetTrigger("damaged");
                    HealthBarHandler.SetHealthBarValue(HealthBarHandler.GetHealthBarValue() - 0.1f);
                }
            }
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
        yield return new WaitForSeconds(1.5f);
        blood.gameObject.SetActive(false);

    }


}
