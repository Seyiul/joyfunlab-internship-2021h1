using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattlePlayer : MonoBehaviour
{
    public static BattlePlayer instance;
    
    Animator animator;
       
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        InitialValues();
        if(PlayerHealthbarHandler.GetHealthBarValue()==0)
        {
            animator.SetBool("die", true);
        }
        else
        {
            HandlePlayer();
        }
    }
    void InitialValues()
    {
        animator.SetBool("die", false);
        animator.SetBool("isJump", false);
        animator.SetBool("isKick", false);
        animator.SetBool("isPunch", false);
    }
    void HandlePlayer()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetBool("isJump", true);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetBool("isKick", true);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetBool("isPunch", true);
        }
        Vector3 temp = new Vector3(13.2f, 0, 0);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position -= temp;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += temp;
        }
    }
}