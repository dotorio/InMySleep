using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RoomButtonManager : MonoBehaviourPunCallbacks
{
    public PhotonManager photonManager;
    public FriendManager friendManager;
    public GameObject errorNoti;
    public Button errorNotiBtn;
    public TMP_Text errorText;
    public GameObject friendList;
    

    private string url = "https://j11e107.p.ssafy.io:8000/api/v1/";

    public void Select1()
    {
        string skin = UserData.instance.bear != null ? 
            UserData.instance.bear : "00";
        photonManager.SelectCharacter("Player1", skin);
    }
    public void Select2()
    {
        string skin = UserData.instance.rabbit != null ?
            UserData.instance.rabbit : "00";
        photonManager.SelectCharacter("Player2", skin);
    }

    public void GetFriendList ()
    {
        friendList.SetActive(true);
        //if (friendManager != null)
        //{
        //    StartCoroutine(DisplayFriendList());
        //}
        //else
        //{
        //    Debug.LogError("FriendManager가 할당되지 않았습니다.");
        //}

    }

    // 게임 시작 버튼
    public void StartGame()
    {
        
        if (PhotonNetwork.IsMasterClient)
        {
            // 인원 수가 차있는지 확인
            if (PhotonNetwork.CurrentRoom.PlayerCount != 2)
            {
                Debug.Log("인원이 부족합니다.");
                errorNoti.SetActive(true);
                errorNotiBtn.onClick.AddListener(ErrorNotiClose);
                errorText.text = "인원이 부족합니다.";
                return;
            }


            // 플레이어가 선택한 캐릭터가 중복인지 확인
            List<string> users = new List<string>();

            StartRoom data = new StartRoom();

            foreach(var player in PhotonNetwork.CurrentRoom.Players)
            {
                string character = (string)player.Value.CustomProperties["character"];
                users.Add(character);

                if(player.Value.IsMasterClient)
                {
                    data.hostId = (int)player.Value.CustomProperties["userId"];

                    if(character == "Player1")
                    {
                        data.characterHost = 1;
                    }
                    else
                    {
                        data.characterHost = 2;
                    }
                }
                else
                {
                    data.participantId = (int)player.Value.CustomProperties["userId"];

                    if (character == "Player1")
                    {
                        data.characterParticipant = 1;
                    }
                    else
                    {
                        data.characterParticipant = 2;
                    }
                }
            }

            if (users[0] == users[1])
            {
                Debug.Log("캐릭터가 중복됩니다.");
                errorNoti.SetActive(true);
                errorNotiBtn.onClick.AddListener(ErrorNotiClose);
                errorText.text = "캐릭터가 중복됩니다.";
                return;
            }

            // 백엔드 db 방 생성
            StartCoroutine(StartRoomDB(data));

            // 이스터 에그 정보 생성
            int randomStage = Random.Range(1, 5);

            // Room Custom Properties에 이스터 에그 정보 저장
            ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
            customProperties["EasterEggStage"] = randomStage;
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);

            // 씬 로드
            PhotonNetwork.LoadLevel("CutScene1");
        }
    }

    void ErrorNotiClose()
    {
        errorNoti.SetActive(false);
    }

    IEnumerator DisplayFriendList()
    {
        // FriendManager의 코루틴을 호출하여 친구 목록 로드
        yield return StartCoroutine(friendManager.LoadFriendList());

        // 친구 목록 로드가 완료된 후, 친구 목록 출력
        List<FriendDto> friends = friendManager.friendList;

        if (friends != null && friends.Count > 0)
        {
            Debug.Log("친구 목록 출력:");

            foreach (FriendDto friend in friends)
            {
                Debug.Log($"ID: {friend.userId}, Username: {friend.username}, Email: {friend.email}");
            }
        }
        else
        {
            Debug.Log("친구 목록이 비어 있습니다.");
        }
    }
    IEnumerator StartRoomDB(StartRoom data)
    {
        // 로그인 정보를 JSON 형식으로 준비
        string jsonData = JsonUtility.ToJson(data);

        // UnityWebRequest로 HTTP POST 요청을 준비
        UnityWebRequest request = new UnityWebRequest(url + "room/create", "POST");
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
                Debug.Log("방 생성 성공");
                UserData.instance.roomId = response.data.roomId;
            }
            else
            {
                Debug.Log("방 생성 실패");
            }
        }
    }
}

[System.Serializable]
public class StartRoom
{
    public int hostId;
    public int participantId;
    public int characterHost;
    public int characterParticipant;
}
