using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayer : MonoBehaviour
{
    public static MenuPlayer instance;

    // 애니메이션 관련 변수
    RuntimeAnimatorController animIdle;
    Animator animator;

    void Awake() { instance = this; }

    void Start()
    {
        animator = GetComponent<Animator>();
        GetComponent<Animation>().wrapMode = WrapMode.Loop;
        animIdle = Resources.Load("3DResources/AnimationControllers/JoyRunIdle") as RuntimeAnimatorController;
    }

    void Update() { HandleMenuPlayer(); }

    // 메뉴 플레이어 설정
    void HandleMenuPlayer() {
        HandleMenuPlayerPosition();
        HandleMenuPlayerAnimtaion();
    }

    // 메뉴 플레이어 위치 조정 (유저가 없을 경우 중앙에 위치)
    void HandleMenuPlayerPosition() 
    { 
        if (Avatar.GetUserValid() && Setting.GetCurrentAnimationState() == AnimationState.Kinect)
            transform.position = new Vector3(Avatar.userPosition.x * (ConstInfo.runningTrackWidth / ConstInfo.floorUICanvasWidth) + ConstInfo.center,
                ConstInfo.playerInitialPositionY, ConstInfo.playerInitialPositionZ);
        else
            transform.position = new Vector3(ConstInfo.center, ConstInfo.playerInitialPositionY, ConstInfo.playerInitialPositionZ);
    }

    // 메뉴 플레이어 애니메이션 설정
    void HandleMenuPlayerAnimtaion()
    {
        if (Setting.GetCurrentAnimationState() == AnimationState.Animation)
            animator.runtimeAnimatorController = animIdle;
        else if (Setting.GetCurrentAnimationState() == AnimationState.Kinect)
            animator.runtimeAnimatorController = null;
    }
}
