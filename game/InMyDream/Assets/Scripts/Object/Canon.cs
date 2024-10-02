using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Canon : MonoBehaviourPunCallbacks
{
    private bool isLoaded = false;
    public GameObject loadedObject; // 발사할 물체
    public GameObject breakablePrefab; // 파괴 가능한 물체 프리팹
    public GameObject boss; // 파괴 가능한 물체 프리팹
    public Transform breakableSpawnPoint; // Breakable 객체가 생성될 위치
    public Transform firePoint; // 물체를 발사할 위치

    public AudioSource loadSource; // 소리 재생기
    public AudioSource fireSource; // 소리 재생기
    //public AudioClip loadSound; // 장전 소리
    //public AudioClip fireSound; // 발사 소리

    public ParticleSystem fireEffect; // 발사 이펙트

    private GameObject currentBreakableObject; // 현재 Breakable 객체
    private GameObject loadedInstance; // 현재 Breakable 객체

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable") && !isLoaded)
        {
            // 대포를 장전
            isLoaded = true;
            PhotonNetwork.Destroy(other.gameObject); // 기존 물체 파괴
            Debug.Log("대포가 장전되었습니다.");
            //photonView.RPC("DestroyBombRPC", RpcTarget.AllBuffered);
            //

            // 장전 소리 재생
            loadSource.Play();

            // Breakable 객체 생성
            CreateBreakable();
        }
    }

    //[PunRPC]
    //public void DestroyBombRPC()
    //{
    //    //PhotonNetwork.Destroy(other.gameObject);
    //}

    private void CreateBreakable()
    {
        if (breakablePrefab != null && breakableSpawnPoint != null)
        {
            currentBreakableObject = Instantiate(breakablePrefab, breakableSpawnPoint.position, breakableSpawnPoint.rotation);
            // loadedObject 생성
            loadedInstance = Instantiate(loadedObject, firePoint.position, firePoint.rotation);

            // loadedObject 고정
            if (loadedInstance.GetComponent<Rigidbody>() != null)
            {
                loadedInstance.GetComponent<Rigidbody>().isKinematic = true;  // 물리적으로 고정
            };
            isLoaded = true;   
        }
    }

    // Breakable 객체가 파괴될 때 호출될 메서드
    public void OnBreakableDestroyed()
    {
        Debug.Log("OnBreakableDestroyed 함수가 호출되었습니다.");
        if (isLoaded)
        {
            Fire(); // 대포 발사
        }
    }

    private void Fire()
    {
        if (isLoaded && loadedObject != null)
        {
            // 장전된 물체를 생성하고 발사
            

            Rigidbody rb = loadedInstance.GetComponent<Rigidbody>();
            loadedInstance.GetComponent<Rigidbody>().isKinematic = false;
            Vector3 launchDirection = CalculateLaunchDirection(firePoint.position, boss.transform.position, 20f);
            rb.velocity = launchDirection;

            // 발사 소리 재생
            fireSource.Play();

            // 발사 이펙트 재생
            if (fireEffect != null)
            {
                fireEffect.Play();
            }

            isLoaded = false; // 발사 후 장전 해제
            Debug.Log("대포가 발사되었습니다.");
        }
    }

    Vector3 CalculateLaunchDirection(Vector3 player, Vector3 target, float initialAngle)
    {
        float gravity = Physics.gravity.magnitude;
        float angle = initialAngle * Mathf.Deg2Rad;

        Vector3 planarTarget = new Vector3(target.x, 0, target.z);
        Vector3 planarPosition = new Vector3(player.x, 0, player.z);

        float distance = Vector3.Distance(planarTarget, planarPosition);
        float yOffset = player.y - target.y;

        float initialVelocity
            = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity
            = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        float angleBetweenObjects
            = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (target.x > player.x ? 1 : -1);


        Vector3 finalVelocity
            = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
        return finalVelocity;


    }

    //private void PlaySound(AudioSource AS)
    //{
    //    AS.Play();           // 사운드 재생
    //}
}
