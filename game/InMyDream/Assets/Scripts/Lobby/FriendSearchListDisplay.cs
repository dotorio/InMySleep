using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendSearchListDisplay : MonoBehaviour
{
    public Transform contentPanel;       // Scroll View의 Content
    public TMP_InputField searchInputField;  // TMP_InputField 사용

    void OnDisable()
    {
        ClearFriendSearchList();
    }
    void ClearFriendSearchList()
    {
        Debug.Log(searchInputField.text);
        searchInputField.text = string.Empty;
        Debug.Log(searchInputField.text);
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);  // ContentPanel의 자식 오브젝트(친구 항목) 제거
        }
    }
}
