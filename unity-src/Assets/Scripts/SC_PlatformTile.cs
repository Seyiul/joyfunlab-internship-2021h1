using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PlatformTile : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public GameObject[] obstacles; //Objects that contains different obstacle types which will be randomly activated
    public GameObject[] life;
    public void ActivateRandomObstacle()
    {
        DeactivateAllObstacles();

        System.Random random = new System.Random();
        int randomNumber = random.Next(0, obstacles.Length);
        obstacles[randomNumber].SetActive(true);
    }

    public void DeactivateAllObstacles()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].SetActive(false);
        }
    }
    public void ActivateRandomLife()
    {
        DeactivateAllLife();
        System.Random random = new System.Random();
        int randNum = random.Next(0, life.Length * 15);
        if(randNum < life.Length&&life != null)
        {
            Debug.Log(randNum);
            life[randNum].SetActive(true);
        }
    }
    public void DeactivateAllLife()
    {
        for (int i = 0; i < life.Length; i++)
        {
            if(life != null)
                life[i].SetActive(false);
        }
    }
}