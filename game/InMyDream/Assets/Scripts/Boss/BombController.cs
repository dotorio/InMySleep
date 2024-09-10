using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject explode;
    private bool hasExploded = false; // 중복 폭발 방지

    // 카운트다운을 시작하는 메서드
    public void StartDestroyCountdown(float delay)
    {
        StartCoroutine(DestroyAndExplode(delay));
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!hasExploded) // 이미 폭발하지 않았을 경우에만 실행
        {
            Explode();
        }
    }

    // 일정 시간 후에 발사체를 제거하고 폭발 효과 생성
    IEnumerator DestroyAndExplode(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!hasExploded) // 이미 폭발하지 않았을 경우에만 실행
        {
            Explode();
        }
    }

    // 폭발 처리
    private void Explode()
    {
        hasExploded = true;

        // 발사체의 현재 위치 저장
        Vector3 projectilePosition = transform.position;

        // 발사체 제거
        Destroy(gameObject);

        // 발사체가 제거된 위치에 폭발 효과 생성
        GameObject projectile = Instantiate(explode, projectilePosition, Quaternion.identity);
        Destroy(projectile, 1f); // 1초 후에 폭발 효과 제거
    }
}
