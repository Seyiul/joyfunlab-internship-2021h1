using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_GroundGenerator : MonoBehaviour
{
    public Camera mainCamera;
    public Transform startPoint; //Point from where ground tiles will start
    public SC_PlatformTile tilePrefab;
    public float movingSpeed = 10;
    public int tilesToPreSpawn = 6; //How many tiles should be pre-spawned
    public int tilesWithoutObstacles = 3; //How many tiles at the beginning should not have obstacles, good for warm-up

    List<SC_PlatformTile> spawnedTiles = new List<SC_PlatformTile>();
    int nextTileToActivate = -1;
    [HideInInspector]
    public bool gameOver = false;
    static bool gameStarted = false;
    float score = 0;
    int highScore = 0;
    int secondScore = 0;
    int thirdScore = 0;

    float time = 100000;
    public static SC_GroundGenerator instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        highScore = PlayerPrefs.GetInt("highscore");
        secondScore = PlayerPrefs.GetInt("secondscore");
        thirdScore = PlayerPrefs.GetInt("thirdscore");

        Vector3 spawnPosition = startPoint.position;
        int tilesWithNoObstaclesTmp = tilesWithoutObstacles;
        for (int i = 0; i < tilesToPreSpawn; i++)
        {
            spawnPosition -= tilePrefab.startPoint.localPosition;
            SC_PlatformTile spawnedTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity) as SC_PlatformTile;
            if (tilesWithNoObstaclesTmp > 0)
            {
                spawnedTile.DeactivateAllObstacles();
                spawnedTile.DeactivateAllLife();
                tilesWithNoObstaclesTmp--;
            }
            else
            {
                spawnedTile.ActivateRandomObstacle();
                spawnedTile.ActivateRandomLife();
            }

            spawnPosition = spawnedTile.endPoint.position;
            spawnedTile.transform.SetParent(transform);
            spawnedTiles.Add(spawnedTile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move the object upward in world space x unit/second.
        //Increase speed the higher score we get
        if(movingSpeed < 50)
            movingSpeed += 0.05f;
        if (!gameOver && gameStarted)
        {
            transform.Translate(-spawnedTiles[0].transform.forward * Time.deltaTime * (movingSpeed + (score / 500)), Space.World);
            score += Time.deltaTime * movingSpeed;
            time = time-Time.deltaTime;
            if(time <0 ) gameOver=true;
            
        }
   
        if (mainCamera.WorldToViewportPoint(spawnedTiles[0].endPoint.position).z < 0)
        {
            //Move the tile to the front if it's behind the Camera
            SC_PlatformTile tileTmp = spawnedTiles[0];
            spawnedTiles.RemoveAt(0);
            tileTmp.transform.position = spawnedTiles[spawnedTiles.Count - 1].endPoint.position - tileTmp.startPoint.localPosition;
            tileTmp.ActivateRandomObstacle();
            tileTmp.ActivateRandomLife();
            spawnedTiles.Add(tileTmp);
        }

        if (gameOver || !gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (gameOver)
                {
                    //Restart current scene
                    Scene scene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(scene.name);
                }
                else
                {
                    //Start the game
                    gameStarted = true;
                }
            }
            if (score < highScore)
            {
                if (score < secondScore)
                {
                    if (score < thirdScore)
                        return;
                    PlayerPrefs.SetInt("thirdscore", (int)score);
                    return;
                }
                PlayerPrefs.SetInt("thirdscore", secondScore);
                PlayerPrefs.SetInt("secondscore", (int)score);
                return;
            }
            if (score == highScore) return;
            PlayerPrefs.SetInt("thirdscore", secondScore);
            PlayerPrefs.SetInt("secondscore", highScore);
            PlayerPrefs.SetInt("highscore", (int)score);
        }
    }
   
    void OnGUI()
    {
        if (gameOver)
        {
            GUI.color = Color.red;
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200), "Game Over\nYour score is: " + ((int)score) + "\nPress 'Space' to restart");
        }
        else
        {
            if (!gameStarted)
            {
                GUI.color = Color.red;
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200), "Press 'Space' to start");
            }
        }

        GUI.color = Color.green;
        GUI.Label(new Rect(5, 5, 200, 25), "Score: " + ((int)score));
        GUI.color = Color.yellow;
        GUI.Label(new Rect(5, 25, 200, 25), "1st: " + ((int)highScore));
        GUI.color = Color.yellow;
        GUI.Label(new Rect(5, 50, 200, 25), "2nd: " + ((int)secondScore));
        GUI.color = Color.yellow;
        GUI.Label(new Rect(5, 75, 200, 25), "3rd: " + ((int)thirdScore));

        GUI.color = Color.black;
        GUI.Label(new Rect(300,5, 200,25 ),"Time: "+(int)time);
    }
}
