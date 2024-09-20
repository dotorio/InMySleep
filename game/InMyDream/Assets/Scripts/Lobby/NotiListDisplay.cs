using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using TMPro.Examples;

public class NotiListDisplay : MonoBehaviour
{
    public FriendManager friendManager;  // FriendManager 스크립트 참조
    public GameObject notiItemPrefab;  // 친구 목록 아이템 프리팹
    public Transform contentPanel;       // Scroll View의 Content
    void OnEnable()
    {
        StartCoroutine(LoadAndDisplayNotis());
    }

    IEnumerator ClearNotiList()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);  // ContentPanel의 자식 오브젝트(친구 항목) 제거
        }


        yield return null;
    }

    IEnumerator LoadAndDisplayNotis()
    {
        yield return StartCoroutine(ClearNotiList());
        // FriendManager의 LoadFriendList 코루틴 실행
        yield return StartCoroutine(friendManager.LoadReceiveFriendList());

        // 로드된 친구 목록을 가져옴
        List<UserInfo> receiveFriends = friendManager.receiveFriends;
        //Debug.Log(friendManager.receiveFriends);

        // 친구 목록이 있으면 동적으로 UI 항목을 생성
        if (receiveFriends != null && receiveFriends.Count > 0)
        {
            foreach (UserInfo friend in receiveFriends)
            {
                Debug.Log(friend.username);
                Debug.Log(friend.userId);

                // 친구 항목 UI 프리팹을 생성하고 Content에 자식으로 추가
                GameObject newFriendItem = Instantiate(notiItemPrefab, contentPanel);
                // 생성된 친구 항목의 초대 버튼 가져오기
                Button yesBtn = newFriendItem.transform.Find("YesBtn").GetComponent<Button>();
                Button noBtn = newFriendItem.transform.Find("NoBtn").GetComponent<Button>();
                
                // 생성된 친구 항목의 텍스트를 설정
                newFriendItem.transform.Find("UsernameText").GetComponent<TextMeshProUGUI>().text = friend.username;
                yesBtn.onClick.AddListener(() => AcceptFriend(friend.userId));
                noBtn.onClick.AddListener(() => RefuseFriend(friend.userId));
            }
        }
        else
        {
            Debug.Log("친구 목록이 비어 있습니다.");
        }
    }

    void AcceptFriend(int userId)
    {
        StartCoroutine(AcceptFriendAndReload(userId));
    }

    IEnumerator AcceptFriendAndReload(int userId)
    {
        // 친구 요청 수락이 완료될 때까지 대기
        yield return StartCoroutine(friendManager.AcceptFriendRequest(userId));

        // 수락 후 다시 친구 목록을 불러옴
        yield return StartCoroutine(LoadAndDisplayNotis());
    }

    void RefuseFriend(int userId)
    {
        StartCoroutine(friendManager.RefuseFriendRequest(userId));
        StartCoroutine(LoadAndDisplayNotis());
    }
}
