using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    public GameObject tile;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.curTile = Instantiate(tile, new Vector3(ConstInfo.tileX, ConstInfo.tileY, ConstInfo.tileZ), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        MakeTile();
    }
    public void MakeTile()
    {
        if (GameManager.instance.nextTile != null)
        {
            if (GameManager.instance.nextTile.transform.position.z <= 0)
            {
                Destroy(GameManager.instance.curTile);
                GameManager.instance.curTile = GameManager.instance.nextTile;
                GameManager.instance.nextTile = Instantiate(tile, new Vector3(ConstInfo.tileX, ConstInfo.tileY, ConstInfo.tileLength), Quaternion.identity);
            }
        }
        else
        {
            GameManager.instance.nextTile = Instantiate(tile, new Vector3(ConstInfo.tileX, ConstInfo.tileY, ConstInfo.tileLength), Quaternion.identity);
        }
    }

}
