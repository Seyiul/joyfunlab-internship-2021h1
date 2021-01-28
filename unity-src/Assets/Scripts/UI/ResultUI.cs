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
        playtimeText.text = (Mathf.Floor((Player.instance.playtime - Player.instance.time) * 10) * 0.1f).ToString() + " 초";
        float comboPoint = 1 + (float)((int)Player.instance.maxCombo / 10)/10;
        pointText.text = (Mathf.Round(Player.instance.maxCombo * comboPoint)).ToString() + " 점";
        if (Input.GetKeyDown(KeyCode.Return))
            GameManager.instance.SetGameState(GameState.Menu);
    }
}
