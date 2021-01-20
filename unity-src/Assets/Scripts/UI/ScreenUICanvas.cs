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
    public Text continueText;
    public Text restartText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GetGameState() == GameState.Menu)
        {
            ActivateUI(menu);
            continueText.text = "게임 시작";
           // restartText.text = "환경 설정";
        }
        else if (GameManager.instance.GetGameState() == GameState.Rank)
        {
            ActivateUI(rank);
        }
        else if (GameManager.instance.GetGameState() == GameState.Result)
        {
            ActivateUI(result);
        }
        else if (GameManager.instance.GetGameState() == GameState.Setting)
        {
            ActivateUI(setting);
        }
        else if (GameManager.instance.GetGameState() == GameState.Pause)
        {
            ActivateUI(menu);
            continueText.text = "게임 재개";
            //restartText.text = "다시 시작";
        }
        else
            DeactivateAllUI();
    }
    public void ActivateUI(GameObject obj)
    {
        DeactivateAllUI();
        obj.SetActive(true);
    }

    public void DeactivateAllUI()
    {
        menu.SetActive(false);
        rank.SetActive(false);
        result.SetActive(false);
        setting.SetActive(false);
    }
}
