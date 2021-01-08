using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // 게임 오브젝트 변수 선언
    public GameObject heartTile;
    public GameObject hurdleTile;
    public GameObject emptyTile;
    public GameObject passTile;
    public GameObject trapTile;
    public GameObject balloonTile;
    public GameObject trapTileEvent;
    public GameObject hurdleTileEvent;

    // 리스트 변수 선언
    public static List<GameObject> randomTiles = new List<GameObject>();
    public static List<GameObject> badTiles = new List<GameObject>();
    public static List<GameObject> activatedTiles;

    // 마지막 줄의 하트 위치 변수
    public static float lastHeartDirection;

    // 추가속도, 실제속도 변수
    public static float userSpeed;
    public static float actualSpeed;

    // 타일이 나온 뒤 다음타일이 나오기까지의 시간
    public float tileCreateDelay;

    // 생성된 타일의 줄 수 변수
    public int createTileCount;

    // 하트의 보너스 점수 체크용 변수
    public int heartBonusCount;

    // 애니메이터 변수 선언
    public RuntimeAnimatorController trapActivate;
    public RuntimeAnimatorController trapEvent;
    public RuntimeAnimatorController hurdleEvent;
    public RuntimeAnimatorController hurdleDown;
    public RuntimeAnimatorController balloonEvent;

    void Start()
    {
        InitialValues();
        SetTileLists();
    }

    // 변수 초기화
    void InitialValues()
    {
        activatedTiles = new List<GameObject>();
        tileCreateDelay = ConstInfo.InitialTileCreateDelay;
        actualSpeed = ConstInfo.initialActualSpeed;
        createTileCount = 0;
        userSpeed = 0;
        lastHeartDirection = 0;
        heartBonusCount = 0;
    }

    // List 설정 (random: 장애물, 빈, 함정 / bad: 장애물, 함정)
    void SetTileLists() {
        randomTiles.Add(hurdleTile);
        randomTiles.Add(emptyTile);
        randomTiles.Add(trapTile);

        badTiles.Add(hurdleTile);
        badTiles.Add(trapTile);
    }

    void Update()
    {
        HandleTiles();
        CheckCollision();
    }

    // 타일 설정 (생성, 속도, 이동, 삭제)
    void HandleTiles()
    {
        HandleTileCreate();
        HandleTileAnimation();
        HandleTileSpeed();
        HandleTileMove();
        HandleTileDestroy();
    }

    // 타일 생성 알고리즘
    void HandleTileCreate()
    {
        if (IsTimeToCreateTiles())
        {
            CreateTiles();           
            if (createTileCount == 10)
            {
                tileCreateDelay -= ConstInfo.tileCreateDelayIncrease;
                createTileCount = 0;
            }
        }
    }

    // 마지막 줄 타일이 간격만큼 이동 시 타일 생성
    bool IsTimeToCreateTiles()
    {
        if (activatedTiles.Count > 0)
            return activatedTiles[activatedTiles.Count - 1].transform.position.z < ConstInfo.tileInitialPositionZ - ConstInfo.spacingBetweenTiles;
        else
            return true;
    }

    // 마지막 줄 타일에 하트 존재 여부에 따른 타일 생성
    void CreateTiles()
    {
        if (lastHeartDirection == 0)
            CreateTilesAfterNoHeart();
        else
            CreateTilesAfterHeart();
        createTileCount += 1;
    }

    // 마지막 줄에 하트가 없을 경우의 타일 생성
    void CreateTilesAfterNoHeart()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                SetPassDirection(ConstInfo.left);
                CreateOne(GetRandomFromList(randomTiles), ConstInfo.center);
                CreateOne(GetRandomFromList(randomTiles), ConstInfo.right);
                break;
            case 1:
                CreateOne(GetRandomFromList(randomTiles), ConstInfo.left);
                SetPassDirection(ConstInfo.center);
                CreateOne(GetRandomFromList(randomTiles), ConstInfo.right);
                break;
            case 2:
                CreateOne(GetRandomFromList(randomTiles), ConstInfo.left);
                CreateOne(GetRandomFromList(randomTiles), ConstInfo.center);
                SetPassDirection(ConstInfo.right);
                break;
        }
    }
    
    // 마지막 줄에 하트가 있는 경우의 타일 생성
    void CreateTilesAfterHeart()
    {
        if (lastHeartDirection == ConstInfo.left)
        {
            if (RandomNum(2) % 2 == 0)
            {
                CreateOne(GetRandomFromList(badTiles), ConstInfo.left);
                SetPassDirection(ConstInfo.center);
                CreateOne(GetRandomFromList(randomTiles), ConstInfo.right);
            }
            else
            {
                CreateOne(GetRandomFromList(badTiles), ConstInfo.left);
                CreateOne(GetRandomFromList(randomTiles), ConstInfo.center);
                SetPassDirection(ConstInfo.right);
            }
        }
        else if (lastHeartDirection == ConstInfo.center)
        {
            if (RandomNum(2) % 2 == 0)
            {
                SetPassDirection(ConstInfo.left);
                CreateOne(GetRandomFromList(badTiles), ConstInfo.center);
                CreateOne(GetRandomFromList(randomTiles), ConstInfo.right);
            }
            else
            {
                CreateOne(GetRandomFromList(randomTiles), ConstInfo.left);
                CreateOne(GetRandomFromList(badTiles), ConstInfo.center);
                SetPassDirection(ConstInfo.right);
            }
        }
        else if (lastHeartDirection == ConstInfo.right)
        {
            if (RandomNum(2) % 2 == 0)
            {
                SetPassDirection(ConstInfo.left);
                CreateOne(GetRandomFromList(randomTiles), ConstInfo.center);
                CreateOne(GetRandomFromList(badTiles), ConstInfo.right);
            }
            else
            {
                CreateOne(GetRandomFromList(randomTiles), ConstInfo.left);
                SetPassDirection(ConstInfo.center);
                CreateOne(GetRandomFromList(badTiles), ConstInfo.right);
            }
        }
    }

    // 지나갈 수 있는 길 생성 (1/6:풍선, 2/6: 하트, 1/2: 빈타일)
    void SetPassDirection(float direction)
    {
        int random = RandomNum(6);
        lastHeartDirection = 0;
        if (random <= 1)
            CreateOne(balloonTile, direction);
        else if (random <= 3)
        {
            CreateOne(heartTile, direction);
            lastHeartDirection = direction;
        } else
            CreateOne(passTile, direction);
    }

    // 타일을 direction에 한 개 생성
    void CreateOne(GameObject tile, float direction)
    {
        activatedTiles.Add(Instantiate(tile, new Vector3(direction, ConstInfo.tileInitialPositionY, ConstInfo.tileInitialPositionZ), 
            Player.instance.transform.rotation) as GameObject);
    }


    // 이벤트 발생 알고리즘 (애니메이션)
    void HandleTileAnimation() {
        if (GameUI.instance.timer <= ConstInfo.gameTime - ConstInfo.obstableAnimStartTime)
            HandleHurdleAnimation();
        if (GameUI.instance.timer <= ConstInfo.gameTime - ConstInfo.trapAnimStartTime)
            HandleTrapAnimation();
    }


    // 조건 만족 시 장애물 애니메이션 재생
    void HandleHurdleAnimation()
    {
        for (int i = 0; i <= activatedTiles.Count - 3; i += 3)
        {
            GameObject lastLeftTile = activatedTiles[i];
            GameObject lastCenterTile = activatedTiles[i + 1];
            GameObject lastRightTile = activatedTiles[i + 2];

            // 중앙에 장애물이 있고 좌, 우로 통과가 가능한 경우
            if ((lastCenterTile.tag == "Hurdle Tile" || lastCenterTile.tag == "Trap Tile")
                && (lastRightTile.tag == "Empty Tile" || lastLeftTile.tag == "Empty Tile")
                && Mathf.Abs(lastCenterTile.transform.position.z - (ConstInfo.tileAnimationStartPositionZ + actualSpeed)) < ConstInfo.collisionRange)
            {
                if (lastLeftTile.tag == "Empty Tile")
                    HandleAnimation(lastLeftTile, hurdleTileEvent, i);
                else if (lastRightTile.tag == "Empty Tile")
                    HandleAnimation(lastRightTile, hurdleTileEvent, i + 2);
            }
        }
    }

    // 조건 만족 시 구덩이 애니메이션 재생
    void HandleTrapAnimation()
    {
        for (int i = 0; i < activatedTiles.Count; i++)
            if (activatedTiles[i].tag == "Empty Tile"
                && Mathf.Abs(activatedTiles[i].transform.position.z - (ConstInfo.tileAnimationStartPositionZ + actualSpeed)) < ConstInfo.collisionRange)
                HandleAnimation(activatedTiles[i], trapTileEvent, i);
    }

    // 애니메이션 재생 타일로 변경
    void HandleAnimation(GameObject oldTile, GameObject newTile, int index)
    {
        activatedTiles[index] = Instantiate(newTile, oldTile.transform.position, transform.rotation);
        oldTile.SetActive(false);
        Destroy(oldTile);
    }



    // 타일 이동 알고리즘
    void HandleTileMove()
    {
        for (int i = 0; i < activatedTiles.Count; i++)
            activatedTiles[i].transform.Translate(Vector3.back * actualSpeed * Time.deltaTime);
    }

    // 타일 속도 알고리즘
    void HandleTileSpeed()
    {
        if (GameManager.instance.GetGameState() == GameState.Game)
        {
            actualSpeed = ((ConstInfo.spacingBetweenTiles / tileCreateDelay) + userSpeed);
            if (actualSpeed > ConstInfo.MaxActualSpeed)
                actualSpeed = ConstInfo.MaxActualSpeed;
        }
        else
            actualSpeed = 0;
    }

    // 타일 삭제 알고리즘
    void HandleTileDestroy()
    {
        for (int i = 0; i < activatedTiles.Count; i++)
        {
            if (activatedTiles[i].transform.position.z < ConstInfo.tileDestroyPositionZ)
            {
                activatedTiles[i].SetActive(false);
                Destroy(activatedTiles[i]);
                activatedTiles.RemoveAt(i);
            }
        }
    }



    // 충동 판정 알고리즘
    void CheckCollision() // 하트 기준 좌표: 타일 + 30, 36, 42, 48
    {
        for (int i = 0; i < activatedTiles.Count; i++)
        {
            switch (activatedTiles[i].tag)
            {
                case "Heart Tile":
                    CheckCollisionHeart(activatedTiles[i]);
                    break;
                case "Hurdle Tile":
                    CheckCollisionObstacle(activatedTiles[i], hurdleDown);
                    break;
                case "Empty Tile":
                    CheckCollisionEmpty(activatedTiles[i]);
                    break;
                case "Pass Tile":
                    CheckCollisionEmpty(activatedTiles[i]);
                    break;
                case "Trap Tile":
                    CheckCollisionObstacle(activatedTiles[i], trapActivate);
                    break;
                case "Balloon Tile":
                    CheckCollisionBalloon(activatedTiles[i]);
                    break;
            }
        }
    }

    // 하트 충돌 판정 알고리즘
    void CheckCollisionHeart(GameObject obj)
    {
        for (int i = 0; i < ConstInfo.collisionPositionZ.Length; i++)
        {
            if (Mathf.Abs(obj.transform.position.z - ConstInfo.collisionPositionZ[i]) < ConstInfo.collisionRange
                && GetChildTransform(obj, i * 2).localScale.x != 0
                && obj.transform.position.x == Player.instance.highlight.transform.position.x
                && !Player.instance.isJumping)
            {
                if (i == 0)
                    Player.instance.EmptyCollision();
                GetChildTransform(obj, i * 2).localScale = Vector3.zero;
                GetChildTransform(obj, i * 2 + 1).localScale = Vector3.zero;
                heartBonusCount++;
                Player.instance.HeartCollision(i);
            }
        }

        if (Mathf.Abs(obj.transform.position.z - ConstInfo.resetHeartBonusCountPositionZ) < ConstInfo.collisionRange
            && GetChildTransform(obj, 8).localScale.x != 0)
            HandleHeartBonus(obj);
    }

    // 하트 보너스 점수 (한 쌍당 3점이며 네 쌍을 다 먹었을 경우 +15점)
    void HandleHeartBonus(GameObject obj) {
        if (heartBonusCount == 4)
            Player.instance.point += 15;
        else
            Player.instance.point += 3 * heartBonusCount;     

        heartBonusCount = 0;
        GetChildTransform(obj, 8).localScale = Vector3.zero;
    }


    // 장애물 충돌 판정 알고리즘
    void CheckCollisionObstacle(GameObject obj, RuntimeAnimatorController runtimeAnimatorController)
    {
        if (Mathf.Abs(obj.transform.position.z - ConstInfo.collisionPositionZ[0]) < ConstInfo.collisionRange
            && GetChildTransform(obj, 0).localScale.x != 0
            && obj.transform.position.x == Player.instance.highlight.transform.position.x
            )
        {
            if (Player.instance.isJumping)
            {
                if (GetChildTransform(obj, 1).localScale.x != 0)
                {
                    Player.instance.EmptyCollision();
                    GetChildTransform(obj, 1).localScale = Vector3.zero;
                }
            }
            else
            {
                GetChildTransform(obj, 0).localScale = Vector3.zero;
                obj.transform.GetChild(2).GetComponent<Animator>().runtimeAnimatorController = null;
                obj.transform.GetChild(2).GetComponent<Animator>().runtimeAnimatorController = runtimeAnimatorController;
                Player.instance.ObstacleCollision();
                GameFloorTile.InitialStepRecords();
                userSpeed = 0;
            }

        }
    }

    // 풍선 충돌 판정 알고리즘
    void CheckCollisionBalloon(GameObject obj)
    {
        if (Mathf.Abs(obj.transform.position.z - ConstInfo.collisionPositionZ[0]) < ConstInfo.collisionRange
        && GetChildTransform(obj, 1).localScale.x != 0
        && obj.transform.position.x == Player.instance.highlight.transform.position.x
        && Player.instance.isPunching)
        {
            Player.instance.BalloonCollision();
            GetChildTransform(obj, 0).GetComponent<Animator>().runtimeAnimatorController = balloonEvent;
            GetChildTransform(obj, 1).localScale = Vector3.zero;
        }
    }

    // 빈칸 충돌 판정 알고리즘
    void CheckCollisionEmpty(GameObject obj)
    {
        if (Mathf.Abs(obj.transform.position.z - ConstInfo.collisionPositionZ[0]) < ConstInfo.collisionRange
            && GetChildTransform(obj, 0).localScale.x != 0
            && obj.transform.position.x == Player.instance.highlight.transform.position.x)
        {
            GetChildTransform(obj, 0).localScale = Vector3.zero;
            Player.instance.EmptyCollision();
        }
    }



    // 2분의 1 확률 랜덤 함수
    int RandomNum(int size) { return Random.Range(1, size + 1); }

    // 리스트에서 랜덤 오브젝트 반환
    GameObject GetRandomFromList(List<GameObject> list) { return list[Random.Range(0, list.Count)]; }

    // 타일이 최초 지나가는 길로 설정된 길인 지 판별
    bool IsPassDirection(GameObject tile) { return tile.tag == "Heart Tile" || tile.tag == "Pass Tile"; }

    // 자식 오브젝트 tramsform 반환
    Transform GetChildTransform(GameObject obj, int index) { return obj.transform.GetChild(index).transform; }

}
