using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    // OptionCanvas를 할당하기 위한 변수
    [SerializeField] private GameObject optionCanvas;

    // 종료 버튼
    public GameObject exitNoti;
    public Button exitYesBtn;
    public Button exitNoBtn;
    public TMP_Text exitText;
    public TMP_Text exitTitle;
    public TMP_Text exitYesText;
    public TMP_Text exitNoText;

    void Start()
    {
        // OptionCanvas가 할당되지 않았다면 경고 메시지 출력
        if (optionCanvas == null)
        {
            Debug.LogWarning("OptionCanvas가 OptionManager에 할당되지 않았습니다.");
        }
        else
        {
            // 시작 시 OptionCanvas를 비활성화
            optionCanvas.SetActive(false);
        }

        // Ensure exitNoti is initially inactive
        if (exitNoti != null)
        {
            exitNoti.SetActive(false);
        }

        // Add listeners once to prevent multiple subscriptions
        if (exitYesBtn != null)
        {
            exitYesBtn.onClick.AddListener(OnExitYesClicked);
        }

        if (exitNoBtn != null)
        {
            exitNoBtn.onClick.AddListener(OnExitNoClicked);
        }
    }

    void Update()
    {
        // ESC 키가 눌렸는지 확인
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (exitNoti != null && exitNoti.activeSelf)
            {
                // exitNoti가 활성화되어 있으면 닫기
                exitNoti.SetActive(false);
            }
            else
            {
                // 그렇지 않으면 OptionCanvas 토글
                ToggleOptionCanvas();
            }
        }
    }

    // OptionCanvas의 활성화 상태를 토글하는 함수
    public void ToggleOptionCanvas()
    {
        if (optionCanvas != null)
        {
            Debug.Log("캔버스 토글");
            bool isActive = optionCanvas.activeSelf;
            optionCanvas.SetActive(!isActive);
        }
    }

    // 게임 종료 버튼
    public void ExitGame()
    {
        Debug.Log("ExitGame called");
        if (exitNoti != null)
        {
            exitNoti.SetActive(true);
            exitTitle.text = "종료";
            exitText.text = "게임을 종료하시겠습니까?";
            exitYesText.text = "네";
            exitNoText.text = "아니요";
        }
    }

    // 방 나가기 버튼
    public void RoomExit()
    {
        Debug.Log("RoomExit called");
        if (exitNoti != null)
        {
            exitNoti.SetActive(true);
            exitTitle.text = "방 나가기";
            exitText.text = "게임 방을 나가겠습니까?";
            exitYesText.text = "네";
            exitNoText.text = "아니요";
        }
    }

    // ExitYes 버튼 클릭 시 호출되는 함수
    private void OnExitYesClicked()
    {
        // Determine whether it's an exit game or room exit based on the title
        if (exitTitle.text == "종료")
        {
            ExitGameEvent();
        }
        else if (exitTitle.text == "방 나가기")
        {
            RoomExitEvent();
        }
    }

    // ExitNo 버튼 클릭 시 호출되는 함수
    private void OnExitNoClicked()
    {
        if (exitNoti != null)
        {
            exitNoti.SetActive(false);
        }
    }

    // 알림 창 닫기 및 게임 종료 함수
    public void ExitGameEvent()
    {
        Debug.Log("ExitGameEvent called");
        // 게임 종료 처리
#if UNITY_EDITOR
        // 유니티 에디터에서 실행 중일 경우 플레이 모드 중지
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 빌드된 게임에서는 실제로 게임 종료
        Application.Quit();
#endif
    }

    public void RoomExitEvent()
    {
        // 방 나가기 버튼을 눌렀을 때 호출
        Debug.Log("RoomExitEvent 함수 호출됨");

        // PhotonNetwork를 사용하여 방 나가기 처리
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom(); // 현재 방에서 나가기
        }
        else
        {
            Debug.LogWarning("현재 방에 있지 않습니다.");
        }
    }
}
