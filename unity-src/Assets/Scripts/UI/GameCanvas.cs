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
    public GameObject HpDecreaseText;
    // Start is called before the first frame update
    void Start()
    {
        TimeIncreaseText.SetActive(false);
        HpIncreaseText.SetActive(false);
        HpDecreaseText.SetActive(false);
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
        TimeIncreaseText.SetActive(true);
        StartCoroutine(InvisibleText(TimeIncreaseText));
    }
    IEnumerator InvisibleText(GameObject text)
    {
        yield return new WaitForSeconds(0.5f);
        text.SetActive(false);
    }
    public void DisplayHp()
    {
        HpText.text = Player.instance.hp.ToString() + "/" + Player.instance.maxHp.ToString();
    }
    public void DisplayHpIncrease()
    {
        HpIncreaseText.SetActive(true);
        StartCoroutine(InvisibleText(HpIncreaseText));
    }
    public void DisplayHpDecrease()
    {
        HpDecreaseText.SetActive(true);
        StartCoroutine(InvisibleText(HpDecreaseText));
    }
    public void DisplayCombo()
    {
        Combo.text = Player.instance.combo.ToString() + " Combo";
    }
}
