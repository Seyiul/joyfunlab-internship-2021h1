using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenUICanvas : MonoBehaviour
{
    public GameObject menu;
    public GameObject rank;
    public GameObject result;
    public GameObject setting;
    public GameObject pmenu;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void MenuHandle()
    {
        if (GameManager.instance.GetGameState() == GameState.Menu)
        {
            if(GameManager.instance.GetStateChanged())
                ActivateUI(menu);
            menu.GetComponent<MenuUI>().MenuHandle();
        }
        else if (GameManager.instance.GetGameState() == GameState.Rank)
        {
            if(GameManager.instance.GetStateChanged())
                ActivateUI(rank);
            rank.GetComponent<MyRankUI>().MenuHandle();
        }
        else if (GameManager.instance.GetGameState() == GameState.Result)
        {
            if(GameManager.instance.GetStateChanged())
                ActivateUI(result);
            result.GetComponent<ResultUI>();
        }
        else if (GameManager.instance.GetGameState() == GameState.Setting)
        {
            if(GameManager.instance.GetStateChanged())
                ActivateUI(setting);
            setting.GetComponent<SettingUI>().MenuHandle();
        }
        else if (GameManager.instance.GetGameState() == GameState.Pause)
        {
            if(GameManager.instance.GetStateChanged())
                ActivateUI(pmenu);
            pmenu.GetComponent<PauseUI>().MenuHandle();
        }
        else
        {
            if(GameManager.instance.GetStateChanged())
                DeactivateAllUI();
        }
    }
    void ActivateUI(GameObject obj)
    {
        DeactivateAllUI();
        obj.SetActive(true);
    }

    void DeactivateAllUI()
    {
        menu.SetActive(false);
        rank.SetActive(false);
        result.SetActive(false);
        setting.SetActive(false);
        pmenu.SetActive(false);
    }
}
