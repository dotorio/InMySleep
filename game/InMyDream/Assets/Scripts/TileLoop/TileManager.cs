// // using System.Collections.Generic;
// // using UnityEngine;

// // public class InfiniteTileManager : MonoBehaviour
// // {
// //     public GameObject[] tilePrefabs; // 타일 프리팹 배열
// //     public Transform player; // 플레이어 트랜스폼
// //     public float spawnDistance = 50f; // 타일 생성 거리
// //     public int tileCount = 5; // 보유할 타일 수

// //     private Queue<GameObject> activeTiles; // 활성 타일 큐
// //     private Vector3 lastSpawnPosition;

// //     void Start()
// //     {
// //         // tilePrefabs 배열이 비어있는지 확인
// //         if (tilePrefabs == null || tilePrefabs.Length == 0)
// //         {
// //             Debug.LogError("tilePrefabs 배열이 비어 있습니다. 타일 프리팹을 설정하세요.");
// //             return; // 배열이 비어 있으면 실행 중단
// //         }

// //         activeTiles = new Queue<GameObject>();
// //         lastSpawnPosition = player.position;

// //         // 초기 타일 생성 (1, 2, 3, 4, 5 순서)
// //         for (int i = 0; i < tileCount; i++)
// //         {
// //             SpawnTile(i % tilePrefabs.Length); // 인덱스를 사용하여 순서대로 생성
// //         }
// //     }

// //     void Update()
// //     {
// //         // 플레이어가 끝에 가까워지면 타일을 생성
// //         if (Vector3.Distance(player.position, lastSpawnPosition) < spawnDistance)
// //         {
// //             // 랜덤한 타일 생성, 배열이 비어있는지 다시 한 번 확인
// //             if (tilePrefabs.Length > 0)
// //             {
// //                 SpawnTile(Random.Range(0, tilePrefabs.Length));
// //             }
// //             RemoveOldTile();
// //         }
// //     }

// //     void SpawnTile(int prefabIndex)
// //     {
// //         // 인덱스가 배열 범위를 벗어나는지 확인
// //         if (prefabIndex < 0 || prefabIndex >= tilePrefabs.Length)
// //         {
// //             Debug.LogError("잘못된 prefabIndex: " + prefabIndex);
// //             return;
// //         }

// //         // 특정 인덱스의 타일 프리팹 선택
// //         GameObject tile = Instantiate(tilePrefabs[prefabIndex]);
// //         tile.transform.position = lastSpawnPosition + new Vector3(0, 0, 15); // 타일의 위치 조정
// //         lastSpawnPosition = tile.transform.position; // 마지막 생성된 타일 위치 업데이트
// //         activeTiles.Enqueue(tile); // 큐에 추가
// //     }

// //     void RemoveOldTile()
// //     {
// //         // 큐에서 가장 오래된 타일 제거
// //         if (activeTiles.Count > tileCount)
// //         {
// //             GameObject oldTile = activeTiles.Dequeue();
// //             Destroy(oldTile); // 타일 삭제
// //         }
// //     }
// // }

// using System.Collections.Generic;
// using UnityEngine;

// public class InfiniteTileManager : MonoBehaviour
// {
//     public GameObject[] tilePrefabs; // 타일 프리팹 배열
//     public Transform player; // 플레이어 트랜스폼
//     public float spawnDistance = 50f; // 타일 생성 거리
//     public int tileCount = 5; // 보유할 타일 수

//     private Queue<GameObject> activeTiles; // 활성 타일 큐
//     private Vector3 lastSpawnPosition;
//     private int spawnedTileCount = 0; // 생성된 타일 수 추적

//     void Start()
//     {
//         // tilePrefabs 배열이 비어있는지 확인
//         if (tilePrefabs == null || tilePrefabs.Length == 0)
//         {
//             Debug.LogError("tilePrefabs 배열이 비어 있습니다. 타일 프리팹을 설정하세요.");
//             return; // 배열이 비어 있으면 실행 중단
//         }

//         activeTiles = new Queue<GameObject>();
//         lastSpawnPosition = player.position;

//         // 초기 타일 생성 (1, 2, 3, 4, 5 순서)
//         for (int i = 0; i < tileCount; i++)
//         {
//             SpawnTile(i % tilePrefabs.Length); // 인덱스를 사용하여 순서대로 생성
//         }
//     }

//     void Update()
//     {
//         // 플레이어가 끝에 가까워지면 타일을 생성
//         if (Vector3.Distance(player.position, lastSpawnPosition) < spawnDistance)
//         {
//             // 20번째 타일에서 6번 타일을 생성하고 이후 생성을 중지
//             if (spawnedTileCount < 20)
//             {
//                 // 랜덤한 타일 생성, 배열이 비어있는지 다시 한 번 확인
//                 if (tilePrefabs.Length > 0)
//                 {
//                     SpawnTile(Random.Range(0, tilePrefabs.Length));
//                     spawnedTileCount++;
//                 }
//             }
//             else if (spawnedTileCount == 20)
//             {
//                 SpawnTile(6); // 6번 타일 생성
//                 spawnedTileCount++; // 타일 카운트 증가
//             }

//             RemoveOldTile();
//         }
//     }

//     void SpawnTile(int prefabIndex)
//     {
//         // 인덱스가 배열 범위를 벗어나는지 확인
//         if (prefabIndex < 0 || prefabIndex >= tilePrefabs.Length)
//         {
//             Debug.LogError("잘못된 prefabIndex: " + prefabIndex);
//             return;
//         }

//         // 특정 인덱스의 타일 프리팹 선택
//         GameObject tile = Instantiate(tilePrefabs[prefabIndex]);
//         tile.transform.position = lastSpawnPosition + new Vector3(0, 0, 15); // 타일의 위치 조정
//         lastSpawnPosition = tile.transform.position; // 마지막 생성된 타일 위치 업데이트
//         activeTiles.Enqueue(tile); // 큐에 추가
//     }

//     void RemoveOldTile()
//     {
//         // 큐에서 가장 오래된 타일 제거
//         if (activeTiles.Count > tileCount)
//         {
//             GameObject oldTile = activeTiles.Dequeue();
//             Destroy(oldTile); // 타일 삭제
//         }
//     }
// }
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs; // 타일 프리팹 배열 (6개로 설정)
    public Transform player; // 플레이어 트랜스폼
    public float spawnDistance = 50f; // 타일 생성 거리
    public int tileCount = 5; // 보유할 타일 수

    private Queue<GameObject> activeTiles; // 활성 타일 큐
    private Vector3 lastSpawnPosition;
    private int spawnedTileCount = 0; // 생성된 타일 수 추적
    private bool sixTileSpawned = false; // 6번 타일이 생성되었는지 추적

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
            // 6번 타일이 아직 생성되지 않았고, 현재 타일 카운트가 20 이상이면 6번 타일 생성
            if (spawnedTileCount == 20 && !sixTileSpawned)
            {
                SpawnTile(6); // 6번 타일 생성
                sixTileSpawned = true; // 6번 타일 생성 상태 업데이트
            }
            else if (spawnedTileCount < 20)
            {
                // 20번째 타일까지 랜덤한 타일 생성 (6번 타일은 제외)
                SpawnTile(Random.Range(0, tilePrefabs.Length - 1));
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

