using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InviteSender : MonoBehaviour
{
    // Start is called before the first frame update
    public FriendManager friendManager;

    public void OnInviteButtonClick()
    {
        //// TextMeshProUGUI에서 텍스트를 가져옴
        //string friendName = gameObject.transform.parent.Find("UsernameText").GetComponent<TextMeshProUGUI>().text;
        //Debug.Log("친구 추가: " + friendName);
        //friendManager.SendInvite(friendName);
       
        Transform usernameTransform = transform.Find("UsernameText");
        if (usernameTransform != null)
        {
            string friendName = usernameTransform.GetComponent<TextMeshProUGUI>().text;
            Debug.Log("친구 추가: " + friendName);
            friendManager.SendInvite(friendName);
        }
        else
        {
            Debug.LogError("UsernameText 오브젝트를 찾을 수 없습니다.");
        }
    }
}
