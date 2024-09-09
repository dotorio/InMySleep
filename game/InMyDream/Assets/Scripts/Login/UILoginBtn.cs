using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoginBtn : MonoBehaviour
{
    public Button btn;
    public UIPopupLogin uiPopupLogin;
    
    public void Init()
    {
        // 로그인 팝업에서 x 클릭 시 닫기
        this.uiPopupLogin.btnClose.onClick.AddListener(() =>
        {
            this.uiPopupLogin.Close();
        });
        
        // 로그인 버튼 클릭시 로그인 팝업 열기
        this.btn.onClick.AddListener(() =>
        {
            this.uiPopupLogin.Open();
        });
    }
}
