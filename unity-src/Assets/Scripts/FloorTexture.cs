using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using static UnityEngine.LowLevel.PlayerLoop;
using UnityEngine.UI;

public class FloorTexture : MonoBehaviour
{
    public static Texture2D PositionButton;
    public static Texture2D PositionButtonBlue;
    public static Texture2D PositionDisabled;

    public static Texture2D BackButton;
    public static Texture2D BackButtonPress;

    public static Texture2D DownArrowButton;
    public static Texture2D DownArrowButtonPress;

    public static Texture2D HomeButton;
    public static Texture2D HomeButtonPress;

    public static Texture2D LeftArrowButton;
    public static Texture2D LeftArrowButtonPress;

    public static Texture2D LeftButton;
    public static Texture2D LeftButtonPress;

    public static Texture2D RightArrowButton;
    public static Texture2D RightArrowButtonPress;

    public static Texture2D RightButton;
    public static Texture2D RightButtonPress;

    public static Texture2D UpArrowButton;
    public static Texture2D UpArrowButtonPress;

    public static Texture2D RemeasurementButton;
    public static Texture2D RemeasurementButtonPress;
    
    public static Texture2D FloorTileSelected;
    public static Texture2D FloorTileUnSelected;



    void Start()
    {
        PositionButton = Resources.Load("UI/Floor/PositionButton") as Texture2D;
        PositionButtonBlue = Resources.Load("UI/Floor/PositionButtonBlue") as Texture2D;
        PositionDisabled = Resources.Load("UI/Floor/PositionDisabled") as Texture2D;

        BackButton = Resources.Load("UI/Floor/BackButton") as Texture2D;
        BackButtonPress = Resources.Load("UI/Floor/BackButtonPress") as Texture2D;

        DownArrowButton = Resources.Load("UI/Floor/DownArrowButton") as Texture2D;
        DownArrowButtonPress = Resources.Load("UI/Floor/DownArrowButtonPress") as Texture2D;

        HomeButton = Resources.Load("UI/Floor/HomeButton") as Texture2D;
        HomeButtonPress = Resources.Load("UI/Floor/HomeButtonPress") as Texture2D;

        LeftArrowButton = Resources.Load("UI/Floor/LeftArrowButton") as Texture2D;
        LeftArrowButtonPress = Resources.Load("UI/Floor/LeftArrowButtonPress") as Texture2D;

        LeftButton = Resources.Load("UI/Floor/LeftButton") as Texture2D;
        LeftButtonPress = Resources.Load("UI/Floor/LeftButtonPress") as Texture2D;

        RightArrowButton = Resources.Load("UI/Floor/RightArrowButton") as Texture2D;
        RightArrowButtonPress = Resources.Load("UI/Floor/RightArrowButtonPress") as Texture2D;

        RightButton = Resources.Load("UI/Floor/RightButton") as Texture2D;
        RightButtonPress = Resources.Load("UI/Floor/RightButtonPress") as Texture2D;

        UpArrowButton = Resources.Load("UI/Floor/UpArrowButton") as Texture2D;
        UpArrowButtonPress = Resources.Load("UI/Floor/UpArrowButtonPress") as Texture2D;

        RemeasurementButton = Resources.Load("UI/Floor/Re-measurementButton") as Texture2D;
        RemeasurementButtonPress = Resources.Load("UI/Floor/Re-measurementButtonPress") as Texture2D;

        FloorTileSelected = Resources.Load("Image/FloorTileSelected") as Texture2D;
        FloorTileUnSelected = Resources.Load("Image/FloorTileUnSelected") as Texture2D;
    }

    // 버튼의 텍스처를 변경 (0번째 자식 오브젝트의 텍스처 변경)
    public static void setButtonTexture(GameObject obj, Texture newTexture) { obj.transform.GetChild(0).GetComponent<RawImage>().texture = newTexture; }

    // 바닥 타일의 텍스처를 변경
    public static void setFloorTileTexture(GameObject obj, Texture newTexture) { obj.GetComponent<RawImage>().texture = newTexture; }

    // 버튼을 누르는 효과 (시계방향으로 Delay가 삭제)
    public static void ProgressDelayTexture(GameObject obj, float ratio) { obj.transform.GetChild(1).GetComponent<Image>().fillAmount = 1 - ratio; }

    // 버튼을 누르는 효과 (버튼이 밑으로 10 이동)
    public static void MoveAllChildTexture(GameObject obj, bool move) 
    {
        float y = move ? -10 : 0;
        for (int i = 0; i < 3; i++)
            obj.transform.GetChild(i).transform.localPosition = new Vector3(obj.transform.GetChild(i).transform.localPosition.x, y);
    }
}
