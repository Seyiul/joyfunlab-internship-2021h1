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
    public void MenuHandle()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.instance.SetGameState(GameState.Menu);
            GameManager.instance.SetStateChanged(true);
        }
    }
}
