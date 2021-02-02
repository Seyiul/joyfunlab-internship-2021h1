using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCanvas : MonoBehaviour
{
    public GameObject result;

    // Start is called before the first frame update
    void Start()
    {
        result.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    if (GameManager.instance.GetGameState() == GameState.Result)
        {
            if (GameManager.instance.GetStateChanged())
                result.SetActive(true);
            result.GetComponent<ResultUI>().ShowBattleResult();

        }
    }
    public void MenuHandle()
    {
        
    }
    void ActivateUI(GameObject obj)
    {
        obj.SetActive(true);
    }
}
