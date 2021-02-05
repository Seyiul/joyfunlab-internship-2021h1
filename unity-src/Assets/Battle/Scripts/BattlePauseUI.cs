using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BattlePauseUI : MonoBehaviour
{
    enum pmenuBtn
    {
        resume = 0,
        restart = 1,
        quit = 2
    }

    public GameObject resumeBtn;
    public GameObject restartBtn;
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
            case pmenuBtn.quit:
                quitBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
        }

    }
    void Unselected()
    {
        resumeBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        restartBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        quitBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
    }
    public void MenuHandle()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Floor.isUp)
        {
            Floor.isUp = false;
            if (curBtn > pmenuBtn.resume)
                curBtn--;
            else if (curBtn == pmenuBtn.resume)
                curBtn = pmenuBtn.quit;
            Selected();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Floor.isDown)
        {
            Floor.isDown = false;
            if (curBtn < pmenuBtn.quit)
                curBtn++;
            else if (curBtn == pmenuBtn.quit)
                curBtn = pmenuBtn.resume;
            Selected();
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Floor.isEnter)
        {
            Floor.isEnter = false;
            if (curBtn == pmenuBtn.resume)
            {
                GameManager.instance.SetStateChanged(true);

                Time.timeScale = 1;
                GameManager.instance.SetGameState(GameState.Battle);
            }
               
            else
            {
                if (curBtn == pmenuBtn.restart)
                {
                    GameManager.instance.SetStateChanged(true);

                    GameManager.instance.SetGameState(GameState.Game);
                    SceneManager.LoadScene("Game");
                    Player.instance.InitialAll();
                }
                else if (curBtn == pmenuBtn.quit)
                { GameManager.instance.SetGameState(GameState.Result);
                    gameObject.SetActive(false);
                }
            }
            GameManager.instance.SetStateChanged(true);
        }
    }
}
