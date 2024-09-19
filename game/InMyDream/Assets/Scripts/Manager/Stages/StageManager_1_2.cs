using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class StageManager_1_2 : MonoBehaviourPun, StageManager
{
    // 스폰 위치
    public GameObject Master_1;
    public GameObject Client_1;
    public GameObject Master_2;
    public GameObject Client_2;

    void Start()
    {
        string characterName = (string)PhotonNetwork.LocalPlayer.CustomProperties["character"];
        if (string.IsNullOrEmpty(characterName))
        {
            Debug.LogError("캐릭터가 선택되지 않았습니다.");
            return;
        }

        Debug.Log($"선택된 캐릭터: {characterName}");

        int stage = UserData.instance.stage;
        Transform point;

        if(stage == 1)
        {
            if(PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                point = Master_1.transform;
            }
            else
            {
                point = Client_1.transform;
            }
        }
        else
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                point = Master_2.transform;
            }
            else
            {
                point = Client_2.transform;
            }
        }

        // 선택된 캐릭터 프리팹을 인스턴스화합니다.
        GameObject instantiatedCharacter = PhotonNetwork.Instantiate(characterName, point.position, point.rotation, 0);

        if (instantiatedCharacter == null)
        {
            Debug.LogError("캐릭터 인스턴스화에 실패했습니다.");
        }
        else
        {
            Debug.Log("캐릭터가 성공적으로 인스턴스화되었습니다.");

            // 스킨 설정
            Player localPlayer = PhotonNetwork.LocalPlayer;

            photonView.RPC("RPC_ChangeMaterial", 
                RpcTarget.AllBuffered,
                localPlayer.ActorNumber,
                "Player1".Equals(localPlayer.CustomProperties["character"]),
                (string)localPlayer.CustomProperties["material"]);
        }
    }

    // 스폰 포인트 반환
    public Transform getSpawnPoint()
    {
        int stage = UserData.instance.stage;
        Transform point = stage == 1 ?
            PhotonNetwork.LocalPlayer.IsMasterClient ?
                Master_1.transform : Client_1.transform :
            PhotonNetwork.LocalPlayer.IsMasterClient ?
                Master_2.transform : Client_2.transform;

        return point;
    }

    // 스킨 변경 메소드
    [PunRPC]
    public void RPC_ChangeMaterial(int targetActorNumber, bool isBear, string material)
    {
        string fileName = (isBear ? "Bear" : "Bunny") + material;

        Material newMaterial = Resources.Load<Material>(fileName);

        GameObject[] characters = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject character in characters)
        {
            PhotonView pv = character.GetComponent<PhotonView>();
            if (pv != null)
            {
                if (pv.Owner.ActorNumber == targetActorNumber)
                {
                    Transform mesh = character.transform.GetChild(1);

                    if (mesh != null)
                    {
                        Transform body = mesh.GetChild(0);

                        if (body != null)
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
