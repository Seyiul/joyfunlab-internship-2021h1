using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightTiles : MonoBehaviour
{
    Renderer leftTile;
    Renderer centerTile;
    Renderer rightTile;
    // Start is called before the first frame update
    void Start()
    {
        leftTile = GameObject.Find("leftTile").GetComponent<Renderer>();
        centerTile = GameObject.Find("centerTile").GetComponent<Renderer>();
        rightTile = GameObject.Find("rightTile").GetComponent<Renderer>();
        Highlight(PlayerLocation.Center);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Highlight(PlayerLocation curLocation)
    {
        turnOffAll();
        if (curLocation == PlayerLocation.Left)
            leftTile.material.color = Color.yellow;
        else if (curLocation == PlayerLocation.Center)
            centerTile.material.color = Color.yellow;
        else
            rightTile.material.color = Color.yellow;
    }
    void turnOffAll()
    {
        leftTile.material.color = Color.white;
        centerTile.material.color = Color.white;
        rightTile.material.color = Color.white;
    }
}
