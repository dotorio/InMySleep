using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        // 선택된 캐릭터 정보를 불러옵니다.
        string characterName = (string)PhotonNetwork.LocalPlayer.CustomProperties["character"];
        if (string.IsNullOrEmpty(characterName))
        {
            Debug.LogError("캐릭터가 선택되지 않았습니다.");
            return;
        }

        Debug.Log($"선택된 캐릭터: {characterName}");

        Transform[] points = GameObject.Find("CreatePlayerGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);

        // 선택된 캐릭터 프리팹을 인스턴스화합니다.
        GameObject instantiatedCharacter = PhotonNetwork.Instantiate(characterName, points[idx].position, points[idx].rotation, 0);

        if (instantiatedCharacter == null)
        {
            Debug.LogError("캐릭터 인스턴스화에 실패했습니다.");
        }
        else
        {
            Debug.Log("캐릭터가 성공적으로 인스턴스화되었습니다.");
        }
    }
}
