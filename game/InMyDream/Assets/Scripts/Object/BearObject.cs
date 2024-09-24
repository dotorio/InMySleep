using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BearObject : MonoBehaviour
{
    public GameObject smallObjectPrefab; // 생성할 작은 물체 프리팹

    private void OnDestroy()
    {
        int numberOfSmallObjects = Random.Range(3, 5); // 생성할 작은 물체 수

        // 작은 물체를 여러 개 생성 (모든 클라이언트에 동기화)
        for (int i = 0; i < numberOfSmallObjects; i++)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere; // 무작위 위치
            PhotonNetwork.Instantiate(smallObjectPrefab.name, spawnPosition, Quaternion.identity);
        }
    }
}
