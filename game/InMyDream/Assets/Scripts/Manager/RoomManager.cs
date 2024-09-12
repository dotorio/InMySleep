using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPun
{
    private GameObject player;
    public Transform[] spawnPoints;

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
