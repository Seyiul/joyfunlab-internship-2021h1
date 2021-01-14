using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbarHandler : MonoBehaviour
{
    private static Image HealthBarImage;
    private Text currentHp;

    public static void SetHealthBarValue(float value)
    {
        HealthBarImage.fillAmount = value;
        if (HealthBarImage.fillAmount < 0.3f)
        {
            SetHealthBarColor(Color.red);
        }
        else if (HealthBarImage.fillAmount < 0.6f)
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

    /// <summary>
    /// Sets the health bar color
    /// </summary>
    /// <param name="healthColor">Color </param>
    public static void SetHealthBarColor(Color healthColor)
    {
        HealthBarImage.color = healthColor;
    }

    private void Start()
    {
        HealthBarImage = GetComponent<Image>();
        currentHp = GameObject.Find("Playercurrent").GetComponent<Text>();
    }
    private void Update()
    {
        var hp = (int)(HealthBarImage.fillAmount * 100);
        currentHp.text = hp.ToString();
    }
}
