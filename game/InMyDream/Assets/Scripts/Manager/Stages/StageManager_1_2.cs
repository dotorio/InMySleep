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
    public GameObject EasterEgg_1;
    public GameObject EasterEgg_2;
    public GameObject[] Batterys;

    public AudioSource Stage2BGM;
    public AudioSource Stage21BGM;

    public GameObject Dogs;

    private int battery;

    void Start()
    {
        battery = 0;

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

        if(stage == 1)
        {
            Dogs.SetActive(false);

            if(PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                point = Master_1.transform;

                if(stage == easterStage)
                {
                    GameObject instantiatedEasterEgg = PhotonNetwork.Instantiate("EasterEgg", EasterEgg_1.transform.position, EasterEgg_1.transform.rotation, 0);

                    if(instantiatedEasterEgg == null)
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
                point = Client_1.transform;
            }
        }
        else
        {
            Stage2BGM.Play();
            Stage21BGM.Play();

            foreach (var Battery in Batterys)
            {
                Battery.SetActive(false);
            }

            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                point = Master_2.transform;


                if (stage == easterStage)
                {
                    GameObject instantiatedEasterEgg = PhotonNetwork.Instantiate("EasterEgg", EasterEgg_2.transform.position, EasterEgg_2.transform.rotation, 0);

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

            if(controller != null)
            {
                controller.SetStageManager(this);
            }

            // 카메라 설정
            CameraSwitcher camera = instantiatedCharacter.GetComponent<CameraSwitcher>();
            if(camera != null)
            {
                camera.SwitchToFollowCamera();
            }


            // 개 설정
            if (stage == 2)
            {
                foreach (Transform dog in Dogs.transform)
                {
                    if (dog == null)
                    {
                        Debug.LogError("dog 객체가 null입니다.");
                        return; // 또는 적절한 대체 로직
                    }

                    var dogMovement = dog.gameObject.GetComponent<DogMovement>();
                    if (dogMovement == null)
                    {
                        Debug.LogError("SpotlightPlayerDetector가 dog 객체에 없습니다.");
                        continue; // 다음 dog로 넘어감
                    }

                    PhotonView playerPhotonView = instantiatedCharacter.GetPhotonView();

                    if (playerPhotonView == null)
                    {
                        Debug.LogError("playerPhotonView 객체가 null입니다.");
                        return; // 또는 적절한 대체 로직
                    }

                    dogMovement.addPlayer(playerPhotonView.ViewID);
                }
            }

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

    public int getBattery()
    {
        return battery;
    }

    // 배터리 흭득 시 호출하는 메소드
    public void CollectBattery(int batteryViewId)
    {
        battery++;

        photonView.RPC("SyncBattery", RpcTarget.All, battery, batteryViewId);
    }

    [PunRPC]
    void SyncBattery(int newBatteryCount, int batteryViewId)
    {
        battery = newBatteryCount;
        Debug.Log("Battery count synchronized");

        // 전달받은 ViewID로 배터리 오브젝트 찾기
        PhotonView batteryPhotonView = PhotonView.Find(batteryViewId);
        if (batteryPhotonView != null)
        {
            // 배터리 오브젝트를 모든 클라이언트에서 제거
            PhotonNetwork.Destroy(batteryPhotonView.gameObject);
        }

    }
}
