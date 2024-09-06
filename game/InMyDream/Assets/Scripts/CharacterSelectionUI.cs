using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class CharacterSelectionUI : MonoBehaviour
{
    public Button player1Button;
    public Button player2Button;
    public Button startGameButton;

    private testPhoton photonManager;

    private void Start()
    {
        photonManager = FindObjectOfType<testPhoton>();

        player1Button.onClick.AddListener(() => OnSelectCharacter("Player1"));
        player2Button.onClick.AddListener(() => OnSelectCharacter("Player2"));

        startGameButton.onClick.AddListener(() => photonManager.StartGame());

        // 초기에는 비활성화
        startGameButton.interactable = false;
    }

    private void OnSelectCharacter(string characterName)
    {
        photonManager.OnSelectCharacter(characterName);

        // 캐릭터 선택 후 게임 시작 버튼 활성화 (모든 플레이어가 선택된 후 호스트가 시작 가능)
        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.interactable = true;
        }
    }
}
