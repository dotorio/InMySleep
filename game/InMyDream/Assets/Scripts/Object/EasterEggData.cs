using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class EasterEggData : MonoBehaviour
{
    private string character;
    private int skin;
    private string addEasterUrl = "https://j11e107.p.ssafy.io:8000/api/v1/easter/add-skin";
    [SerializeField] private GameObject easterEggPanel;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image skinImage;

    [System.Serializable]
    public class ServerResponse
    {
        public bool success;    // 로그인 성공 여부
        public EasterEggInfo data;    // 유저 정보가 포함된 "data" 필드
        public string message;   // 서버에서 반환된 메시지
    }

    [System.Serializable]
    public class Attributes
    {
        public int color;
        public string character;
    }

    [System.Serializable]
    public class EasterEggInfo
    {
        public string skinImgUrl;
        public string description;
        public Attributes attributes;
        public bool duplicated;
    }

    [System.Serializable]
    public class UserData
    {
        public int userId;

        public UserData(int userId)
        {
            this.userId = userId;
        }
    }

    private void Start()
    {
        easterEggPanel.SetActive(true);
        /*        int easterData = (int)PhotonNetwork.CurrentRoom.CustomProperties["EasterEggData"];

                if (easterData <= 5)
                {
                    character = "Bear";
                    skin = easterData;
                }
                else
                {
                    character = "Bunny";
                    skin = easterData - 5;
                }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(AddEasterEgg());
            Debug.Log("이스터 에그 획득 성공");
        }
    }

    // 임시 코드, api 완성시 수정
    IEnumerator AddEasterEgg()
    {
        // 로그인 정보를 JSON 형식으로 준비
        /*int userId = UserData.instance.userId;*/
        int userId = 45;
        string jsonData = $"{{\"userId\":{userId}}}";

        // UnityWebRequest로 HTTP POST 요청을 준비
        UnityWebRequest request = new UnityWebRequest(addEasterUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // 서버로부터 응답을 받을 때까지 대기

        yield return request.SendWebRequest();
        // 요청 결과 처리
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {

            // 에러 발생 시 처리
            Debug.LogError("Error: " + request.error);
            //errorMessageText.text = "서버 연결 오류: " + request.error;
        }
        else
        {
            // 응답 성공 시 처리
            Debug.Log("Response: " + request.downloadHandler.text);

            // 서버로부터 받은 JSON 응답을 파싱
            ServerResponse response = JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);

            if (response.success)
            {

                Debug.Log("이스터에그 획득 성공");
                Debug.Log("skinImgUrl: " + response.data.skinImgUrl);
                Debug.Log("description: " + response.data.description);
                Debug.Log("attributes: " + response.data.attributes.character);
                Debug.Log("attributes: " + response.data.attributes.color);
                Debug.Log("duplicated: " + response.data.duplicated);
                EasterEggInfo easterEggInfo = new EasterEggInfo();
                easterEggInfo.skinImgUrl = response.data.skinImgUrl;
                easterEggInfo.description = response.data.description;
                Attributes attributes = new Attributes();
                attributes.character = response.data.attributes.character;
                attributes.color = response.data.attributes.color;
                easterEggInfo.attributes = attributes;
                easterEggInfo.duplicated = response.data.duplicated;

                StartCoroutine(LoadImageFromUrl(easterEggInfo.skinImgUrl));

                yield return new WaitForSeconds(3f);
                easterEggPanel.SetActive(false);
            }
            else
            {
                Debug.Log("이스터에그 획득 실패");
            }
        }
        Destroy(gameObject);
    }
    private IEnumerator LoadImageFromUrl(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            skinImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            Debug.LogError("이미지 로드 실패: " + request.error);
        }
    }
}
