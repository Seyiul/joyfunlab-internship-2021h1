using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum pmenuBtn
{
    resume = 0,
    restart = 1,
    ranking = 2,
    quit = 3
}
public class PauseUI : MonoBehaviour
{
    public GameObject resumeBtn;
    public GameObject restartBtn;
    public GameObject rankingBtn;
    public GameObject quitBtn;
    pmenuBtn curBtn;
    public Sprite buttonSelected;
    public Sprite buttonUnselected;
    // Start is called before the first frame update
    void Start()
    {
        curBtn = pmenuBtn.resume;
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
            case pmenuBtn.resume:
                resumeBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
            case pmenuBtn.restart:
                restartBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
            case pmenuBtn.ranking:
                rankingBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
            case pmenuBtn.quit:
                quitBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
        }

    }
    void Unselected()
    {
        resumeBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        restartBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        rankingBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        quitBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
    }
    public void MenuHandle()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (curBtn > pmenuBtn.resume)
                curBtn--;
            else if (curBtn == pmenuBtn.resume)
                curBtn = pmenuBtn.quit;
            Selected();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (curBtn < pmenuBtn.quit)
                curBtn++;
            else if (curBtn == pmenuBtn.quit)
                curBtn = pmenuBtn.resume;
            Selected();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (curBtn == pmenuBtn.resume)
                GameManager.instance.SetGameState(GameState.Game);
            else if (curBtn == pmenuBtn.restart)
            {
                GameManager.instance.SetGameState(GameState.Game);
                Player.instance.InitialValues();
                Destroy(GameManager.instance.curTile);
                Destroy(GameManager.instance.nextTile);
            }
            else if (curBtn == pmenuBtn.ranking)
                GameManager.instance.SetGameState(GameState.Rank);
            else if (curBtn == pmenuBtn.quit)
                GameManager.instance.SetGameState(GameState.Result);
            GameManager.instance.SetStateChanged(true);
        }
    }

}
