using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Shelf : MonoBehaviourPun
{
    public int health;
    public AudioSource damageSound; // 데미지 시 소리
    public AudioClip destructionSound; // 파괴 시 소리
    public GameObject destructionEffect; // 파괴 시 이펙트

    public GameObject book1;
    public GameObject book2;


    private void Start()
    {
        health = 5;
    }
    private void Update()
    {
        if (health <= 0)
        {
            // 파괴 이펙트 생성
            if (destructionEffect != null)
            {
                Instantiate(destructionEffect, transform.position, transform.rotation);
            }

            // 파괴 사운드 재생
            if (destructionSound != null)
            {
                AudioSource.PlayClipAtPoint(destructionSound, transform.position);
            }

            Rigidbody book1Rb = book1.AddComponent<Rigidbody>();
            book1Rb.useGravity = true;
            book1Rb.isKinematic = false;

            Rigidbody book2Rb = book2.AddComponent<Rigidbody>();
            book2Rb.useGravity = true;
            book2Rb.isKinematic = false;


            Destroy(gameObject);
            Debug.Log("책장 받침이 파괴되었습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Grabable") && PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            health--;
            if (damageSound != null)
            {
                damageSound.Play();
            }

            photonView.RPC("SyncHealth", RpcTarget.AllBuffered, health);
            photonView.RPC("SyncBallEffect", RpcTarget.AllBuffered, other.GetComponent<PhotonView>().ViewID);
            PhotonNetwork.Instantiate("BallEffect", other.transform.position, Quaternion.identity);
        }
    }

    [PunRPC]
    public void SyncHealth(int newHealth)
    {
        health = newHealth;
    }

    [PunRPC]
    public void SyncBallEffect(int ballID)
    {
        PhotonView ball = PhotonView.Find(ballID);
        if (ball != null)
        {
            Destroy(ball.gameObject);
        }
    }
}
