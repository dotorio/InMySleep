using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomButtonManager : MonoBehaviourPunCallbacks
{
    public PhotonManager photonManager;

    public void Select1()
    {
        photonManager.SelectCharacter("Player1", "01");
    }
    public void Select2()
    {
        photonManager.SelectCharacter("Player2", "04");
    }
    // 게임 시작 버튼
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 인원 수가 차있는지 확인
            if (PhotonNetwork.PlayerList.Length != 2)
            {
                Debug.Log("인원이 부족합니다.");
                return;
            }

            // 플레이어가 선택한 캐릭터가 중복인지 확인
            string user1 = (string)PhotonNetwork.PlayerList[0].CustomProperties["character"];
            string user2 = (string)PhotonNetwork.PlayerList[1].CustomProperties["character"];

            if (user1 == user2)
            {
                Debug.Log("캐릭터가 중복됩니다.");
                return;
            }

            // 씬 로드
            PhotonNetwork.LoadLevel("kysLobbyTest");
        }
    }
}
