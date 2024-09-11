using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField inputEmail; // 사용자 이메일 입력 필드
    public TMP_InputField inputPassword; // 비밀번호 입력 필드
    //public InputField emailInputField;  
    //public InputField passwordInputField;  
    public Text errorMessageText;          // 오류 메시지 표시 텍스트
    public GameObject loginPanel;          // 로그인 팝업창 패널

    private string loginUrl = "https://j11e107.p.ssafy.io:8000/api/v1/auth/login";  // 서버의 로그인 API URL

    // 로그인 버튼 클릭 시 호출
    public void OnLoginButtonClicked()
    {
        string email = inputEmail.text;
        string password = inputPassword.text;

        // 입력한 정보를 서버로 전송하여 로그인 요청
        StartCoroutine(LoginRequest(email, password));
    }

    // 로그인 요청을 처리하는 코루틴 함수
    IEnumerator LoginRequest(string email, string password)
    {
        // 로그인 정보를 JSON 형식으로 준비
        LoginData loginData = new LoginData(email, password);
        string jsonData = JsonUtility.ToJson(loginData);

        // UnityWebRequest로 HTTP POST 요청을 준비
        UnityWebRequest request = new UnityWebRequest(loginUrl, "POST");
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
            errorMessageText.text = "서버 연결 오류: " + request.error;
        }
        else
        {
            // 응답 성공 시 처리
            Debug.Log("Response: " + request.downloadHandler.text);

            // 서버로부터 받은 JSON 응답을 파싱
            ServerResponse response = JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);

            if (response.success)
            {
                Debug.Log("로그인 성공! 사용자 ID: " + response.userId);

                // 로그인 성공 시 토큰 저장 등 처리 (PlayerPrefs 사용)
                PlayerPrefs.SetString("token", response.token);
                PlayerPrefs.SetInt("userId", response.userId);
                PlayerPrefs.Save();

                // 로그인 성공 시 팝업창 닫기 또는 다른 씬으로 전환
                loginPanel.SetActive(false);
            }
            else
            {
                // 로그인 실패 시 오류 메시지 표시
                errorMessageText.text = "로그인 실패: " + response.message;
            }
        }
    }
}

// 로그인 요청 데이터를 나타내는 클래스
[System.Serializable]
public class LoginData
{
    public string email;
    public string password;

    public LoginData(string email, string password)
    {
        this.email = email;
        this.password = password;
    }
}

// 서버로부터 받은 응답 데이터를 나타내는 클래스
[System.Serializable]
public class ServerResponse
{
    public bool success;     // 로그인 성공 여부
    public string token;     // 인증 토큰
    public int userId;       // 사용자 ID
    public string message;   // 오류 메시지
}
