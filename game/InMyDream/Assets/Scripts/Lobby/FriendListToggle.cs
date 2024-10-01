using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendListToggle : MonoBehaviour
{
    public RectTransform friendListPanel;  // 친구 목록 패널의 RectTransfor
    public RectTransform friendSearchListPanel;  // 친구 검색 목록 패널의 RectTransfor
    public RectTransform NotiListPanel;  // 친구 검색 목록 패널의 RectTransfor
    public Button toggleButton;
    public Button searchToggleButton;
    public Button notiToggleButton;
    public Button searchCloseBtn;
    public Button notiCloseBtn;
    

    public GameObject friendList;
    public GameObject friendSearchList;
    public GameObject notiList;

    public float slideSpeed = 500f;        // 슬라이드 속도
    private bool isPanelVisible = false;   // 초대 목록 패널이 보이는지 여부
    private bool isSearchPanelVisible = false;   //  검색 패널이 보이는지 여부
    private bool isNotiPanelVisible = false;   //  우편함 패널이 보이는지 여부


    private Vector2 hiddenPosition;        // 패널이 숨겨졌을 때 위치
    private Vector2 visiblePosition;       // 패널이 보일 때 위치

    void Start()
    {
        // 버튼에 클릭 이벤트 추가
        toggleButton.onClick.AddListener(() => TogglePanel(friendListPanel, ref isPanelVisible, friendList));
        searchToggleButton.onClick.AddListener(() => TogglePanel(friendSearchListPanel, ref isSearchPanelVisible, friendSearchList));
        notiToggleButton.onClick.AddListener(() => TogglePanel(NotiListPanel, ref isNotiPanelVisible, notiList));
        searchCloseBtn.onClick.AddListener(() => TogglePanel(friendSearchListPanel, ref isSearchPanelVisible, friendSearchList));
        notiCloseBtn.onClick.AddListener(() => TogglePanel(NotiListPanel, ref isNotiPanelVisible, notiList));

        // Canvas에 맞춘 UI 패널의 크기 기준으로 위치 설정
        float canvasWidth = friendListPanel.parent.GetComponent<RectTransform>().rect.width;

        // 패널의 시작 위치 설정 (화면 오른쪽 밖)
        hiddenPosition = new Vector2(canvasWidth / 2 + 650, friendListPanel.anchoredPosition.y);
        visiblePosition = new Vector2(canvasWidth / 2 - 50, friendListPanel.anchoredPosition.y);

        // 처음에 패널을 화면 밖에 숨김
        friendListPanel.anchoredPosition = hiddenPosition;
        friendSearchListPanel.anchoredPosition = hiddenPosition;
        NotiListPanel.anchoredPosition = hiddenPosition;

    }

    private void TogglePanel(RectTransform panel, ref bool isVisible, GameObject panelGameObject)
    {
        // 열려 있는 다른 패널을 닫기
        if (panel != friendListPanel && isPanelVisible)
        {
            StartCoroutine(SlidePanel(friendListPanel, visiblePosition, hiddenPosition, () => friendList.SetActive(false)));
            isPanelVisible = false;
        }
        if (panel != friendSearchListPanel && isSearchPanelVisible)
        {
            StartCoroutine(SlidePanel(friendSearchListPanel, visiblePosition, hiddenPosition, () => friendSearchList.SetActive(false)));
            isSearchPanelVisible = false;
        }
        if (panel != NotiListPanel && isNotiPanelVisible)
        {
            StartCoroutine(SlidePanel(NotiListPanel, visiblePosition, hiddenPosition, () => notiList.SetActive(false)));
            isNotiPanelVisible = false;
        }

        // 패널 열기/닫기
        if (isVisible)
        {
            StartCoroutine(SlidePanel(panel, visiblePosition, hiddenPosition, () => panelGameObject.SetActive(false)));
        }
        else
        {
            panel.SetSiblingIndex(panel.parent.childCount - 1);
            panelGameObject.SetActive(true);
            StartCoroutine(SlidePanel(panel, hiddenPosition, visiblePosition));
        }

        isVisible = !isVisible;
    }

    IEnumerator SlidePanel(RectTransform panel, Vector2 start, Vector2 end, System.Action onComplete = null)
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

        if (onComplete != null)
        {
            onComplete();
        }
    }
}
