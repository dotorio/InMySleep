using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendListToggle : MonoBehaviour
{
    public RectTransform friendListPanel;  // 친구 목록 패널의 RectTransfor
    public RectTransform friendSearchListPanel;  // 친구 검색 목록 패널의 RectTransfor
    public Button toggleButton;
    public Button searchToggleButton;
    public Button searchCloseBtn;

    public GameObject friendList;
    public GameObject friendSearchList;

    public float slideSpeed = 500f;        // 슬라이드 속도
    private bool isPanelVisible = false;   // 초대 목록 패널이 보이는지 여부
    private bool isSearchPanelVisible = false;   //  검색 패널이 보이는지 여부


    private Vector2 hiddenPosition;        // 패널이 숨겨졌을 때 위치
    private Vector2 visiblePosition;       // 패널이 보일 때 위치

    void Start()
    {
        // 버튼에 클릭 이벤트 추가
        toggleButton.onClick.AddListener(ToggleFriendList);
        searchToggleButton.onClick.AddListener(SearchToggleFriendList);
        searchCloseBtn.onClick.AddListener(SearchToggleFriendList);

        // 패널의 시작 위치 설정 (화면 오른쪽 밖)
        hiddenPosition = new Vector2(Screen.width / 2 + 650 , friendListPanel.anchoredPosition.y);
        visiblePosition = new Vector2(Screen.width / 2  - 50, friendListPanel.anchoredPosition.y);

        // 처음에 패널을 화면 밖에 숨김
        friendListPanel.anchoredPosition = hiddenPosition;
        friendSearchListPanel.anchoredPosition = hiddenPosition;
    }

    public void ToggleFriendList()
    {
        // 패널이 보이는 상태인지에 따라 이동 방향 설정
        if (isPanelVisible)
        {
            StartCoroutine(SlidePanel(friendListPanel, visiblePosition, hiddenPosition));
            friendList.SetActive(false);
        }
        else
        {
            StartCoroutine(SlidePanel(friendListPanel, hiddenPosition, visiblePosition));
        }

        // 상태 토글
        isPanelVisible = !isPanelVisible;
    }

    public void SearchToggleFriendList()
    {
        // 패널이 보이는 상태인지에 따라 이동 방향 설정
        if (isSearchPanelVisible)
        {
            StartCoroutine(SlidePanel(friendSearchListPanel, visiblePosition, hiddenPosition));
            friendSearchList.SetActive(false);
        }
        else
        {
            StartCoroutine(SlidePanel(friendSearchListPanel, hiddenPosition, visiblePosition));
            friendSearchList.SetActive(true);
        }

        // 상태 토글
        isSearchPanelVisible = !isSearchPanelVisible;
    }

    // 패널을 부드럽게 슬라이딩하는 코루틴
    IEnumerator SlidePanel(RectTransform panel, Vector2 start, Vector2 end)
    {
        float elapsedTime = 0;
        float duration = Mathf.Abs(Vector2.Distance(start, end)) / slideSpeed;

        while (elapsedTime < duration)
        {
            panel.anchoredPosition = Vector2.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = end;
    }
}
