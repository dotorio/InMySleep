using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;

public class GoalManager : MonoBehaviourPunCallbacks
{
    // 도착지점 체크
    private bool localPlayerReached = false;
    private bool otherPlayerReached = false;

    // 1 스테이지 조건 걸기 위한 stage manager
    public StageManager_1_2 stageManager;

    private string[] nextScene = {"", "CutScene2", "CutScene3", "CutScene4", "CutScene4", "4_1_4_2stage", "4_2_end_stage"};
    private string url = "https://j11e107.p.ssafy.io:8000/api/v1/";


    // 골인지점의 Collider에 붙는 스크립트
    private void OnTriggerEnter(Collider other)
    {
        // 태그는 동일하게 Player로 설정
        if (other.CompareTag("Player"))
        {
            // PhotonView를 사용해 각 플레이어의 ID를 확인
            PhotonView photonView = other.GetComponent<PhotonView>();

            if (photonView != null)
            {
                // 로컬 플레이어 체크
                if(photonView.IsMine)
                {
                    localPlayerReached = true;
                }
                else
                {
                    otherPlayerReached = true;
                }

                CheckIfBothPlayersReached();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 태그는 동일하게 Player로 설정
        if (other.CompareTag("Player"))
        {
            // PhotonView를 사용해 각 플레이어의 ID를 확인
            PhotonView photonView = other.GetComponent<PhotonView>();

            if (photonView != null)
            {
                // 로컬 플레이어 체크
                if (photonView.IsMine)
                {
                    localPlayerReached = false;
                }
                else
                {
                    otherPlayerReached = false;
                }
            }
        }
    }

    private void CheckIfBothPlayersReached()
    {
        if (localPlayerReached && otherPlayerReached)
        {
            int stage = UserData.instance.stage;
            
            // 두 플레이어 모두 도착하면 씬 전환 메소드 실행
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if(stage == 1 && stageManager.getBattery() < 2)
                {
                    Debug.Log("Need more battery");
                    return;
                }

                int roomId = UserData.instance.roomId;
                if (stage < 4)
                {
                    StartCoroutine(UpdateClear(roomId, stage + 1));
                }
                else if(stage == 6)
                {
                    StartCoroutine(UpdateClear(roomId, 4));
                    StartCoroutine(GameClear(roomId));
                }

                // RPC로 모든 플레이어의 스테이지 값을 업데이트
                photonView.RPC("RPC_UpdateStage", RpcTarget.AllBuffered, stage + 1);

                PhotonNetwork.LoadLevel(nextScene[stage]);
            }

        }
    }

    [PunRPC]
    public void RPC_UpdateStage(int newStage)
    {
        // 모든 클라이언트에서 UserData의 stage 값을 업데이트
        UserData.instance.stage = newStage;
    }

    // 스테이지 클리어 정보 전달
    IEnumerator UpdateClear(int roomId, int stage)
    {
        // 로그인 정보를 JSON 형식으로 준비
        ClearData clearData = new ClearData(roomId, stage);
        Debug.LogError($"{roomId} {stage}");
        string jsonData = JsonUtility.ToJson(clearData);

        // UnityWebRequest로 HTTP POST 요청을 준비
        UnityWebRequest request = new UnityWebRequest(url+"game-info/clear-stage", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // 서버로부터 응답을 받을 때까지 대기
        yield return request.SendWebRequest();

        // 요청 결과 처리
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            // 응답 실패 시 오류 메시지 표시

            // 에러 발생 시 처리
            Debug.LogError("Error: " + request.downloadHandler.text);
        }
        else
        {
            // 응답 성공 시 처리
            Debug.Log("Response: " + request.downloadHandler.text);

            // 서버로부터 받은 JSON 응답을 파싱
            ClearResponse1 response = JsonUtility.FromJson<ClearResponse1>(request.downloadHandler.text);

            if (response.success)
            {
                Debug.Log($"{stage-1} 스테이지 클리어");
            }
            else
            {
                Debug.Log("업데이트 실패");
            }
        }
    }

    // 게임 클리어 정보 전달
    IEnumerator GameClear(int roomId)
    {
        // 로그인 정보를 JSON 형식으로 준비
        ClearRoom clearRoom = new ClearRoom(roomId);
        string jsonData = JsonUtility.ToJson(clearRoom);

        // UnityWebRequest로 HTTP POST 요청을 준비
        UnityWebRequest request = new UnityWebRequest(url + "room/clear", "PUT");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // 서버로부터 응답을 받을 때까지 대기
        yield return request.SendWebRequest();

        // 요청 결과 처리
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            // 응답 실패 시 오류 메시지 표시

            // 에러 발생 시 처리
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // 응답 성공 시 처리
            Debug.Log("Response: " + request.downloadHandler.text);

            // 서버로부터 받은 JSON 응답을 파싱
            ClearResponse2 response = JsonUtility.FromJson<ClearResponse2>(request.downloadHandler.text);

            if (response.success)
            {
                Debug.Log("게임 스테이지 클리어");
            }
            else
            {
                Debug.Log("업데이트 실패");
            }
        }
    }
}

[System.Serializable]
public class ClearData
{
    public int roomId;
    public int stageNumber;

    public ClearData(int roomId, int stageNumber)
    {
        this.roomId = roomId;
        this.stageNumber = stageNumber;
    }
}

[System.Serializable]
public class ClearRoom
{
    public int roomId;

    public ClearRoom(int roomId)
    {
        this.roomId = roomId;
    }
}

[System.Serializable]
public class UserPerfect
{
    public int userId;
    public string email;
    public string username;
    public int lastStage;
    public string createdAt;
    public string updatedAt;
    public string lastLogin;
    public bool isActive;
}

[System.Serializable]
public class RoomData
{
    public int roomId;
    public UserPerfect hostId;
    public UserPerfect participantId;
    public int characterHost;
    public int characterParticipant;
    public bool isCleared;
    public string startDate;
    public string clearDate;
    public string createdAt;
}

[System.Serializable]
public class ClearResponse1
{
    public bool success;
    public string data;
    public string message;
}

[System.Serializable]
public class ClearResponse2
{
    public bool success;
    public RoomData data;
    public string message;
}