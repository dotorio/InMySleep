using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPun
{
    private GameObject player;
    public Transform[] spawnPoints;
    public GameObject arrowPositionMaster; // 1번 위치의 화살표
    public GameObject arrowPositionGuest; // 2번 위치의 화살표
    // 캐릭터 생성
    public void CreatePlayer()
    {
        string character = (string)PhotonNetwork.LocalPlayer.CustomProperties["character"];

        Transform point;

        if(PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            point = spawnPoints[0];
        }
        else
        {
            point = spawnPoints[1];
        }

        player = PhotonNetwork.Instantiate(character + "Room", point.position, point.rotation, 0);

        if (player == null)
        {
            Debug.LogError("캐릭터 인스턴스화에 실패했습니다.");
        }
        else
        {
            Debug.Log("캐릭터가 성공적으로 인스턴스화되었습니다.");
            // Master Client 여부에 따라 화살표 활성화 위치 결정
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                // Master Client일 경우 1번 위치의 화살표 활성화
                arrowPositionMaster.SetActive(true);
                arrowPositionGuest.SetActive(false); // 2번 위치의 화살표 비활성화
                Debug.Log("1번 위치 화살표 활성화");
            }
            else
            {
                // Master Client가 아닐 경우 2번 위치의 화살표 활성화
                arrowPositionMaster.SetActive(false); // 1번 위치의 화살표 비활성화
                arrowPositionGuest.SetActive(true);
                Debug.Log("2번 위치 화살표 활성화");
            }
            
        }
    }

    // 캐릭터 변경
    public void ChangeCharacter()
    {
        PhotonNetwork.Destroy(player);

        CreatePlayer();
    }

    // 캐릭터 스킨
    public void ChangeMaterial(int targetActorNumber, bool isBear, string material)
    {
        photonView.RPC("RPC_ChangeMaterial", RpcTarget.AllBuffered, targetActorNumber, isBear, material);
    }

    [PunRPC]
    public void RPC_ChangeMaterial(int targetActorNumber, bool isBear, string material)
    {
        string fileName = (isBear ? "Bear" : "Bunny") + material;

        Material newMaterial = Resources.Load<Material>(fileName);

        GameObject[] characters = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject character in characters)
        {
            PhotonView pv = character.GetComponent<PhotonView>();
            if(pv != null)
            {
                if(pv.Owner.ActorNumber == targetActorNumber)
                {
                    Transform mesh = character.transform.GetChild(1);
                    
                    if(mesh != null)
                    {
                        Transform body = mesh.GetChild(0);
                        
                        if(body != null)
                        {
                            Renderer renderer = body.GetComponent<Renderer>();
                            if (renderer != null)
                            {
                                renderer.material = newMaterial;
                            }
                        }
                    }
                }
            }
        }
    }
}
