using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0f";
    //private int userId = UserData.instance.userId;
    //private string userName = UserData.instance.userName;
    public RoomManager roomManager;

    // testing variable
    private int userId = 1;
    private string userName = "KYS";

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = userName;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 2;
        ro.IsOpen = true;
        ro.IsVisible = true;

        //PhotonNetwork.CreateRoom(userName, ro);

        // testing code
        PhotonNetwork.JoinRoom(userName);
    }

    public override void OnJoinedRoom()
    {
        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
        playerProps["character"] = "Player1";
        playerProps["material"] = "00";
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
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
            roomManager.ChangeCharacter();
            roomManager.ChangeMaterial(newMasterClient.ActorNumber,
                "Player1".Equals(newMasterClient.CustomProperties["character"]),
                (string)newMasterClient.CustomProperties["material"]);
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
}
