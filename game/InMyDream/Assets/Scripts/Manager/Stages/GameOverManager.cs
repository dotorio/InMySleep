using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement; // 씬 전환을 위해 필요

public class GameOverManager : MonoBehaviour
{
    void Start()
    {
        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
        playerProps["isDowned"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);

        // 코루틴을 사용하여 3초 후에 씬을 전환
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            
            
            StartCoroutine(ChangeSceneAfterDelay(3f, "3s_Scene"));
        }
    }

    IEnumerator ChangeSceneAfterDelay(float delay, string sceneName)
    {
        // 설정한 시간만큼 대기
        yield return new WaitForSeconds(delay);

        // 씬 전환
        PhotonNetwork.LoadLevel(sceneName);
    }
}
