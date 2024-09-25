using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Chat;
using ExitGames.Client.Photon;

public class PhotonManager : MonoBehaviourPunCallbacks, IChatClientListener
{
    private readonly string version = "1.0f";
    private int userId = UserData.instance.userId;
    private string userName = UserData.instance.userName;
    private ChatClient chatClient;

    public RoomManager roomManager;
    public FriendManager friendManager;

    
    // testing variable
    //private int userId = 41;
    //private string userName = "user2";

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = userName;
        //PhotonNetwork.LocalPlayer.IsMasterClient = true;

        PhotonNetwork.ConnectUsingSettings();

        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
        playerProps["roomName"] = userName;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);

        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
            "1.0",
            new Photon.Chat.AuthenticationValues(userName));


    }


    void Update()
    {
        if(chatClient != null)
        {
            chatClient.Service();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        string roomName = (string) PhotonNetwork.LocalPlayer.CustomProperties["roomName"];

        if (roomName == userName)
        {
            RoomOptions ro = new RoomOptions();
            ro.MaxPlayers = 2;
            ro.IsOpen = true;
            ro.IsVisible = true;
            ro.EmptyRoomTtl = 0; // 방이 비었을 경우 바로 방 삭제

            PhotonNetwork.CreateRoom(roomName, ro);
        }
        else
        {
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    public override void OnJoinedRoom()
    {
        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
        playerProps["character"] = "Player1";
        playerProps["material"] = "00";
        playerProps["userId"] = userId;
        playerProps["isDowned"] = false;
        playerProps["roomName"] = userName;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Failed to join room: {message}");

        PhotonNetwork.JoinLobby();
    }

    // 캐릭터 및 스킨 변경 감지
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("character"))
        {
            if(targetPlayer == PhotonNetwork.LocalPlayer)
            {
                roomManager.ChangeCharacter();
            }
        }

        if(changedProps.ContainsKey("material"))
        {
            roomManager.ChangeMaterial(targetPlayer.ActorNumber,
                "Player1".Equals(targetPlayer.CustomProperties["character"]),
                (string)targetPlayer.CustomProperties["material"]);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // 자신이 방장이 됐을 때
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    // 캐릭터 및 스킨 변경
    public void SelectCharacter(string characterName, string characterMaterial)
    {
        // 캐릭터 및 스킨 선택
        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
        playerProps["character"] = characterName;
        playerProps["material"] = characterMaterial;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);

        // 룸의 유저 id 저장
        ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable();
        roomProps[PhotonNetwork.LocalPlayer.ActorNumber.ToString()] = userId;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
    }

    // 친구 초대용 photon chat 인터페이스 구현 부분
    void IChatClientListener.OnConnected()
    {
        Debug.Log("Connected to Photon Chat");
        //Debug.Log(chatClient);
    }

    public void OnDisconnected()
    {
        Debug.Log("Disconnected from Photon Chat");
    }

    // 개인 메세지(친구 초대)가 왔을 때 실행되는 메소드
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        // 메시지를 바로 초대로 처리 (다른 메시지 유형이 없으므로 바로 처리 가능)
        string roomName = message.ToString().Replace("Invitation: ", "");
        Debug.Log($"Received invitation to room: {roomName} from {sender}");

        if (sender != userName)
        {
            // 초대 수락 여부를 묻는 UI 호출 (예시: 팝업)
            friendManager.ShowInvitationPopup(roomName);
        }

        // 초대를 받은 사람이 자신일 경우에만 팝업을 띄움
        //if (sender != UserData.instance.userName) // 초대 메시지를 보낸 사람이 본인이 아닌 경우
        //{
        //    ShowInvitationPopup(roomName); // 팝업 띄우기
        //}

    }

    // 초대 보내는 함수
    public void SendInvite(string friendName, string roomName)
    {
        if (chatClient != null)
        {
            // 친구에게 방 초대 메시지 전송 (수락 필요)
            chatClient.SendPrivateMessage(friendName, $"Invitation: {roomName}");
            Debug.Log($"Invite sent to {friendName} for room: {roomName}");
        }
    }

    // 쓰지 않지만 구현해야하는 메소드
    public void DebugReturn(DebugLevel level, string message) { }

    public void OnChatStateChange(ChatState state) { }

    public void OnGetMessages(string channelName, string[] senders, object[] messages) { }

    public void OnSubscribed(string[] channels, bool[] results) { }

    public void OnUnsubscribed(string[] channels) { }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message) { }

    public void OnUserSubscribed(string channel, string user) { }

    public void OnUserUnsubscribed(string channel, string user) { }
}
