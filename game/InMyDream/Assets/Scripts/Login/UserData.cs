using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유저 정보를 저장할 클래스
public class UserData : MonoBehaviour
{
    public static UserData instance;

    public int userId;
    public string userName;
    public string email;
    public int lastStage;
    public int stage;
    public int roomId;
    public string bear;
    public string rabbit;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
