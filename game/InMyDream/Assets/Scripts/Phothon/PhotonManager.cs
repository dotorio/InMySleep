using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0f";
    private int userId = UserData.instance.userId;
    private string userName = UserData.instance.userName;

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

        PhotonNetwork.CreateRoom(userName, ro);
    }

    //public override void OnJoinRandomFailed(short returnCode, string message)
    //{
        
    //}

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LocalPlayer.CustomProperties["character"] = "Player1";
        PhotonNetwork.LocalPlayer.CustomProperties["material"] = "00";
    }

    public void OnSelectCharacter(string characterName, string characterMaterial)
    {
        // 중복 캐릭터 선택 확인
        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
        playerProps["character"] = characterName;
        playerProps["material"] = characterMaterial;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);

        // 룸의 유저 id 저장
        ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable();
        roomProps[PhotonNetwork.LocalPlayer.ActorNumber.ToString()] = userId;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 인원 수가 차있는지 확인
            if(PhotonNetwork.PlayerList.Length != 2)
            {
                Debug.Log("인원이 부족합니다.");
                return;
            }

            // 모든 플레이어가 캐릭터를 선택했는지 확인
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (!player.CustomProperties.ContainsKey("character"))
                {
                    Debug.Log("모든 플레이어가 캐릭터를 선택하지 않았습니다.");
                    return;
                }
            }

            // 플레이어가 선택한 캐릭터가 중복인지 확인
            object user1 = PhotonNetwork.PlayerList[0].CustomProperties["character"];
            object user2 = PhotonNetwork.PlayerList[1].CustomProperties["character"];

            if(user1.Equals(user2))
            {
                Debug.Log("캐릭터가 중복됩니다.");
                return;
            }

            // 씬 로드
            PhotonNetwork.LoadLevel("kysTest");
        }
    }
}
