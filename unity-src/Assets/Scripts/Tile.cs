using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Line: int
{
    Left = -1,
    Center = 0,
    Right = 1
}

public enum Obstacle: int
{
    Empty = 0,
    Heart = 1,
    Hurdle = 2,
    Trap = 3,
    Balloon = 4,
    Monster = 5
}

public class Tile : MonoBehaviour
{ 
    private float mapSpeed;
    public GameObject heartSrc;
    public GameObject hurdleSrc;
    public GameObject trapSrc;
    public GameObject balloonSrc;
    public GameObject monsterSrc;
    int point;
    GameObject heart;
    GameObject hurdle;
    GameObject trap;
    GameObject balloon;
    GameObject monster;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.instance.nextTile != null)
        {
            for (point = 0; point <= 4; point++)
            {
                MakePath();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GetGameState() == GameState.Game)
        {
            mapSpeed = Player.instance.speed * 1.5f;
            MoveTile();
        }
    }
    void MoveTile()
    {
        if (GameManager.instance.GetGameState() == GameState.Game)
        {
            transform.Translate(0,0, -mapSpeed * Time.deltaTime);
        }
    }
    void MakePath()
    {
        int emptyTile = Random.Range((int)Line.Left, (int)Line.Right + 1);
        MakeEmpty(emptyTile);
        if (emptyTile == (int)Line.Left)
        {
            MakeObstacle((int)Line.Center);
            MakeObstacle((int)Line.Right);
        }
        else if (emptyTile == (int)Line.Center)
        {
            MakeObstacle((int)Line.Left);
            MakeObstacle((int)Line.Right);
        }
        else
        { 
            MakeObstacle((int)Line.Center);
            MakeObstacle((int)Line.Left);
        }
    }
    void MakeEmpty(int emptyTile)
    {
        if (Random.Range((int)Obstacle.Empty, (int)Obstacle.Heart + 3) == (int)Obstacle.Heart)
            MakeHeart(emptyTile);
    }
    void MakeHeart(int emptyTile)
    {
        heart = Instantiate(heartSrc, new Vector3(ConstInfo.tileX + emptyTile * ConstInfo.lineWidth, ConstInfo.tileY, ConstInfo.tileLength + point * ConstInfo.tileTerm), Quaternion.identity);
        heart.transform.parent = GameManager.instance.nextTile.transform;
    }
    void MakeObstacle(int obstacleTile)
    {
        int obstacle = Random.Range((int)Obstacle.Hurdle, (int)Obstacle.Monster);
        if(obstacle == (int)Obstacle.Hurdle)
            MakeHurdle(obstacleTile);
        else if (obstacle == (int)Obstacle.Trap)
            MakeTrap(obstacleTile);
        else if (obstacle == (int)Obstacle.Balloon)
            MakeBalloon(obstacleTile);
    }
    void MakeHurdle(int obstacleTile)
    {
        hurdle = Instantiate(hurdleSrc, new Vector3(ConstInfo.tileX + obstacleTile * ConstInfo.lineWidth, ConstInfo.tileY, ConstInfo.tileLength + point * ConstInfo.tileTerm), Quaternion.identity);
     
        if(GameManager.instance.nextTile)
        hurdle.transform.parent = GameManager.instance.nextTile.transform;
    }
    void MakeTrap(int obstacleTile)
    {
        trap = Instantiate(trapSrc, new Vector3(ConstInfo.tileX + obstacleTile * ConstInfo.lineWidth, ConstInfo.tileY, ConstInfo.tileLength + point * ConstInfo.tileTerm), Quaternion.identity);
        trap.transform.parent = GameManager.instance.nextTile.transform;
    }
    void MakeBalloon(int obstacleTile)
    {
        balloon = Instantiate(balloonSrc, new Vector3(ConstInfo.tileX + obstacleTile * ConstInfo.lineWidth, ConstInfo.tileY - 4, ConstInfo.tileLength + point * ConstInfo.tileTerm), Quaternion.identity);
        balloon.transform.parent = GameManager.instance.nextTile.transform;
        Debug.Log(GameManager.instance.nextTile);
    }

}
