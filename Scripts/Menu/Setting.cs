using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 애니메이션 설정 (0: 키넥트, 1: 애니메이션)
public enum AnimationState { Kinect = 0, Animation = 1 }

// 체력제한 설정 (0: 기본, 1: 무적)
public enum HpState { Normal = 0, Immortal = 1 }

// 시간제한 설정 (0: 기본 60초, 1: 시간제한없음)
public enum TimeState { Normal = 0, Infinite = 1 }



public static class Setting
{
    // ConstInfo 에 따른 초기설정값
    private static AnimationState currentAnimationState = ConstInfo.initialAnimationState;
    private static HpState currentHpState = ConstInfo.initialHpState;
    private static TimeState currentTimeState = ConstInfo.initialTimeState;
    private static bool displayInspect = ConstInfo.initialDisplayInspect;


    // 설정값 Getters & Setters
    public static AnimationState GetCurrentAnimationState() { return currentAnimationState; }
    public static void SetCurrentAnimationState(AnimationState newAnimationState) { currentAnimationState = newAnimationState; }

    public static HpState GetCurrentHpState() { return currentHpState; }
    public static void SetCurrentHpState(HpState newHpState) { currentHpState = newHpState; }

    public static TimeState GetCurrentTimeState() { return currentTimeState; }
    public static void SetCurrentTimeState(TimeState newTimeState) { currentTimeState = newTimeState; }

    public static bool GetDisplayInspect() { return displayInspect; }
    public static void SetDisplayInspect(bool newDisplayInspect) { displayInspect = newDisplayInspect; }


}
