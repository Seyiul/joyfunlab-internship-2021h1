using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public static PlayerAnim instance;

    Animator animator;

    public float hp;
    public float monsterHp;

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
        hp = (float)PlayerHealthbarHandler.GetHealthBarValue() * 100;
        monsterHp = (float)HealthBarHandler.GetHealthBarValue() * 100;
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
        HandlePlayerPosition();
        HandlePlayerAction();


    }
    void HandlePlayerPosition()
    {

        attack = false;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position = new Vector3(101, transform.position.y, transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position = new Vector3(114, transform.position.y, transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position = new Vector3(127, transform.position.y, transform.position.z);
        }
    }
    void HandlePlayerAction()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            animator.SetTrigger("kick");
            attack = true;
        }


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger("punch");
            attack = true;
        }


        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            animator.SetTrigger("jump");
        }

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
   

}
