using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highlight : MonoBehaviour
{
    public Color highlightColor;
    public Color transparent;
    Renderer rend;

    // 초기 색상 저장
    void Start()
    {
        rend = GetComponent<Renderer>();
        highlightColor = new Color(1, 0.7f, 0);
        transparent = Color.white;
        highlightColor.a = 0.5f;
        transparent.a = 0;
        if (gameObject.name == "centerTile-highlight")
            rend.material.color = highlightColor;
        else
            rend.material.color = transparent;
    }

    // 하이라이트 타일 색상 업데이트
    void Update()
    {
        rend.material.color = transparent;
        switch (gameObject.name)
        {
            case "leftTile-highlight":
                if (Player.instance.highlight.transform.position.x == ConstInfo.left)
                    rend.material.color = highlightColor;
                break;
            case "centerTile-highlight":
                if (Player.instance.highlight.transform.position.x == ConstInfo.center)
                    rend.material.color = highlightColor;
                break;
            case "rightTile-highlight":
                if (Player.instance.highlight.transform.position.x == ConstInfo.right)
                    rend.material.color = highlightColor;
                break;
        }
    }
}



