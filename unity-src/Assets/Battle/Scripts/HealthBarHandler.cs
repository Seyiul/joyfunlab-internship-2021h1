using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
    private static Image HealthBarImage;
    private Text currentHp;

    public static void SetHealthBarValue(float value)
    {
        HealthBarImage.fillAmount = value;
        if (HealthBarImage.fillAmount < 0.2f)
        {
            SetHealthBarColor(Color.red);
        }
        else if (HealthBarImage.fillAmount < 0.65f)
        {
            SetHealthBarColor(Color.yellow);
        }
        else
        {
            SetHealthBarColor(Color.green);
        }
    }

    public static float GetHealthBarValue()
    {
        return HealthBarImage.fillAmount;
    }

    public static void SetHealthBarColor(Color healthColor)
    {
        HealthBarImage.color = healthColor;
    }
    private void Start()
    {
        HealthBarImage = GetComponent<Image>();
        currentHp = GameObject.Find("current").GetComponent<Text>();
    }
    private void Update()
    {
        var hp = (float)(HealthBarImage.fillAmount * 100);
        hp = Mathf.Round(hp * 10) * 0.1f;
        currentHp.text = hp.ToString();
    }
}
