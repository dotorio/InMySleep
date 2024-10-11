using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KeyObject : MonoBehaviour
{
    private StageManager_3 stageManager;
    public SpecialObject specialObject;
    public int keyNum = 1;

    void Start()
    {
        stageManager = FindObjectOfType<StageManager_3>();

        if (stageManager != null)
        {
            Debug.Log("매니저 불러옴!");
        }
        else
        {
            Debug.Log("매니저 못 불러옴!");
        }


    }
    void OnDestroy()
    {
        Debug.Log("당첨:" + specialObject.isKey + ", 현재:" + keyNum);
        // 오브젝트가 파괴될 때 호출되는 함수
        if (specialObject.isKey == keyNum)
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                PhotonNetwork.Instantiate("Star", gameObject.transform.position, Quaternion.identity);
            }
        }
    }
}
