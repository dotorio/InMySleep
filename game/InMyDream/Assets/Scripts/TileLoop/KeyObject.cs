using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // 오브젝트가 파괴될 때 호출되는 함수
        if (specialObject.isKey == keyNum)
        {
            stageManager.isKey = true;
        }
        else
        {
            stageManager.isKey = false;
        }
    }
}
