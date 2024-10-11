using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Canon : MonoBehaviourPunCallbacks
{
    private bool isLoaded = false;
    public Transform fireButtonPoint;
    public Transform firePoint; // 물체를 발사할 위치
    public Transform target;

    public AudioSource loadSource; // 소리 재생기 (장전)
    public AudioSource fireSource; // 소리 재생기 (발사)

    public GameObject fireEffect; // 발사 이펙트

    public GameObject fireObject; // 현재 폭탄 객체

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable") && !isLoaded && PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (CreateBreakable())
            {
                photonView.RPC("LoadRPC",
                    RpcTarget.AllBuffered,
                    other.GetComponent<PhotonView>().ViewID);
            }
        }
    }

    [PunRPC]
    public void LoadRPC(int objectId)
    {
        PhotonView objectIdPhoton = PhotonView.Find(objectId);

        if(objectIdPhoton != null)
        {
            // Breakable 객체 생성
            Destroy(objectIdPhoton.gameObject); // 기존 물체 파괴
            Debug.Log("대포가 장전되었습니다.");

            // 장전 소리 재생
            loadSource.Play();

            // 장전 상태 업데이트
            isLoaded = true;
            fireObject.SetActive(true);
        }
    }

    private bool CreateBreakable()
    {
        if(!isLoaded)
        {
            if(PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                GameObject fireButton = PhotonNetwork.Instantiate("CanonButton", fireButtonPoint.position, fireButtonPoint.rotation);

                if(fireButton == null)
                {
                    Debug.Log("버튼 생성에 실패했습니다.");
                    return false;
                }
                else
                {
                    fireButton.GetComponent<CanonButton>().SetCanon(this);
                    Debug.Log("버튼 생성에 성공했습니다.");
                    return true;
                }
            }
        }
        return false;
    }

    public void Fire()
    {
        if (isLoaded && PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            // 장전된 물체를 생성
            // 포탄 자체의 코드로 포탄을 발사할 예정
            GameObject realFireObject = PhotonNetwork.Instantiate("Boss/FireBomb", firePoint.position, firePoint.rotation);
            
            if(realFireObject != null)
            {
                UserBombController controller = realFireObject.GetComponent<UserBombController>();

                if(controller != null)
                {
                    controller.SetTarget(target);
                }

                photonView.RPC("FireRPC", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    public void FireRPC()
    {
        // 발사 소리 재생
        fireSource.Play();

        // 발사 이펙트 재생
        StartCoroutine(SpawnAndDestroyObject());

        isLoaded = false; // 발사 후 장전 해제
        Debug.Log("대포가 발사되었습니다.");

        // 장전된 포탄 제거
        fireObject.SetActive(false);
    }

    IEnumerator SpawnAndDestroyObject()
    {
        GameObject spawnedObject = Instantiate(fireEffect, firePoint.position, firePoint.rotation); // firePoint 위치에 오브젝트 생성
        yield return new WaitForSeconds(1f); // 1초 대기
        Destroy(spawnedObject); // 1초 후에 오브젝트 제거
    }
}
