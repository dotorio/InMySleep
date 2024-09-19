using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvitePopup : MonoBehaviour
{
    public GameObject inviteNoti;
    public FriendManager friendManager;
    private Button closeBtn;
    private Button noBtn;
    private Button yesBtn;
    private void OnEnable()
    {
        closeBtn = gameObject.transform.Find("TopFrame_Light").transform.Find("Button_Close").GetComponent<Button>();
        noBtn = gameObject.transform.Find("NoBtn").GetComponent<Button>();
        yesBtn = gameObject.transform.Find("YesBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(Close);
        noBtn.onClick.AddListener(Close);
        yesBtn.onClick.AddListener(() => friendManager.AcceptInvite(friendManager.roomNameText));
    }


    void Close()
    {
        inviteNoti.SetActive(false);
    }
}
