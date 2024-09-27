using Photon.Pun;
using UnityEngine;

public class GrabableObject : MonoBehaviourPun, IPunObservable
{
    public bool isHeld;

    void Start()
    {
        isHeld = false;
    }

    // 상태를 네트워크로 동기화
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 로컬 플레이어가 데이터 전송
            stream.SendNext(isHeld);
        }
        else
        {
            // 원격 플레이어가 데이터 수신
            isHeld = (bool)stream.ReceiveNext();
        }
    }
}
