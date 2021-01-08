using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstInfo
{
    // < 공통 >

    // 초기 설정값
    public const AnimationState initialAnimationState = AnimationState.Animation;
    public const HpState initialHpState = HpState.Normal;
    public const TimeState initialTimeState = TimeState.Normal;
    public const bool initialDisplayInspect = false;
    public const int gameFrameRate = 60;

    // 바닥 UI 관련 상수
    public const int floorUICanvasWidth = 1920;
    public const int floorUICanvasHeight = 1080;

    // 바닥 UI 버튼 관련 상수
    public const float buttonPushTime = 0.5f;
    public const float buttonDelayTime = 0.2f;

    // 바닥 UI 발 위치 원 관련 상수
    public const float footPrintInitialScale = 1;
    public const float footPrintSpeed = 0.5f;
    public const float buttonPushFootPrintScaleX = 0.3f;



    // < Game Scene >

    // 체력 상수
    public const int InitialHp = 50;
    public const int maxHp = 100;

    // 장애물 상호작용 관련 상수
    public const int heartTileHpIncrease = 2;
    public const int balloonTimeIncrease = 3;

    // 인게임 UI 표시 시간
    public const float comboDisplayTime = 0.7f;
    public const float damageEffectTime = 0.1f;
    public const float increaseTextAlphaDecrease = 0.02f;

    // 플레이어 위치 관련 상수
    public const float left = 100;
    public const float center = 114;
    public const float right = 128;
    public const float playerInitialPositionY = 1.6f;
    public const float playerInitialPositionZ = -40;

    // 플레이어 동작 조건 관련 상수
    public const float stepHeight = 144;
    public const float punchDistance = 216;
    public const float jumpHeight = 20;
    public const float jumpFootHeightDifferenceLimit = 21.6f;
    public const float jumpFootPositionVariationLimit = 360;


    // 타일 위치 관련 상수
    public const float tileInitialPositionY = 1;
    public const float tileInitialPositionZ = 200;
    public const float tileAnimationStartPositionZ = 72;
    public const float tileDestroyPositionZ = -65;
    public static float[] collisionPositionZ = { -30, -36, -42, -48 };
    public static float resetHeartBonusCountPositionZ = -60;
    public const float spacingBetweenTiles = 60;
    public const float collisionRange = 5;

    // 속도 관련 상수
    public const float initialActualSpeed = 30;
    public const float MaxActualSpeed = 90;
    public const float InitialTileCreateDelay = 2;
    public const float tileCreateDelayIncrease = 0.01f;
    public const float extraSpeedIncrease = 0.1f;

    // 이벤트 시간 관련 상수
    public const float obstableAnimStartTime = 20;
    public const float trapAnimStartTime = 30;

    // 게임 시간 관련 상수
    public const float gameTime = 60;
    public const float jumpingTime = 0.6f;
    public const float stumblingTime = 0.7f;
    public const float punchingTime = 1f;

    // 배경 프리팹 길이 상수
    public const float runningTrackWidth = 42;
    public const float bgroundSizeZ = 335.1012f;
}
