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
    Huddle = 2,
    Trap = 3,
    Balloon = 4,
    Monster = 5
}

public class Tile : MonoBehaviour
{ 
    private float mapSpeed;
    public GameObject heartSrc;
    public GameObject huddleSrc;
    public GameObject trapSrc;
    public GameObject balloonSrc;
    public GameObject monsterSrc;

    GameObject heart;
    GameObject huddle;
    GameObject trap;
    GameObject balloon;
    GameObject monster;

    int counter = 5;
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.instance.nextTile != null)
        {
            for (int i = 1; i <= 5; i++)
                MakePath(i);
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
    public void MoveTile()
    {
        if (GameManager.instance.GetGameState() == GameState.Game)
        {
            transform.Translate(0,0, -mapSpeed * Time.deltaTime);
        }
    }
    public void MakePath(int point)
    {
        int emptyTile = Random.Range((int)Line.Left, (int)Line.Right + 1);
        MakeEmpty(emptyTile,point);
        if (emptyTile == (int)Line.Left)
        {
            MakeObstacle((int)Line.Center, point);
            MakeObstacle((int)Line.Right, point);
        }
        else if (emptyTile == (int)Line.Center)
        {
            MakeObstacle((int)Line.Left, point);
            MakeObstacle((int)Line.Right, point);
        }
        else
        { 
            MakeObstacle((int)Line.Center, point);
            MakeObstacle((int)Line.Left, point);
        }
    }
    public void MakeEmpty(int emptyTile,int point)
    {
        heart = Instantiate(heartSrc, new Vector3(ConstInfo.tileX + emptyTile * ConstInfo.lineWidth, ConstInfo.tileY, point * ConstInfo.tileTerm), Quaternion.identity);
        heart.transform.parent = GameManager.instance.nextTile.transform;
    }
    public void MakeObstacle(int obstacleTile,int point)
    {
        int obstacle = Random.Range((int)Obstacle.Huddle, (int)Obstacle.Monster);
        if(obstacle == (int)Obstacle.Huddle)
        {
            huddle = Instantiate(huddleSrc, new Vector3(ConstInfo.tileX + obstacleTile * ConstInfo.lineWidth, ConstInfo.tileY, point * ConstInfo.tileTerm), Quaternion.identity);
            huddle.transform.parent = GameManager.instance.nextTile.transform;
        }
        else if (obstacle == (int)Obstacle.Trap)
        {
            trap = Instantiate(trapSrc, new Vector3(ConstInfo.tileX + obstacleTile * ConstInfo.lineWidth, ConstInfo.tileY, point * ConstInfo.tileTerm), Quaternion.identity);
            trap.transform.parent = GameManager.instance.nextTile.transform;
        }
        else if (obstacle == (int)Obstacle.Balloon)
        {
            balloon = Instantiate(balloonSrc, new Vector3(ConstInfo.tileX + obstacleTile * ConstInfo.lineWidth, ConstInfo.tileY, point * ConstInfo.tileTerm), Quaternion.identity);
            balloon.transform.parent = GameManager.instance.nextTile.transform;
        }
    }

}
