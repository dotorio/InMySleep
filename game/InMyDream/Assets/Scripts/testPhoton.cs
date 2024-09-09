using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class testPhoton : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0f";
    private string userId = "KHJ";
    private string selectedCharacter;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = userId;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;
        ro.IsOpen = true;
        ro.IsVisible = true;

        PhotonNetwork.CreateRoom("My Room", ro);
    }

    public override void OnJoinedRoom()
    {
        // 선택된 캐릭터 정보를 확인합니다.
        selectedCharacter = (string)PhotonNetwork.LocalPlayer.CustomProperties["character"];
        if (string.IsNullOrEmpty(selectedCharacter))
        {
            Debug.LogError("캐릭터가 선택되지 않았습니다.");
            return;
        }
    }

    public void OnSelectCharacter(string characterName)
    {
        // 중복 캐릭터 선택 확인
        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
        playerProps["character"] = characterName;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);

        // 룸의 캐릭터 선택 정보 업데이트
        ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable();
        roomProps[PhotonNetwork.LocalPlayer.ActorNumber.ToString()] = characterName;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 모든 플레이어가 캐릭터를 선택했는지 확인
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (!player.CustomProperties.ContainsKey("character"))
                {
                    Debug.Log("모든 플레이어가 캐릭터를 선택하지 않았습니다.");
                    return;
                }
            }

            // 씬 로드
            PhotonNetwork.LoadLevel("kysTest");
        }
    }
}
