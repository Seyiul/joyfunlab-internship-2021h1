using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public static List<GameObject> bgrounds;
    public GameObject bground;
    float speed;
    int last;

    // 초기 두 개의 배경 프리팹 생성
    void Start() {
        bgrounds = new List<GameObject>();
        GameObject.Find("Bground").SetActive(false);
        bgrounds.Add(Instantiate(bground, new Vector3(ConstInfo.center, ConstInfo.playerInitialPositionY, 0),
            transform.rotation) as GameObject);
        bgrounds.Add(Instantiate(bground, new Vector3(ConstInfo.center, ConstInfo.playerInitialPositionY, ConstInfo.bgroundSizeZ),
            transform.rotation) as GameObject);
    }

    void Update()
    {
        if (IsTimeToCreateBground())
            CreateBground();
        HandleBgroundMove();
        HandleBgoundDestroy();
    }

    // 배경의 z좌표가 0 이하가 되면 다음 배경 생성
    bool IsTimeToCreateBground() { return bgrounds[bgrounds.Count - 1].transform.position.z <= 0; }

    // 배경 프리팹 생성
    void CreateBground()
    {
        bgrounds.Add(Instantiate(bground, new Vector3(ConstInfo.center, 
            ConstInfo.playerInitialPositionY, bgrounds[bgrounds.Count - 1].transform.position.z + ConstInfo.bgroundSizeZ), transform.rotation));
    }

    // 배경 프리팹 이동
    void HandleBgroundMove()
    {
        for (int i = 0; i < bgrounds.Count; i++)
            bgrounds[i].transform.Translate(Vector3.back * Tile.actualSpeed * Time.deltaTime);
    }

    // 배경 프리팹 삭제
    void HandleBgoundDestroy()
    {
        for (int i = 0; i < bgrounds.Count; i++)
        {
            if (bgrounds[i].transform.position.z < -ConstInfo.bgroundSizeZ)
            {
                bgrounds[i].SetActive(false);
                Destroy(bgrounds[i]);
                bgrounds.RemoveAt(i);
            }
        }
    }
}
