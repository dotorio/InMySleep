using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 씬 전환을 위해 필요
using TMPro;

public class GameClearManager : MonoBehaviour
{
    public Button clearBtn;
    public TextMeshProUGUI userName;
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        userName.text = UserData.instance.userName + "님";

        // 방장이 클리어 버튼 클릭 시 로비로 이동
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            clearBtn.onClick.AddListener(GameClear);
        }
    }


    void GameClear ()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
