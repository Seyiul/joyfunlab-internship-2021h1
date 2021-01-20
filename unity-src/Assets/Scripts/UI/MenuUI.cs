using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum btn
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
    public btn curBtn;
    public Sprite buttonSelected;
    public Sprite buttonUnselected;
    // Start is called before the first frame update
    void Start()
    {
        curBtn = btn.start;
        Selected();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Selected()
    {
        Unselected();
        switch (curBtn)
        {
            case btn.start:
                startBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
            case btn.setting:
                settingBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
            case btn.ranking:
                rankingBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
            case btn.quit:
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
            if (curBtn > btn.start)
                curBtn--;
            else if (curBtn == btn.start)
                curBtn = btn.quit;
            Selected();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (curBtn < btn.quit)
                curBtn++;
            else if (curBtn == btn.quit)
                curBtn = btn.start;
            Selected();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (curBtn == btn.start)
                GameManager.instance.SetGameState(GameState.Game);
            else if (curBtn == btn.setting)
                GameManager.instance.SetGameState(GameState.Setting);
            else if (curBtn == btn.ranking)
                GameManager.instance.SetGameState(GameState.Rank);
            else if (curBtn == btn.quit)
                GameManager.instance.SetGameState(GameState.Result);
        }
    }

}
