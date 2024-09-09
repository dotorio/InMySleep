using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonSceneMain : MonoBehaviour
{
    public UILoginBtn LoginBtn;
    void Start()
    {
        this.LoginBtn.uiPopupLogin.onClickLogin = (id, password) =>
        {
            // 서버 전송 or 저장
            Debug.LogFormat("<color=yellow>id: {0}, password: {1}</color>", id, password);
        };
        this.LoginBtn.Init();
    }


   
}
