using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    // 키넥트 카메라 상의 유저 존재 여부
    private static bool userValid;

    // 유저 위치 변수 (중심, 왼발, 오른발, 왼손, 오른손, 머리)
    public static Vector3 userPosition;
    public static Vector3 userPositionLeftFoot;
    public static Vector3 userPositionRightFoot;
    public static Vector3 userPositionLeftHand;
    public static Vector3 userPositionRightHand;
    public static Vector3 userPositionHead;

    // 유저의 팔과 팔꿈치 사이의 거리
    public static float DistanceBetweenHandAndElbow;

    static void Start() { InitialUserPosition(); }

    // 유저 벡터 초기화 (0, 0, 0)
    static void InitialUserPosition()
    {
        userPosition = Vector3.zero;
        userPositionLeftFoot = Vector3.zero;
        userPositionRightFoot = Vector3.zero;
        userPositionLeftHand = Vector3.zero;
        userPositionRightHand = Vector3.zero;
        userPositionHead = Vector3.zero;
        DistanceBetweenHandAndElbow = 0;
    }



    // 키넥트 좌표를 게임 상의 좌표로 변환 (좌우: -1.35 ~ 1.35 => -960 ~ 960 / 앞뒤: 2.2 ~ 0.7 => -540 ~ 540)
    public static Vector3 HandleKinectPosition(Vector3 kinectPosition)    {
        return new Vector3(kinectPosition.x * 711, kinectPosition.y * 720, (kinectPosition.z - 1.45f) * -720);
    }

    // 발의 y 좌표에 따른 발 위치 원 크기 설정 (scale: 0 ~ 1 -> ConstInfo.footPrintStartSize 에 따라 변경 가능)
    public static float HandleFootprintSize(float footPositionY) {
        if (footPositionY < 110)
            return ConstInfo.footPrintInitialScale;
        else if (footPositionY >= 110 && footPositionY < 300)
            return (300 - footPositionY) / 190 * ConstInfo.footPrintInitialScale;
        else
            return 0;
    }

    // 오브젝트1이 오브젝트2에 완전히 포함되는지 확진
    public static bool RectOverlaps(GameObject obj1, GameObject obj2)
    {
        RectTransform rectTrans1 = obj1.GetComponent<RectTransform>();
        RectTransform rectTrans2 = obj2.GetComponent<RectTransform>();
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);
        return rect1.Overlaps(rect2) && (obj1.activeSelf && obj2.activeSelf);
    }

    public static bool OneFootOverlaps(GameObject leftFoot, GameObject rightFoot, GameObject tile) {
        return (RectOverlaps(leftFoot, tile) && leftFoot.transform.localScale.x > ConstInfo.buttonPushFootPrintScaleX) 
            || (RectOverlaps(rightFoot, tile) && rightFoot.transform.localScale.x > ConstInfo.buttonPushFootPrintScaleX);
    }

    public static bool TwoFootOverlaps(GameObject leftFoot, GameObject rightFoot, GameObject tile)
    {
        return (RectOverlaps(leftFoot, tile) && leftFoot.transform.localScale.x > ConstInfo.buttonPushFootPrintScaleX) 
            && (RectOverlaps(rightFoot, tile) && rightFoot.transform.localScale.x > ConstInfo.buttonPushFootPrintScaleX);
    }

    public static bool VectorInside(Vector3 Vec, GameObject obj) {
        RectTransform rectTrans = obj.GetComponent<RectTransform>();
        return (Vec.x > rectTrans.localPosition.x + rectTrans.rect.xMin && Vec.x < rectTrans.localPosition.x + rectTrans.rect.xMax) 
            && (Vec.z > rectTrans.localPosition.y + rectTrans.rect.yMin && Vec.z < rectTrans.localPosition.y + rectTrans.rect.yMax);
    }


    // 유저 존재 여부 Getter & Setter
    public static bool GetUserValid() { return userValid; }
    public static void SetUserValid(bool newUserValid) { userValid = newUserValid; }
}
