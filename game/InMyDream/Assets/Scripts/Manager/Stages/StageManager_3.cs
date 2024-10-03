using System.Collections;
using System.Collections.Generic;
using EpicToonFX;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class StageManager_3 : MonoBehaviourPunCallbacks, StageManager
{
    // 스폰 위치
    public GameObject Master;
    public GameObject Client;
    public GameObject gameoverScreen;
    public List<Transform> spawnPoints = new List<Transform>();

    // 동적인 스폰 포인트 배정을 위한 변수
    public InfiniteTileManager tileManager;
    public GameObject robotVaccum;
    public float minDistance = 1f;
    bool isGameover = false;    
    public bool isKey = false;


    void Start()
    {
        string characterName = (string)PhotonNetwork.LocalPlayer.CustomProperties["character"];
        //Debug.Log((bool)PhotonNetwork.LocalPlayer.CustomProperties["isDowned"]);
        Debug.Log(UserData.instance.stage);
        if (string.IsNullOrEmpty(characterName))
        {
            Debug.LogError("캐릭터가 선택되지 않았습니다.");
            return;
        }

        Debug.Log($"선택된 캐릭터: {characterName}");

        int stage = UserData.instance.stage;
        Transform point;

        if(PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            point = Master.transform;
        }
        else
        {
            point = Client.transform;
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

            // 스킨 설정
            Player localPlayer = PhotonNetwork.LocalPlayer;

            photonView.RPC("RPC_PlayerSetting", 
                RpcTarget.AllBuffered,
                localPlayer.ActorNumber,
                "Player1".Equals(localPlayer.CustomProperties["character"]),
                (string)localPlayer.CustomProperties["material"]);
        }
    }

    void Update()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            CheckGameOver();
        }
    }

    public int RandomInt()
    {
        int randomIndex = Random.Range(0, 4);
        return randomIndex;
    }
    // 모든 유저가 쓰러져 있는 상태면 게임 오버되고 3스테이지 다시 시작
    private void CheckGameOver()
    {
        bool allDowned = true;

        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            if(!player.Value.CustomProperties.ContainsKey("isDowned") || !(bool)player.Value.CustomProperties["isDowned"])
            {
                allDowned = false;
                break;
            }
        }
        
        if(allDowned && !isGameover)
        {
            isGameover = true;
            //ShowGameOverMessage();
            Debug.Log("재시작!");
            ResetScene();
        }
    }



    private void ShowGameOverMessage()
    {
        // 스테이지 실패 UI 보여줄 필요 있음
        gameoverScreen.SetActive(true);

        //foreach (var player in PhotonNetwork.CurrentRoom.Players)
        //{
        //    if (!player.Value.CustomProperties.ContainsKey("isDowned") || (bool)player.Value.CustomProperties["isDowned"])
        //    {
        //        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
        //        playerProps["isDowned"] = false;
        //        player.Value.SetCustomProperties(playerProps);
        //    }
        //}

        // 1초 대기
        StartCoroutine(WaitForSeconds(1f));
        Debug.Log("모든 플레이어가 쓰러졌습니다. 게임 실패!");
    }

    IEnumerator WaitForSeconds(float time)
    {
        // time 대기 코드
        yield return new WaitForSeconds(time);
    }

    // 3스테이지 씬 다시 시작
    private void ResetScene()
    {
        // 3 스테이지 씬 이름에 따라 변경 필요
        PhotonNetwork.LoadLevel("GameOver");
    }

    // 동적으로 스폰 포인트 추가
    public void AddSpawnPoint(Transform newSpawnPoint)
    {
        spawnPoints.Add(newSpawnPoint);
    }

    // 스폰 포인트 반환
    public Transform getSpawnPoint()
    {
        Transform closestSpawnPoint = null;
        float closestDistance = Mathf.Infinity;
        Vector3 robotPosition = robotVaccum.transform.position;

        // 스폰 불가능한 스폰 포인트를 리스트에서 제거
        // 로봇 청소기에 너무 가깝거나 로봇 청소기가 지나간 위치
        spawnPoints.RemoveAll(spawnPoint => spawnPoint.position.z <= robotPosition.z + minDistance);

        foreach(Transform spawnPoint in spawnPoints)
        {
            // z 축 방향 거리 계산
            float distance = spawnPoint.position.z - robotPosition.z;

            // 일정 거리 이상인 경우만 체크
            if(distance > minDistance && distance <closestDistance)
            {
                closestDistance = distance;
                closestSpawnPoint = spawnPoint;
            }
        }

        return closestSpawnPoint;
    }

    // 스킨 변경 메소드
    [PunRPC]
    public void RPC_PlayerSetting(int targetActorNumber, bool isBear, string material)
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
                    tileManager.addPlayer(character);

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
