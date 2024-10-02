using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Networking;

public class EasterEggData : MonoBehaviour
{
    private string character;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //StartCoroutine(AddEasterEgg());
            Debug.Log("이스터 에그 획득 성공");
            Destroy(gameObject);
        }
    }

    // 임시 코드, api 완성시 수정
    IEnumerator AddEasterEgg()
    {
        // 로그인 정보를 JSON 형식으로 준비
        LoginData loginData = new LoginData("a", "a");
        string jsonData = JsonUtility.ToJson(loginData);

        // UnityWebRequest로 HTTP POST 요청을 준비
        UnityWebRequest request = new UnityWebRequest("a", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // 서버로부터 응답을 받을 때까지 대기
        yield return request.SendWebRequest();

        // 요청 결과 처리
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            // 로그인 실패 시 오류 메시지 표시
            //errorMessageText.text = "로그인 실패: " + response.message;
            //ShowLoginFailPopup();  // 로그인 실패 팝업창 띄우기

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

                Debug.Log("로그인 성공");
                Debug.Log("username: " + response.data.username);
                Debug.Log("userId: " + response.data.userId);
                Debug.Log("email: " + response.data.email);
                Debug.Log("lastStage: " + response.data.lastStage);
                UserData.instance.email = response.data.email;
                UserData.instance.userName = response.data.username;
                UserData.instance.userId = response.data.userId;
                UserData.instance.lastStage = response.data.lastStage;

                //SceneManager.LoadScene("LobbyScene");

                //// 로그인 성공 시 사용자 정보 저장 등 처리 (PlayerPrefs 사용)
                //PlayerPrefs.SetInt("userId", response.data.userId);
                //PlayerPrefs.SetString("username", response.data.username);
                //PlayerPrefs.SetString("email", response.data.email);
                //PlayerPrefs.SetInt("lastStage", response.data.lastStage);
                //PlayerPrefs.Save();

                // 로그인 성공 시 팝업창 닫기 또는 다른 씬으로 전환
                //loginPanel.SetActive(false);
            }
            else
            {
                // 로그인 실패 시 오류 메시지 표시
                //errorMessageText.text = "로그인 실패: " + response.message;
                //ShowLoginFailPopup();  // 로그인 실패 팝업창 띄우기
            }
        }
    }
}
