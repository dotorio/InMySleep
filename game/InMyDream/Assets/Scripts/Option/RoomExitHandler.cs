using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class RoomExitHandler : MonoBehaviourPunCallbacks
{
    private bool isLeaving = false; // 의도적인 방 나가기를 추적하는 플래그

    /// <summary>
    /// 다른 플레이어에게 방 나가기 명령을 전달하는 RPC 메소드
    /// </summary>
    [PunRPC]
    public void RPC_LeaveRoom()
    {
        // 이미 나가는 중이지 않고 방에 있는 경우에만 실행
        if (PhotonNetwork.InRoom && !isLeaving)
        {
            Debug.Log("RPC_LeaveRoom 호출됨. 방을 떠납니다.");
            isLeaving = true;
            PhotonNetwork.LeaveRoom();
        }
    }

    /// <summary>
    /// 다른 플레이어들에게 방 나가기 명령을 전달하는 함수
    /// </summary>
    public void NotifyRoomExit()
    {
        // 방에 다른 플레이어가 있는 경우에만 RPC 전송
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            Debug.Log("다른 플레이어에게 방 나가기를 알립니다.");
            photonView.RPC("RPC_LeaveRoom", RpcTarget.Others);
        }
    }

    /// <summary>
    /// 방 나가기 이벤트를 시작하는 함수
    /// </summary>
    public void RoomExitEvent()
    {
        Debug.Log("RoomExitEvent 호출됨.");

        if (PhotonNetwork.InRoom && !isLeaving)
        {
            isLeaving = true; // 의도적인 나가기 설정

            // 다른 플레이어들에게 방 나가기 알림
            NotifyRoomExit();

            // 자신은 방을 나감
            PhotonNetwork.LeaveRoom();

            // 플레이어 데이터 초기화
            if (UserData.instance != null)
            {
                UserData.instance.stage = 1;
            }
        }
        else
        {
            Debug.LogWarning("방을 나갈 수 없습니다: 현재 방에 없거나 이미 나가고 있는 중입니다.");
        }
    }

    /// <summary>
    /// 로컬 플레이어가 방을 떠났을 때 호출되는 콜백
    /// </summary>
    public override void OnLeftRoom()
    {
        Debug.Log("방에서 성공적으로 나갔습니다.");

        if (isLeaving)
        {
            // 로비 씬으로 전환
            SceneManager.LoadScene("LobbyScene");
            isLeaving = false; // 플래그 초기화
        }
        // 의도적인 나가기가 아닐 경우, 추가 동작이 필요 없다면 아무것도 하지 않음
    }

    /// <summary>
    /// 다른 플레이어가 방을 떠났을 때 호출되는 콜백
    /// </summary>
    /// <param name="otherPlayer">방을 떠난 플레이어</param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName}님이 방에서 나갔습니다.");

        // 다른 플레이어가 나갔을 때, 아직 나가지 않았다면 방 나가기 프로세스를 시작
        if (!isLeaving)
        {
            Debug.Log("다른 플레이어가 방을 떠났습니다. 방 나가기 프로세스를 시작합니다.");
            RoomExitEvent();
        }
    }

    /// <summary>
    /// 객체가 파괴될 때 플래그를 초기화하여 안전성 확보
    /// </summary>
    private void OnDestroy()
    {
        isLeaving = false;
    }
}
