//using GLTF.Schema;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanonButton : MonoBehaviour
{
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
    }

    public void SetCanon(Canon canon)
    {
        cannonScript = canon;
    }

    private void OnDestroy()
    {
        Destroy(spawnedSpark);

        if (cannonScript != null)
        {
            cannonScript.Fire();
        }
    }
}

