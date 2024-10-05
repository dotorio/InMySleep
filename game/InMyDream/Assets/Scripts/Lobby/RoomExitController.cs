using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class RoomExitController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject exitBtn;
    public GameObject roomExitBtn;


    public void roomExitButtonUpdate()
    {
        // 방에 2명 이상 있으면 (본인 포함) exitBtn 활성화
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            exitBtn.SetActive(false);
            roomExitBtn.SetActive(true);
        }
        else
        {
            exitBtn.SetActive(true);
            roomExitBtn.SetActive(false);
        }
    }
}
