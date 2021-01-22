using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleDamage : MonoBehaviour
{
    GameObject player;
    GameObject monster;

    public bool monsterAttack;
    public bool playerAttack;


    public float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        monster = GameObject.Find("Robot Kyle");

        monsterAttack = false;
        playerAttack = false;

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
                PlayerHealthbarHandler.SetHealthBarValue(PlayerHealthbarHandler.GetHealthBarValue() - 0.1f);
            }

            timer = 0;
        }
        else if (playerAttack)
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
                    HealthBarHandler.SetHealthBarValue(HealthBarHandler.GetHealthBarValue() - 0.1f);
                }
            }
            timer = 0;
        }
    }

    //void MonsterAttackDetection()
    //{
    //    if (MonsterAnim.GetMonsterState()==true)
    //    {
    //        if(PlayerAnim.GetPlayerJumpState() == false)
    //        {
    //            Debug.Log("Monster Attack!");
    //            PlayerHealthbarHandler.SetHealthBarValue(PlayerHealthbarHandler.GetHealthBarValue() - 0.1f);
    //            //timer = 0;
    //        }
    //        timer = 0;
    //    }
    //}
    //void PlayerAttackDetection()
    //{
        
    //    if (PlayerAnim.GetPlayerState() == true)
    //    {
    //        Debug.Log("Player Attack!");
    //        if (PlayerAnim.KillMonster() == true)
    //        {
    //            HealthBarHandler.SetHealthBarValue(0);
    //        }
    //        else
    //        {
    //            HealthBarHandler.SetHealthBarValue(HealthBarHandler.GetHealthBarValue() - 0.1f);
    //        }
    //        timer = 0;
    //    }
    //}

}
