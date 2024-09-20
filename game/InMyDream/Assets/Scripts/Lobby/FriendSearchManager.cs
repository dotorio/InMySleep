using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine;
using System.Text.RegularExpressions;

public class FriendSearchManager : MonoBehaviour
{
    public TextMeshProUGUI searchInputField;  // 친구 이름을 입력받을 Input Field
    public Button searchButton;                // 검색 버튼
    public Transform contentPanel;             // Scroll View의 Content 부분
    public GameObject friendItemPrefab;        // 친구 항목 프리팹

    private string apiUrl = "https://j11e107.p.ssafy.io:8000/api/v1/user/search";  // 서버 API URL
    public int page = 0;
    public int size = 100;
    public int userId = 22;

    void Start()
    {
        // 검색 버튼에 OnSearchButtonClick 이벤트 연결
        searchButton.onClick.AddListener(OnSearchButtonClick);
    }

    public void OnSearchButtonClick()
    {
        string searchText = searchInputField.text;  // Input Field에서 검색 텍스트 가져오기
        if (!string.IsNullOrEmpty(searchText))
        {
            StartCoroutine(GetFriendList(searchText));
        }
    }

    IEnumerator GetFriendList(string friendName)
    {
        string cleanedUsername = Regex.Replace(friendName, @"\u200B", "").Trim();
        string url = $"{apiUrl}?username={cleanedUsername}&page={page}&size={size}&userId={userId}";
        Debug.Log(url);
        UnityWebRequest request = UnityWebRequest.Get(url);  // GET 요청 생성
        yield return request.SendWebRequest();  // 서버 응답 대기

        if (request.result == UnityWebRequest.Result.Success)
        {
            // JSON 파싱
            FriendSearchResponse response = JsonUtility.FromJson<FriendSearchResponse>(request.downloadHandler.text);
            if (response.success)
            {
                DisplayFriendList(response.data);  // 친구 목록을 UI에 표시
            }
            else
            {
                Debug.LogError("친구 목록을 불러오는 데 실패했습니다: " + response.message);
            }
        }
        else
        {
            Debug.LogError("친구 목록을 불러오는 데 실패했습니다: " + request.error);
        }
    }

    // JSON 데이터 모델 정의
    [System.Serializable]
    public class FriendSearchDto
    {
        public int userId;         // 친구 ID
        public string username;    // 친구 이름
        public string email;       // 친구 이메일
        public int lastStage;      // 마지막 단계
        public bool friend;        // 친구 여부
    }

    [System.Serializable]
    public class FriendSearchResponse
    {
        public bool success;                       // 요청 성공 여부
        public List<FriendSearchDto> data;       // 친구 목록
        public string message;                     // 메시지
        public PageInfo pageInfo;                  // 페이지 정보
    }

    [System.Serializable]
    public class PageInfo
    {
        public int size;                           // 페이지 사이즈
        public int page;                           // 현재 페이지
        public int total;                          // 총 유저 수
        public int totalPages;                     // 총 페이지 수
        public int currentPage;                    // 현재 페이지 번호
    }

    private void DisplayFriendList(List<FriendSearchDto> friendList)
    {
        // 기존에 생성된 친구 항목들을 모두 삭제
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        // 새로운 친구 항목을 동적으로 생성
        foreach (FriendSearchDto friend in friendList)
        {
            GameObject newFriendItem = Instantiate(friendItemPrefab, contentPanel);  // 프리팹 생성
            newFriendItem.transform.Find("UsernameText").GetComponent<TextMeshProUGUI>().text = friend.username;  // 친구 이름 설정
        }
    }
}
