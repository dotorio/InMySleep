// using System.Collections.Generic;
// using UnityEngine;

// public class TileManager : MonoBehaviour
// {
//     public GameObject[] tilePrefabs; // 여러 타일 프리팹 배열
//     public float tileLength = 5f;     // 타일 하나의 길이 (Z축 길이)
//     public int initialTiles = 5;      // 처음에 생성할 타일 수
//     public Transform player;           // 플레이어 위치
//     private float spawnZ = 0.0f;      // 다음 타일이 스폰될 Z 위치
//     private float safeZone = 10.0f;   // 플레이어가 타일 앞에서 삭제되는 타일 거리
//     private List<GameObject> activeTiles; // 활성화된 타일 목록

//     void Start()
//     {
//         activeTiles = new List<GameObject>();

//         // 초기 타일 생성
//         for (int i = 0; i < initialTiles; i++)
//         {
//             SpawnTile(); // 최초 타일 스폰
//         }
//     }

//     void Update()
//     {
//         // 플레이어가 타일 끝 쪽에 가까워지면 새로운 타일 스폰
//         if (player.position.z + safeZone > spawnZ)
//         {
//             SpawnTile();
//             DeleteTile();
//         }
//     }

//     // 타일 생성 함수
//     void SpawnTile()
//     {
//         // 랜덤 프리팹 선택
//         GameObject tile = Instantiate(tilePrefabs[Random.Range(0, tilePrefabs.Length)]) as GameObject;
//         tile.transform.SetParent(transform);

//         // 타일을 붙여서 생성
//         tile.transform.position = new Vector3(0, 0, spawnZ); // 중앙에서 붙도록
//         spawnZ += tileLength;  // 다음 타일의 스폰 위치를 타일 길이만큼 증가시킴

//         activeTiles.Add(tile);
//     }

//     // 오래된 타일 삭제
//     void DeleteTile()
//     {
//         if (activeTiles.Count > 0) // 삭제할 타일이 있을 경우에만 삭제
//         {
//             Destroy(activeTiles[0]);
//             activeTiles.RemoveAt(0);
//         }
//     }
// }


using System.Collections.Generic;
using UnityEngine;

public class InfiniteTileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs; // 타일 프리팹 배열
    public Transform player; // 플레이어 트랜스폼
    public float spawnDistance = 50f; // 타일 생성 거리
    public int tileCount = 5; // 보유할 타일 수

    private Queue<GameObject> activeTiles; // 활성 타일 큐
    private Vector3 lastSpawnPosition;

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

        // 초기 타일 생성 (1, 2, 3, 4, 5 순서)
        for (int i = 0; i < tileCount; i++)
        {
            SpawnTile(i % tilePrefabs.Length); // 인덱스를 사용하여 순서대로 생성
        }
    }

    void Update()
    {
        // 플레이어가 끝에 가까워지면 타일을 생성
        if (Vector3.Distance(player.position, lastSpawnPosition) < spawnDistance)
        {
            // 랜덤한 타일 생성, 배열이 비어있는지 다시 한 번 확인
            if (tilePrefabs.Length > 0)
            {
                SpawnTile(Random.Range(0, tilePrefabs.Length));
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
        tile.transform.position = lastSpawnPosition + new Vector3(0, 0, 15); // 타일의 위치 조정
        lastSpawnPosition = tile.transform.position; // 마지막 생성된 타일 위치 업데이트
        activeTiles.Enqueue(tile); // 큐에 추가
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
