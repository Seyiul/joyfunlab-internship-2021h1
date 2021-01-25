using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    public Text Combo;
    public Text CountDownTimer;
    public Text SpeedText;
    public Text TimerText;
    public Text HpText;
    public GameObject TimeIncreaseText;
    float displayTimer;
    // Start is called before the first frame update
    void Start()
    {
        displayTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisplayTime()
    {
        TimerText.text = (Mathf.Floor(Player.instance.time*10)*0.1f).ToString();
    }
    public void DisplayTimeIncrease()
    {
        displayTimer += Time.deltaTime;
        if(displayTimer >= ConstInfo.displayTimer)
            TimeIncreaseText.SetActive(true);
        else
        {
            TimeIncreaseText.SetActive(false);
            displayTimer = 0;
        }
    }
    public void DisplayHp()
    {
        HpText.text = Player.instance.hp.ToString() + "/" + Player.instance.maxHp.ToString();
    }
}
