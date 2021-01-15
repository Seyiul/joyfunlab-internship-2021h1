using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum btn
{
    start = 0,
    setting = 1,
    ranking = 2,
    quit = 3
}
public class MenuUI : MonoBehaviour
{
    public GameObject startBtn;
    public GameObject settingBtn;
    public GameObject rankingBtn;
    public GameObject quitBtn;
    public btn current;
    public Sprite buttonSelected;
    public Sprite buttonUnselected;
    // Start is called before the first frame update
    void Start()
    {
        current = btn.start;
        Selected();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Selected()
    {
        Unselected();
        switch (current)
        {
            case btn.start:
                startBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
            case btn.setting:
                settingBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
            case btn.ranking:
                rankingBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
            case btn.quit:
                quitBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
                break;
        }

    }
    void Unselected()
    {
        startBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        settingBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        rankingBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
        quitBtn.GetComponent<UnityEngine.UI.Image>().sprite = buttonUnselected;
    }
}
