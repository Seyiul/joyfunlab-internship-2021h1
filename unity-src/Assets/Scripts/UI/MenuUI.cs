using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum menuBtn
{
    start = 0,
    setting = 1,
    ranking = 2,
    quit = 3
}
public class MenuUI : MonoBehaviour
{
    public GameObject startBtn;
    public GameObject settingBtn;
    public GameObject rankingBtn;
    public GameObject quitBtn;
    menuBtn curBtn;
    public Sprite buttonSelected;
    public Sprite buttonUnselected;
    // Start is called before the first frame update
    void Start()
    {
        curBtn = menuBtn.start;
        Selected();
    }
    // Update is called once per frame
    void Update()
    {
    }
    void Selected()
    {
        Unselected();
        switch (curBtn)
        {
            case menuBtn.start:
                startBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
            case menuBtn.setting:
                settingBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
            case menuBtn.ranking:
                rankingBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
            case menuBtn.quit:
                quitBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
        }

    }
    void Unselected()
    {
        startBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        settingBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        rankingBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        quitBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
    }
    public void MenuHandle()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (curBtn > menuBtn.start)
                curBtn--;
            else if (curBtn == menuBtn.start)
                curBtn = menuBtn.quit;
            Selected();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (curBtn < menuBtn.quit)
                curBtn++;
            else if (curBtn == menuBtn.quit)
                curBtn = menuBtn.start;
            Selected();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (curBtn == menuBtn.start)
            {
                GameManager.instance.SetGameState(GameState.Game);
                Player.instance.InitialValues();
            }
            else if (curBtn == menuBtn.setting)
                GameManager.instance.SetGameState(GameState.Setting);
            else if (curBtn == menuBtn.ranking)
                GameManager.instance.SetGameState(GameState.Rank);
            else if (curBtn == menuBtn.quit)
                GameManager.instance.SetGameState(GameState.Result);
            GameManager.instance.SetStateChanged(true);
        }
    }

}
