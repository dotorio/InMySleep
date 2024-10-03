//using GLTF.Schema;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanonButton : MonoBehaviour
{
    public GameObject canonObject; // Canon 객체
    public GameObject spark; // Spark 프리팹
    public GameObject bomb; // Spark 프리팹
    private GameObject spawnedSpark; // 생성된 Spark 객체
    private Canon cannonScript;
    private List<GameObject> playersInTrigger = new List<GameObject>(); // 트리거 안에 있는 플레이어들을 추적

    private void Start()
    {
        if (spark != null)
        {
            // CanonButton의 위치에서 Spark를 생성
            spawnedSpark = Instantiate(spark, transform.position, Quaternion.identity);
        }
        // 캐논 오브젝트에서 Cannon 스크립트 가져오기
        if (canonObject != null)
        {
            cannonScript = canonObject.GetComponent<Canon>();

            if (cannonScript == null)
            {
                Debug.LogError("Cannon 스크립트를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("Cannon 오브젝트가 할당되지 않았습니다.");
        }
    }

    

    //// 트리거 콜라이더에 들어왔을 때
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player")) // Player 태그를 가진 오브젝트만 감지
    //    {
    //        playersInTrigger.Add(other.gameObject); // 트리거 안에 들어온 플레이어 추가
    //    }
    //}

    //// 트리거 콜라이더에서 나갔을 때
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playersInTrigger.Remove(other.gameObject); // 트리거에서 나간 플레이어 제거
    //    }
    //}

    //// 키 입력을 감지
    //void Update()
    //{
    //    Debug.Log(playersInTrigger.Count);
    //    foreach (GameObject player in playersInTrigger)
    //    {
    //        // 트리거 안에 있는 플레이어 중 하나가 F 키를 눌렀는지 확인
    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
    //            Debug.Log("ㅁㅁㅁㅁ");
    //            // 해당 플레이어가 F 키를 눌렀다면
    //            if (player == GetPlayerWhoPressedF())
    //            {
    //                cannonScript.OnBreakableDestroyed();
    //                PerformAction(); // 플레이어가 F키를 눌렀을 때 실행할 함수 호출
    //            }
    //        }
    //    }
    //}

    //// F 키를 누른 플레이어를 찾는 함수
    //private GameObject GetPlayerWhoPressedF()
    //{
    //    // 여기서 원하는 방식으로 특정 플레이어의 입력을 확인할 수 있음
    //    // 예를 들어 네트워크 게임이라면, 특정 네트워크 ID로 플레이어를 구분할 수 있음
    //    // 기본적으로는 로컬에서 Input을 체크하는 예시임

    //    // 트리거 안에 들어온 플레이어 중 실제로 F 키를 누른 플레이어를 반환
    //    foreach (GameObject player in playersInTrigger)
    //    {
    //        // 특정 조건을 넣을 수 있음, 예: 특정 키 입력을 개별적으로 처리하고 싶을 때
    //        // 여기서는 단순히 첫 번째 플레이어가 F 키를 눌렀다고 가정
    //        return player;
    //    }

    //    return null;
    //}

    //[PunRPC]
    //private void PerformAction()
    //{
    //    Debug.Log("PerformAction 함수가 실행되었습니다.");

    //    if (PhotonNetwork.IsMasterClient) // 네트워크에서 마스터 클라이언트가 파괴를 실행
    //    {
    //        Debug.Log("MasterClient");
    //        if (spawnedSpark != null)
    //        {
    //            PhotonNetwork.Destroy(spawnedSpark); // 네트워크에서 Spark 객체 파괴
    //        }
    //        PhotonNetwork.Destroy(gameObject); // 네트워크에서 CanonButton 오브젝트 파괴
    //    }
    //}

    private void OnDestroy()
    {
        Destroy(spark);
        cannonScript.OnBreakableDestroyed();
    }
}

