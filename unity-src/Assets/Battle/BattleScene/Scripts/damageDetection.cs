using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageDetection : MonoBehaviour
{
    GameObject player;
    GameObject monster;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        monster = GameObject.Find("Robot Kyle");
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x == monster.transform.position.x)
        {
            if(MonsterMovement.instance.isPunching || MonsterMovement.instance.isKicking)
            {
                if(playerBattle.instance.isJumping == false)
                    GetDamaged();
            }
        }
    }
    void GetDamaged()
    {
        PlayerHealthbarHandler.SetHealthBarValue(PlayerHealthbarHandler.GetHealthBarValue() - 0.003f);
    }
}
