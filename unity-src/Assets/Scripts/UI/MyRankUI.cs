using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRankUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }
    // 랭크 UI에서의 입력 핸들
    public void MenuHandle()
    {
        // 엔터 키 누르면(뒤로가기 버튼 하나밖에 없음)
        if (Input.GetKeyDown(KeyCode.Return)||Floor.isEnter)
        {
            Floor.isEnter = false;
            // 메뉴 상태로 변경
            GameManager.instance.SetGameState(GameState.Menu);
            GameManager.instance.SetStateChanged(true);
        }
    }
}
