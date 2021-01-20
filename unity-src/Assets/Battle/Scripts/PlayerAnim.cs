using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattlePlayerLocation : int
{
    Left = -1,
    Center = 0,
    Right = 1
}
public class PlayerAnim : MonoBehaviour
{
    public static PlayerAnim instance;

    Animator animator;

    public float hp;
    public float monsterHp;

    public float timer = 0;

    public static bool attack;

    public BattlePlayerLocation curLocation;
    BattleHighlight highlightTiles;



    void Awake() { instance = this; }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        attack = false;
        curLocation = BattlePlayerLocation.Center;
        highlightTiles = GameObject.Find("HighlightTiles").GetComponent<BattleHighlight>();


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

        if (timer > 1f)
        {
            attack = false;
        }


    }
    void HandlePlayerPosition()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            curLocation = BattlePlayerLocation.Left;
            transform.position = new Vector3(101, transform.position.y, transform.position.z);
            attack = false;

        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            curLocation = BattlePlayerLocation.Center;
            transform.position = new Vector3(114, transform.position.y, transform.position.z);
            attack = false;

        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            curLocation = BattlePlayerLocation.Right;
            transform.position = new Vector3(127, transform.position.y, transform.position.z);
            attack = false;

        }
        highlightTiles.Highlight(curLocation);
    }
    void HandlePlayerAction()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            animator.SetTrigger("kick");
            attack = true;
            timer = 0;
        }


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger("punch");
            attack = true;
            timer = 0;
        }


        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            attack = false;
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