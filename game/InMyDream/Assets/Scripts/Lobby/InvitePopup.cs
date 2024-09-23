using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvitePopup : MonoBehaviour
{
    public GameObject inviteNoti;
    public FriendManager friendManager;
    private Button closeBtn;
    private Button yesBtn;
    private void OnEnable()
    {
        closeBtn = gameObject.transform.Find("Popup").transform.Find("Button_Close").GetComponent<Button>();
        yesBtn = gameObject.transform.Find("Popup").transform.Find("YesBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(Close);
        yesBtn.onClick.AddListener(() => friendManager.AcceptInvite(friendManager.roomNameText));
    }


    public void Close()
    {
        inviteNoti.SetActive(false);
    }
}
