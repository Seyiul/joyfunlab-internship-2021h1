using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    public GameObject tile;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        MakeTile();
    }
    public void MakeTile()
    {
        if (GameManager.instance.curTile == null)
            GameManager.instance.curTile = Instantiate(tile, new Vector3(ConstInfo.tileX, ConstInfo.tileY, ConstInfo.tileZ), Quaternion.identity);
        if (GameManager.instance.nextTile != null)
        {
            if (GameManager.instance.nextTile.transform.position.z < 0)
            {
                Transform[] childs = GameManager.instance.curTile.GetComponentsInChildren<Transform>(true);
                if(childs != null)
                {
                    for(int i = 0; i < childs.Length; i++)
                    {
                        Destroy(childs[i].gameObject);
                    }
                }
                GameManager.instance.curTile = GameManager.instance.nextTile;
                GameManager.instance.nextTile = Instantiate(tile, new Vector3(ConstInfo.tileX, ConstInfo.tileY, ConstInfo.tileLength), Quaternion.identity);
            }
        }
        //게임매니저의 nextTile이 null이면(초기 타일이 없는 상태)
        else
        {
            GameManager.instance.nextTile = Instantiate(tile, new Vector3(ConstInfo.tileX, ConstInfo.tileY, ConstInfo.tileLength), Quaternion.identity);
        }
    }

}
