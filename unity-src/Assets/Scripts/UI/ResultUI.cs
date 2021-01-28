using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResultUI : MonoBehaviour
{
    public Text maxComboText;
    public Text playtimeText;
    public Text pointText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowResult()
    {
        maxComboText.text = Player.instance.maxCombo.ToString() + " 회";
        playtimeText.text = (Player.instance.playtime - Player.instance.time).ToString() + " 초";

        float comboPoint = 1 + (float)((int)Player.instance.maxCombo / 10)/10;
        pointText.text = (Player.instance.maxCombo * comboPoint).ToString() + " 점";
    }
}
