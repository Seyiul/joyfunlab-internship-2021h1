﻿using System.Collections;
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
    public static bool jump;
    public static bool kick;

    // 바닥 UI 타일
    public GameObject leftFloorTile;
    public GameObject centerFloorTile;
    public GameObject rightFloorTile;



    public BattlePlayerLocation curLocation;
    BattleHighlight highlightTiles;

    //Kinect
    public bool kinectState;
    public float avatarPosition;


    void Awake() { instance = this; }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        attack = false;
        jump = false;
        kick = false;

        avatarPosition = 0;

        kinectState = GameManager.instance.GetKinectState();
        curLocation = BattlePlayerLocation.Center;
        highlightTiles = GameObject.Find("HighlightTiles").GetComponent<BattleHighlight>();
        
    }

    // Update is called once per frame
    void Update()
    {
        hp = (float)PlayerHealthbarHandler.GetHealthBarValue() * 100;
        monsterHp = (float)HealthBarHandler.GetHealthBarValue() * 100;

        avatarPosition = (Avatar.userPosition.x * ((ConstInfo.lineWidth * 3) / 1920) + ConstInfo.tileX);

        HandleGame(hp, monsterHp);
        

    }
    void HandleFloorTileHighlight()
    {
        UnselectFloorTile();
        SelectFloorTile();
    }
    void UnselectFloorTile()
    {
        FloorTexture.setFloorTileTexture(leftFloorTile, FloorTexture.FloorTileUnSelected);
        FloorTexture.setFloorTileTexture(centerFloorTile, FloorTexture.FloorTileUnSelected);
        FloorTexture.setFloorTileTexture(rightFloorTile, FloorTexture.FloorTileUnSelected);
    }

    void SelectFloorTile()
    {
        if (kinectState)
        {
            if (avatarPosition < 107)
            {
                FloorTexture.setFloorTileTexture(leftFloorTile, FloorTexture.FloorTileSelected);
            }
            else if (avatarPosition > 121)
            {
                FloorTexture.setFloorTileTexture(rightFloorTile, FloorTexture.FloorTileSelected);

            }
            else
            {
                FloorTexture.setFloorTileTexture(centerFloorTile, FloorTexture.FloorTileSelected);

            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                FloorTexture.setFloorTileTexture(leftFloorTile, FloorTexture.FloorTileSelected);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                FloorTexture.setFloorTileTexture(centerFloorTile, FloorTexture.FloorTileSelected);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                FloorTexture.setFloorTileTexture(rightFloorTile, FloorTexture.FloorTileSelected);
            }
        }
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
        if (kinectState)
        {
            HandlePlayerKinect();
        }
        else
        {
            HandlePlayerPosition();
            HandlePlayerAction();
        }

    }
    void HandlePlayerKinect()
    {
        if (avatarPosition < 107)
        {
            HandlePlayerLocation(BattlePlayerLocation.Left);
            HandleFloorTileHighlight();
        }
        else if (avatarPosition > 121)
        {
            HandlePlayerLocation(BattlePlayerLocation.Right);
            HandleFloorTileHighlight();

        }
        else
        {
            HandlePlayerLocation(BattlePlayerLocation.Center);
            HandleFloorTileHighlight();

        }
    }
    void HandlePlayerPosition()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HandlePlayerLocation(BattlePlayerLocation.Left);
            HandleFloorTileHighlight();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            HandlePlayerLocation(BattlePlayerLocation.Center);
            HandleFloorTileHighlight();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HandlePlayerLocation(BattlePlayerLocation.Right);
            HandleFloorTileHighlight();
        }
        
    }
    public void HandlePlayerLocation(BattlePlayerLocation MovedLocation)
    {
        if (MovedLocation == BattlePlayerLocation.Left)
        {
            curLocation = BattlePlayerLocation.Left;
            transform.position = new Vector3(100, transform.position.y, transform.position.z);
        }
        else if (MovedLocation == BattlePlayerLocation.Center)
        {
            curLocation = BattlePlayerLocation.Center;
            transform.position = new Vector3(114, transform.position.y, transform.position.z);
        }
        else
        {
            curLocation = BattlePlayerLocation.Right;
            transform.position = new Vector3(128, transform.position.y, transform.position.z);
        }
        highlightTiles.Highlight(curLocation);
        
    }
    void HandlePlayerAction()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            animator.SetTrigger("kick");
            attack = true;
            kick = true;
            StartCoroutine(HandleAttackTimer());
        }


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger("punch");
            attack = true;
            StartCoroutine(HandleAttackTimer());
        }


        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            animator.SetTrigger("jump");
            jump = true;
            StartCoroutine(HandleJumpTimer());

        }

    }
    private IEnumerator HandleAttackTimer()
    {
        yield return new WaitForSeconds(1f);
        attack = false;
        kick = false;

    }
    private IEnumerator HandleJumpTimer()
    {
        yield return new WaitForSeconds(2f);
        jump = false;
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
    public static bool GetPlayerJumpState()
    {
        if (jump == true)
        {
            return true;
        }
        else
            return false;
    }
    //kick 발동시, 1/10 확률로 즉사
    public static bool KillMonster()
    {
        if (kick == true)
        {
            int random = Random.Range(1, 11);
            if (random == 1)
            {
                Debug.Log("KIIILLLLLl");
                return true;
            }
            else
                return false;
        }
        else
            return false;

    }

}