using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class BombController : MonoBehaviourPunCallbacks
{
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
            if (CompareTag("Bomb") && other.CompareTag("Player"))
            {
                Explode();
            }
            else if (CompareTag("Stone") && other.CompareTag("Boss"))
            {
                Explode();
            }
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

        // 발사체가 제거된 위치에 폭발 효과 생성
        GameObject projectile = PhotonNetwork.Instantiate("Boss/Explosion", projectilePosition, Quaternion.identity);

        photonView.RPC("HandleExplosion", 
            RpcTarget.AllBuffered, 
            projectile.GetComponent<PhotonView>().ViewID, 
            photonView.ViewID);

    }

    [PunRPC]
    public void HandleExplosion(int effectId, int bombId)
    {
        // DestroyEffect 부분
        PhotonView effectPhoton = PhotonView.Find(effectId);
        if (effectPhoton != null)
        {
            Destroy(effectPhoton.gameObject, 0.5f);
        }

        // DestroyBomb 부분
        PhotonView bombPhoton = PhotonView.Find(bombId);
        if (bombPhoton != null)
        {
            Destroy(bombPhoton.gameObject);
        }
    }
}
