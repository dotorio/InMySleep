using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BunnyObject : MonoBehaviour
{
    public GameObject smallObjectPrefab; // 생성할 작은 물체 프리팹
    public GameObject effectPrefab; // 부딪히는 이펙트 프리팹
    public AudioClip sound; // 부딪히는 소리 클립
    private AudioSource audioSource; // 오디오 소스 컴포넌트

    private void Start()
    {
        // 오디오 소스 설정
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // 파괴 이펙트 생성 (모든 클라이언트에 동기화)
        if (effectPrefab != null)
        {
            PhotonNetwork.Instantiate(effectPrefab.name, transform.position, Quaternion.identity);
        }

        // 소리 재생
        if (sound != null)
        {
            audioSource.PlayOneShot(sound);
        }

        // 작은 물체를 여러 개 생성 (모든 클라이언트에 동기화)
        int numberOfSmallObjects = Random.Range(3, 5);
        for (int i = 0; i < numberOfSmallObjects; i++)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere; // 무작위 위치
            PhotonNetwork.Instantiate(smallObjectPrefab.name, spawnPosition, Quaternion.identity);
        }
    }
}
