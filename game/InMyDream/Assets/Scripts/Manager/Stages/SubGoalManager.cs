using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;

public class SubGoalManager : MonoBehaviourPunCallbacks
{
    // 도착지점 체크
    private bool localPlayerReached = false;
    private bool otherPlayerReached = false;

    private bool phaseChanged = false;

    public CatController boss;

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
                if (photonView.IsMine)
                {
                    localPlayerReached = true;
                    UserData.instance.stage = 6;
                }
                else
                {
                    otherPlayerReached = true;
                }

                CheckIfBothPlayersReached();
            }
        }
    }

    private void CheckIfBothPlayersReached()
    {
        if (localPlayerReached && otherPlayerReached && !phaseChanged)
        {
            int stage = UserData.instance.stage;

            // 두 플레이어 모두 도착하면 보스 페이즈 전환 메소드 실행
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                // RPC로 모든 플레이어의 스테이지 값을 업데이트
                photonView.RPC("RPC_UpdatePhase", RpcTarget.AllBuffered);
            }

        }
    }

    [PunRPC]
    public void RPC_UpdatePhase()
    {
        // 모든 클라이언트에서 UserData의 stage 값을 업데이트
        UserData.instance.stage = 6;

        phaseChanged = true;

        boss.phase = 3;
    }
}