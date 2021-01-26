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
    public GameObject HpIncreaseText;
    // Start is called before the first frame update
    void Start()
    {
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
        TimeIncreaseText.GetComponent<Outline>().useGraphicAlpha = false;
    }
    public void UnDisplayTimeIncrease()
    {
        TimeIncreaseText.GetComponent<Outline>().useGraphicAlpha = true;
    }
    public void DisplayHp()
    {
        HpText.text = Player.instance.hp.ToString() + "/" + Player.instance.maxHp.ToString();
    }
    public void DisplayHpIncrease()
    {
        HpIncreaseText.GetComponent<Outline>().useGraphicAlpha = false;
    }
    public void UnDisplayHpIncrease()
    {
        HpIncreaseText.GetComponent<Outline>().useGraphicAlpha = true;
    }
    public void DisplayCombo()
    {
        Combo.text = Player.instance.combo.ToString() + " Combo";
    }
}
