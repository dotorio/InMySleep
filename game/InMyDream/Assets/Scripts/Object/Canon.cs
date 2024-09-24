using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Canon : MonoBehaviour
{
    private bool isLoaded = false;
    public GameObject loadedObject; // 발사할 물체
    public GameObject breakablePrefab; // 파괴 가능한 물체 프리팹
    public Transform breakableSpawnPoint; // Breakable 객체가 생성될 위치
    public Transform firePoint; // 물체를 발사할 위치
    public float fireForce = 10f; // 발사할 힘

    public AudioSource audioSource; // 소리 재생기
    public AudioClip loadSound; // 장전 소리
    public AudioClip fireSound; // 발사 소리

    public ParticleSystem fireEffect; // 발사 이펙트

    private GameObject currentBreakableObject; // 현재 Breakable 객체

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable") && !isLoaded)
        {
            // 대포를 장전
            isLoaded = true;
            PhotonNetwork.Destroy(other.gameObject); // 기존 물체 파괴
            Debug.Log("대포가 장전되었습니다.");

            // 장전 소리 재생
            PlaySound(loadSound);

            // Breakable 객체 생성
            CreateBreakable();
        }
    }

    private void CreateBreakable()
    {
        if (breakablePrefab != null && breakableSpawnPoint != null)
        {
            currentBreakableObject = PhotonNetwork.Instantiate(breakablePrefab.name, breakableSpawnPoint.position, breakableSpawnPoint.rotation);
            Debug.Log("파괴 가능한 객체가 생성되었습니다.");
        }
    }

    // Breakable 객체가 파괴될 때 호출될 메서드
    public void OnBreakableDestroyed()
    {
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
            GameObject firedObject = PhotonNetwork.Instantiate(loadedObject.name, firePoint.position, firePoint.rotation);

            Rigidbody rb = firedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(firePoint.forward * fireForce, ForceMode.Impulse); // 대포에서 물체 발사
            }

            // 발사 소리 재생
            PlaySound(fireSound);

            // 발사 이펙트 재생
            if (fireEffect != null)
            {
                fireEffect.Play();
            }

            isLoaded = false; // 발사 후 장전 해제
            Debug.Log("대포가 발사되었습니다.");
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // 소리 재생
        }
    }
}
