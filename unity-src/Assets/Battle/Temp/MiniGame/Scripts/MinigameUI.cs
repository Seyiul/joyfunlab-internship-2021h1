using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameUI : MonoBehaviour
{
    public static MinigameUI instance;

    public GameObject MonsterHP;
    public GameObject PlayerHP;

    void Awake()
    {
    
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.gameObject.SetActive(true);
    }
    public void show(int point)
    {
        GameManager.instance.SetGameState(GameState.Battle);
    }
    public void SetText(int point)
    {
        //MonsterHP.bar.fillAmount = 3;
    }
}
