using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartButton : MonoBehaviour
{
    string currentText;
    string start = "게임 시작";
    string restart = "게임 재개";
    public Text textBox;

    void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        currentText = start;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeStartBtn()
    {
        if (GameManager.instance.GetGameState() == GameState.Menu && currentText != start)
        {
            textBox.text = start;
            currentText = start;
        }
        else if (GameManager.instance.GetGameState() == GameState.Pause && currentText != restart)
        {
            textBox.text = restart;
            currentText = restart;
        }
        currentText = textBox.text;
    }
    public void ClickStartBtn()
    {
        if (string.Compare(currentText, start) == 0)
        {
            Player.instance.InitialValues();
            GameManager.instance.SetGameState(GameState.Game);
        }
        else if (string.Compare(currentText, restart) == 0)
            GameManager.instance.SetGameState(GameState.Game);
    }
}
