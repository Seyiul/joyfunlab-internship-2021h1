using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerHP : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerHealthbarHandler.SetHealthBarValue(1);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerHealthbarHandler.SetHealthBarValue(PlayerHealthbarHandler.GetHealthBarValue() - 0.0005f);
    }
}
