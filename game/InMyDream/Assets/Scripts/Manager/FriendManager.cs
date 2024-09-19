using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Networking;
using static UnityEngine.Rendering.DebugUI;
using TMPro;

public class FriendManager : MonoBehaviourPunCallbacks
{
    //private int myUserId = UserData.instance.userId;
    int myUserId = 1;
    public PhotonManager photonManager;
    public GameObject inviteNoti;
    public List<FriendDto> friendList = new List<FriendDto>();
    public List<UserInfo> requestFriends = new List<UserInfo>();
    public List<UserInfo> receiveFriends = new List<UserInfo>();
    public string roomNameText;
    private string url = "https://j11e107.p.ssafy.io:8000/api/v1/friend/";


    // 친구 목록 확인
    public IEnumerator LoadFriendList()
    {
        UnityWebRequest request = new UnityWebRequest(url+"list?userId="+myUserId, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);

            // 서버로부터 받은 JSON 응답을 파싱
            FriendResponse response = JsonUtility.FromJson<FriendResponse>(request.downloadHandler.text);

            if (response.success)
            {
                Debug.Log("친구 목록 요청 성공");
                friendList = JsonUtility.FromJson<List<FriendDto>>(response.data);
            }
            else
            {
                Debug.Log("친구 목록 요청 실패");
            }
        }
    }

    // 보낸 친구 요청 목록 확인
    IEnumerator LoadRequestFriendList()
    {
        UnityWebRequest request = new UnityWebRequest(url + "request-list?userId=" + myUserId, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);

            // 서버로부터 받은 JSON 응답을 파싱
            FriendResponse response = JsonUtility.FromJson<FriendResponse>(request.downloadHandler.text);

            if (response.success)
            {
                Debug.Log("보낸 친구 요청 목록 성공");
                requestFriends = JsonUtility.FromJson<List<UserInfo>>(response.data);
            }
            else
            {
                Debug.Log("보낸 친구 요청 목록 실패");
            }
        }
    }

    // 받은 친구 요청 목록 확인
    IEnumerator LoadReceiveFriendList()
    {
        UnityWebRequest request = new UnityWebRequest(url + "receive-list?userId=" + myUserId, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);

            // 서버로부터 받은 JSON 응답을 파싱
            FriendResponse response = JsonUtility.FromJson<FriendResponse>(request.downloadHandler.text);

            if (response.success)
            {
                Debug.Log("받츤 친구 요청 목록 성공");
                receiveFriends = JsonUtility.FromJson<List<UserInfo>>(response.data);
            }
            else
            {
                Debug.Log("받은 친구 요청 목록 실패");
            }
        }
    }

    // 친구 추가
    IEnumerator AddFriend(int userId)
    {
        FriendRequestDto friendRequest = new FriendRequestDto(myUserId, userId);
        string jsonData = JsonUtility.ToJson(friendRequest);

        UnityWebRequest request = new UnityWebRequest(url + "request", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);

            // 서버로부터 받은 JSON 응답을 파싱
            FriendResponse response = JsonUtility.FromJson<FriendResponse>(request.downloadHandler.text);

            if (response.success)
            {
                Debug.Log("친구 요청 성공");
            }
            else
            {
                Debug.Log("친구 요청 실패");
            }
        }
    }

    // 친구 수락
    IEnumerator AcceptFriendRequest(int userId)
    {
        FriendRequestDto friendRequest = new FriendRequestDto(myUserId, userId);
        string jsonData = JsonUtility.ToJson(friendRequest);

        UnityWebRequest request = new UnityWebRequest(url + "accept", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);

            // 서버로부터 받은 JSON 응답을 파싱
            FriendResponse response = JsonUtility.FromJson<FriendResponse>(request.downloadHandler.text);

            if (response.success)
            {
                Debug.Log("친구 수락 성공");
            }
            else
            {
                Debug.Log("친구 수락 실패");
            }
        }
    }

    // 친구 거절
    IEnumerator RefuseFriendRequest(int userId)
    {
        FriendRequestDto friendRequest = new FriendRequestDto(myUserId, userId);
        string jsonData = JsonUtility.ToJson(friendRequest);

        UnityWebRequest request = new UnityWebRequest(url + "refuse", "PUT");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);

            // 서버로부터 받은 JSON 응답을 파싱
            FriendResponse response = JsonUtility.FromJson<FriendResponse>(request.downloadHandler.text);

            if (response.success)
            {
                Debug.Log("친구 거절 성공");
            }
            else
            {
                Debug.Log("친구 거절 실패");
            }
        }
    }

    // 친구 삭제
    IEnumerator DeleteFriend(int userId)
    {
        FriendRequestDto friendRequest = new FriendRequestDto(myUserId, userId);
        string jsonData = JsonUtility.ToJson(friendRequest);

        UnityWebRequest request = new UnityWebRequest(url + "delete", "DELETE");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);

            // 서버로부터 받은 JSON 응답을 파싱
            FriendResponse response = JsonUtility.FromJson<FriendResponse>(request.downloadHandler.text);

            if (response.success)
            {
                Debug.Log("친구 삭제 성공");
            }
            else
            {
                Debug.Log("친구 삭제 실패");
            }
        }
    }

    public void SendInvite(string friendName)
    {
        //string userName = UserData.instance.userName;
        string userName = "user1";

        photonManager.SendInvite(friendName, userName);
        
        //Debug.Log($"Invite sent to {friendName} for room: {userName}");
    }

    // 초대 팝업 띄우기
    public void ShowInvitationPopup(string roomName)
    {
        // UI 구현 필요
        //if (isAccepted)
        //{
        //    AcceptInvite(roomName);
        //}
        //Debug.Log(inviteNoti.transform.Find("Popup").transform.Find("Text_Info").name);
        inviteNoti.transform.Find("Popup").transform.Find("Text_Info").GetComponent<TextMeshProUGUI>().text = roomName + "님이\n초대하였습니다.";
        inviteNoti.SetActive(true);
        roomNameText = roomName;
        Debug.Log("초대 왔음!");

    }

    // 친구 초대를 수락하고 해당 방에 참여
    public void AcceptInvite(string roomName)
    {
        // 초대를 수락하면 해당 방으로 이동

        PhotonNetwork.LeaveRoom();
        PhotonNetwork.JoinRoom(roomName);
        Debug.Log($"Accepted invitation and joining room: {roomName}");
    }
}

[System.Serializable]
public class FriendRequestDto
{
    public int requestUserId;
    public int receiveUserId;

    public FriendRequestDto(int requestUserId, int receiveUserId)
    {
        this.requestUserId = requestUserId;
        this.receiveUserId = receiveUserId;
    }
}


[System.Serializable]
public class FriendDto
{
    public int userId;
    public string username;
    public string email;


    public FriendDto(int userId, string username, string email)
    {
        this.userId = userId;
        this.username = username;
        this.email = email;
    }
}

[System.Serializable]
public class FriendResponse
{
    public bool success;
    public string data;
    public string message;
}
