using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Run : MonoBehaviour
{
    private bool isJump;
    private bool isWalk;
    private bool isIdle;
    public Text textbox;
    
    
    // Start is called before the first frame update
    void Start()
    {
        isJump = false;
        isWalk = false;
        isIdle = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleUI();
        HandleKeyboardGame();
    }
    void HandleKeyboardGame()
    {
        if (Input.GetKey(KeyCode.LeftAlt)) {
            isJump = true;
            isIdle = false;
            isWalk = false;
        }
        else if (Input.GetKey(KeyCode.LeftShift)){
            isWalk = true;
            isIdle = false;
            isJump = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow)&&Input.GetKey(KeyCode.RightArrow)){
            isIdle = true;
            isJump = false;
            isWalk = false;
        }
    }
    void HandleUI()
    {
        if(isJump == true)
            textbox.text = "Jump";
        if(isWalk == true)
            textbox.text = "Walk";
        if(isIdle == true)
            textbox.text = "Idle";
    }
}

