using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleDamage : MonoBehaviour
{
    GameObject player;
    GameObject monster;

    public float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        monster = GameObject.Find("Robot Kyle");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 2)
        {
            ComparePosition();
        }
    }
    void ComparePosition()
    {
        if (player.transform.position.x == monster.transform.position.x)
        {
            MonsterAttackDetection();
            PlayerAttackDetection();
        }
    }

    void MonsterAttackDetection()
    {
        if (MonsterAnim.GetMonsterState()==true)
        {
            Debug.Log("Monster Attack!");
            PlayerHealthbarHandler.SetHealthBarValue(PlayerHealthbarHandler.GetHealthBarValue() - 0.11f);
            timer = 0;
        }
    }
    void PlayerAttackDetection()
    {
        if (PlayerAnim.GetPlayerState() == true)
        {
            Debug.Log("Player Attack!");
            HealthBarHandler.SetHealthBarValue(HealthBarHandler.GetHealthBarValue() - 0.11f);
            timer = 0;
        }
    }

}
