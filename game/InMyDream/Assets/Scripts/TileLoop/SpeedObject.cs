using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpeedObject : MonoBehaviour
{
    // Start is called before the first frame update
    void OnDestroy()
    {
        // 오브젝트가 파괴될 때 호출되는 함수
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PhotonNetwork.Instantiate("SpeedLowItem", gameObject.transform.position, Quaternion.identity);
        }
        
    }
}
