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
    private int userId;
    private string userName;
    private ChatClient chatClient;

    public RoomManager roomManager;
    public FriendManager friendManager;
    
    public StartController startController;
    public RoomExitController roomExitController;

    public GameObject buttonManager;
    private FriendListToggle friendListToggle;  // FriendListToggle의 참조


    //// testing variable
    //private int userId = 42;
    //private string userName = "ttest";

    private void Awake()
    {
        if(UserData.instance != null)
        {
            userId = UserData.instance.userId;
            userName = UserData.instance.userName;
        }

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = userName;
        //PhotonNetwork.LocalPlayer.IsMasterClient = true;
        
        // 이미 방에 있을 경우 ConnectUsingSettings 호출하지 않음
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        // 이미 방에 있을 경우 방 생성 또는 방 참여 로직 스킵
        if (!PhotonNetwork.InRoom)
        {
            ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
            playerProps["roomName"] = userName;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
        }

        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
            "1.0",
            new Photon.Chat.AuthenticationValues(userName));

        // buttonManager에서 FriendListToggle 컴포넌트 가져오기
        friendListToggle = buttonManager.GetComponent<FriendListToggle>();
    }


    void Update()
    {
        if(chatClient != null)
        {
            chatClient.Service();
        }
        if(UserData.instance.isGaming)
        {
            UserData.instance.isGaming = false;

            ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
            playerProps["character"] = PhotonNetwork.LocalPlayer.CustomProperties["character"];
            playerProps["material"] = PhotonNetwork.LocalPlayer.CustomProperties["material"];
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);

            startController.buttonUpdate();
            roomExitController.roomExitButtonUpdate();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        // 이미 방에 있는 경우 방을 다시 생성하거나 참여하지 않음
        if (!PhotonNetwork.InRoom)
        {
            string roomName = (string)PhotonNetwork.LocalPlayer.CustomProperties["roomName"];

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
    }

    public override void OnJoinedRoom()
    {
        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
        playerProps["character"] = "Player1";
        playerProps["material"] = UserData.instance.bear != null ?
            UserData.instance.bear : "00";
        playerProps["userId"] = userId;
        playerProps["isDowned"] = false;
        playerProps["roomName"] = userName;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);

        startController.buttonUpdate();
        roomExitController.roomExitButtonUpdate();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 누군가 방에 들어오면 방장과 참가자 모두 버튼을 업데이트
        roomExitController.roomExitButtonUpdate();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // 누군가 방에서 나가면 방장과 참가자 모두 버튼을 업데이트
        roomExitController.roomExitButtonUpdate();
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Failed to join room: {message}");

        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
        playerProps["roomName"] = userName;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
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

    // 개인 메세지(친구 초대 또는 친구 요청)가 왔을 때 실행되는 메소드
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        string messageText = message.ToString();

        if (messageText.StartsWith("Invitation: "))
        {
            // 초대 메시지 처리
            string roomName = messageText.Replace("Invitation: ", "");
            Debug.Log($"Received invitation to room: {roomName} from {sender}");

            if (sender != userName)
            {
                // 초대 수락 여부를 묻는 UI 호출
                friendManager.ShowInvitationPopup(roomName);
            }
        }
        else if (messageText.StartsWith("FriendRequest: "))
        {
            // 친구 요청 메시지 처리
            Debug.Log($"Received friend request from {sender}");

            // 알림 아이콘 표시
            friendListToggle.ShowNotiListLight();
        }
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

    // 친구 요청
    public void SendFriendRequest(string username)
    {
        if (chatClient != null)
        {
            chatClient.SendPrivateMessage(username, $"FriendRequest: {username}");
            Debug.Log($"Friend Request sent to {username}");
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
