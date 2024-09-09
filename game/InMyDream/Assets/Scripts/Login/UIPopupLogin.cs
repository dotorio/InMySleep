using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIPopupLogin : MonoBehaviour
{
    public Button btnClose;
    public TMP_InputField inputId;
    public TMP_InputField inputPassword;
    public Button btnLogin;
    public Button btnSignup;
    public Button btnForgotPw;
    public Toggle toggleRemember;

    public System.Action<string, string> onClickLogin;

    private void Awake()
    {
        // 아이디 저장 토글
        this.toggleRemember.onValueChanged.AddListener((val) =>
        {
            Debug.LogFormat("isOn: {0}", val);
        });
        // 로그인 버튼 기능 추가
        this.btnLogin.onClick.AddListener(() =>
        {
            // ID, Password 입력 받아 출력
            string id = this.inputId.text;
            string password = this.inputPassword.text;
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password))
            {
                Debug.LogFormat("<color=cyan>ID와 Password 모두 채워야합니다.</color>");
            }
            else
            {
                Debug.LogFormat("ID: {0}", id);
                Debug.LogFormat("Password: {0}", password);
                this.onClickLogin(id, password);
            }
            // ID, Password 값을 튜플로 전송
        });
    }

    public void Init()
    {

    }

    // 로그인 팝업 열기
    public void Open()
    {
        this.gameObject.SetActive(true);
    }
    // 로그인 팝업 닫기
    public void Close()
    {
        this.gameObject.SetActive(false);
    }


}
