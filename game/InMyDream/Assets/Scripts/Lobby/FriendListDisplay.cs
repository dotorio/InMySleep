using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendListDisplay : MonoBehaviour
{
    public FriendManager friendManager;  // FriendManager 스크립트 참조
    public GameObject friendItemPrefab;  // 친구 목록 아이템 프리팹
    public Transform contentPanel;       // Scroll View의 Content


    void OnEnable()
    {
        StartCoroutine(LoadAndDisplayFriends());
    }

    void OnDisable()
    {
        ClearFriendList();
    }

    void ClearFriendList()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);  // ContentPanel의 자식 오브젝트(친구 항목) 제거
        }
    }

    IEnumerator LoadAndDisplayFriends()
    {
        // FriendManager의 LoadFriendList 코루틴 실행
        yield return StartCoroutine(friendManager.LoadFriendList());

        // 로드된 친구 목록을 가져옴
        List<FriendDto> friends = friendManager.friendList;

        // 친구 목록이 있으면 동적으로 UI 항목을 생성
        if (friends != null && friends.Count > 0)
        {
            foreach (FriendDto friend in friends)
            {
                // 친구 항목 UI 프리팹을 생성하고 Content에 자식으로 추가
                GameObject newFriendItem = Instantiate(friendItemPrefab, contentPanel);
                // 생성된 친구 항목의 텍스트를 설정
                Debug.Log(friend);
                newFriendItem.transform.Find("UsernameText").GetComponent<TextMeshProUGUI>().text = friend.username;
            }
        }
        else
        {
            Debug.Log("친구 목록이 비어 있습니다.");
        }
    }
}