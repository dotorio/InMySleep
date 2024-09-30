using System.Collections.Generic;
using UnityEngine;

public class InfiniteTileManager : MonoBehaviour
{
    public StageManager_3 stageManager;
    public GameObject[] tilePrefabs; // 타일 프리팹 배열 (7개로 설정)
    public Transform player; // 플레이어 트랜스폼
    public float spawnDistance = 50f; // 타일 생성 거리
    public int tileCount = 5; // 보유할 타일 수

    private Queue<GameObject> activeTiles; // 활성 타일 큐
    private Vector3 lastSpawnPosition;
    private int spawnedTileCount = 0; // 생성된 타일 수 추적
    private bool sixTileSpawned = false; // 6번 타일이 생성되었는지 추적
    private bool sevenTileSpawned = false; // 7번 타일이 생성되었는지 추적 (추가)

    void Start()
    {
        // tilePrefabs 배열이 비어있는지 확인
        if (tilePrefabs == null || tilePrefabs.Length == 0)
        {
            Debug.LogError("tilePrefabs 배열이 비어 있습니다. 타일 프리팹을 설정하세요.");
            return; // 배열이 비어 있으면 실행 중단
        }

        activeTiles = new Queue<GameObject>();
        lastSpawnPosition = player.position;
    }

    void Update()
    {
        // 플레이어가 끝에 가까워지면 타일을 생성
        if (Vector3.Distance(player.position, lastSpawnPosition) < spawnDistance)
        {
            // 7번 타일이 아직 생성되지 않았고, 현재 타일 카운트가 10 이상이면 7번 타일 생성 (추가)
            if (spawnedTileCount >= 10 && !sevenTileSpawned)
            {
                SpawnTile(6); // 6번 타일 생성
                sevenTileSpawned = true; // 6번 타일 생성 상태 업데이트
            }
            // 6번 타일이 아직 생성되지 않았고, 현재 타일 카운트가 20 이상이면 6번 타일 생성
            else if (spawnedTileCount == 20 && !sixTileSpawned)
            {
                SpawnTile(7); // 7번 타일 생성
                sixTileSpawned = true; // 7번 타일 생성 상태 업데이트
            }
            else if (spawnedTileCount < 20)
            {
                // 20번째 타일까지 랜덤한 타일 생성 (6번, 7번 타일 제외)
                int tileIndex = Random.Range(0, tilePrefabs.Length - 2);
                SpawnTile(tileIndex);
            }

            RemoveOldTile();
        }
    }

    void SpawnTile(int prefabIndex)
    {
        // 인덱스가 배열 범위를 벗어나는지 확인
        if (prefabIndex < 0 || prefabIndex >= tilePrefabs.Length)
        {
            Debug.LogError("잘못된 prefabIndex: " + prefabIndex);
            return;
        }

        // 특정 인덱스의 타일 프리팹 선택
        GameObject tile = Instantiate(tilePrefabs[prefabIndex]);

        GameObject spawnPoint = tile.Find("SpawnPoint");
        if (spawnPoint!=null)
        {
            stageManager.AddSpawnPoint(spawnPoint);
        }
        

        tile.transform.position = lastSpawnPosition + new Vector3(0, 0, 15); // 타일의 위치 조정
        lastSpawnPosition = tile.transform.position; // 마지막 생성된 타일 위치 업데이트
        activeTiles.Enqueue(tile); // 큐에 추가

        spawnedTileCount++; // 타일 카운트 증가
    }

    void RemoveOldTile()
    {
        // 큐에서 가장 오래된 타일 제거
        if (activeTiles.Count > tileCount)
        {
            GameObject oldTile = activeTiles.Dequeue();
            Destroy(oldTile); // 타일 삭제
        }
    }
}
