using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class StageManager_4 : MonoBehaviourPun, StageManager
{
    // 스폰 위치
    public GameObject Master_1;
    public GameObject Client_1;
    public GameObject Master_2;
    public GameObject Client_2;
    public GameObject Master_3;
    public GameObject Client_3;
    public GameObject EasterEgg;

    // 2 페이즈 물체 설정
    public GameObject JumpMap;
    public GameObject Disappearable;
    public GameObject Boss1;
    public GameObject Boss2;

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
        int easterStage = (int)PhotonNetwork.CurrentRoom.CustomProperties["EasterEggStage"];
        Transform point;

        if (stage == 4)
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
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
            // 점프맵 생성
            JumpMap.SetActive(true);

            // 벽 제거
            Disappearable.SetActive(false);

            // 보스 생성
            Boss2.SetActive(true);

            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                point = Master_2.transform;

                if (easterStage == 4)
                {
                    GameObject instantiatedEasterEgg = PhotonNetwork.Instantiate(characterName, EasterEgg.transform.position, EasterEgg.transform.rotation, 0);

                    if (instantiatedEasterEgg == null)
                    {
                        Debug.Log("이스터 에그 생성에 실패했습니다.");
                    }
                    else
                    {
                        Debug.Log("이스터 에그 생성에 성공했습니다.");
                    }
                }
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

            // controller에 stage manager 설정
            ThirdPersonController controller = instantiatedCharacter.GetComponent<ThirdPersonController>();

            if (controller != null)
            {
                controller.SetStageManager(this);
            }

            photonView.RPC("AddPlayer2Boss",
                RpcTarget.AllBuffered,
                instantiatedCharacter.GetComponent<PhotonView>().ViewID);

            // 스킨 설정
            Player localPlayer = PhotonNetwork.LocalPlayer;

            photonView.RPC("RPC_ChangeMaterial", 
                RpcTarget.AllBuffered,
                localPlayer.ActorNumber,
                "Player1".Equals(localPlayer.CustomProperties["character"]),
                (string)localPlayer.CustomProperties["material"]);
        }
    }

    [PunRPC]
    public void AddPlayer2Boss(int playerPhotonId)
    {
        PhotonView playerPhoton = PhotonView.Find(playerPhotonId);

        if (playerPhoton != null)
        {
            Boss1.GetComponent<CatController>().players.Add(playerPhoton.transform);
            Boss2.GetComponent<CatController>().players.Add(playerPhoton.transform);
        }
    }

    // 스폰 포인트 반환
    public Transform getSpawnPoint()
    {
        int stage = UserData.instance.stage;
        Transform point = stage == 4 ?
            PhotonNetwork.LocalPlayer.IsMasterClient ?
                Master_1.transform : Client_1.transform :
            stage == 5 ?
                PhotonNetwork.LocalPlayer.IsMasterClient ?
                    Master_2.transform : Client_2.transform :
                PhotonNetwork.LocalPlayer.IsMasterClient ?
                    Master_3.transform : Client_3.transform;

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
