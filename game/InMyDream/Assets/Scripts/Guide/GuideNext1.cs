using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GuideNext1 : MonoBehaviourPunCallbacks
{
    public Image[] images; // 여러 개의 이미지를 드래그해서 연결
    public TextMeshProUGUI[] texts; // 여러 개의 TMP 텍스트를 드래그해서 연결
    private int currentIndex = 0; // 현재 표시 중인 이미지 및 텍스트 인덱스
    private int shownImageCount = 0; // 현재 화면에 보여진 이미지 개수
    public GameObject Allow1; // 화살표 오브젝트 2-1
    public GameObject Allow21;
    public GameObject Allow22;
    bool isNext = false;

    void Start()
    {
        // 모든 이미지를 투명하게 설정 (알파값 0)
        foreach (Image img in images)
        {
            Color color = img.color;
            color.a = 0f;
            img.color = color;
        }

        // 모든 텍스트를 투명하게 설정 (알파값 0)
        foreach (TextMeshProUGUI txt in texts)
        {
            Color color = txt.color;
            color.a = 0f;
            txt.color = color;
        }

        // 첫 번째 이미지와 텍스트를 즉시 나타내기 시작
        if (images.Length > 0 && texts.Length > 0)
        {
            ShowImageImmediately(images[currentIndex]);
            ShowTextImmediately(texts[currentIndex]);
            shownImageCount++;
        }
    }

    void Update()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (isNext)
            {
                SceneManager.LoadScene("2s_Scene");
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isNext = true;
            }

            // Spacebar가 눌렸을 때 다음 이미지와 텍스트를 나타냄
            if (Input.GetKeyDown(KeyCode.Space))
            {
                photonView.RPC("NextImageSequence", RpcTarget.AllBuffered); // 모든 클라이언트에게 RPC 호출
            }
        }
    }

    [PunRPC]
    public void NextImageSequence()
    {
        if (currentIndex < images.Length - 1 && currentIndex < texts.Length - 1)
        {
            // 이전 이미지와 텍스트를 즉시 숨기기
            if (currentIndex >= 0) // 첫 번째 이미지부터 이전 이미지 숨기기
            {
                HideImageImmediately(images[currentIndex]); // 이전 이미지 즉시 사라지게 함
                HideTextImmediately(texts[currentIndex]); // 이전 텍스트 즉시 사라지게 함
            }

            // 다음 이미지 및 텍스트로 이동
            currentIndex++;

            ShowImageImmediately(images[currentIndex]); // 다음 이미지를 즉시 나타내기
            ShowTextImmediately(texts[currentIndex]);   // 다음 텍스트를 즉시 나타내기
            shownImageCount++; // 표시된 이미지 개수 증가

            if (currentIndex >= 2)
            {
                isNext = true;
            }

            // 캐릭터가 이미지 2번째(인덱스 0)일 때만 나타나도록 설정
            if (currentIndex == 0)
            {
                Allow1.SetActive(true); // 화살표1 활성화
            }
            else
            {
                Allow1.SetActive(false); // 화살표1 비활성화
            }

            // 캐릭터가 이미지 2번째(인덱스 1)일 때만 나타나도록 설정
            if (currentIndex == 1)
            {
                Allow21.SetActive(true); // 화살표2 활성화
                Allow22.SetActive(true); // 화살표2 활성화
            }
            else
            {
                Allow21.SetActive(false); // 화살표2 비활성화
                Allow22.SetActive(false); // 화살표2 비활성화
            }
        }
    }

    // 특정 이미지를 즉시 나타내는 함수
    void ShowImageImmediately(Image img)
    {
        Color color = img.color;
        color.a = 1f; // 알파 값을 즉시 1로 설정
        img.color = color;
    }

    // 특정 텍스트를 즉시 나타내는 함수
    void ShowTextImmediately(TextMeshProUGUI txt)
    {
        Color color = txt.color;
        color.a = 1f; // 알파 값을 즉시 1로 설정
        txt.color = color;
    }

    // 특정 이미지를 즉시 숨기는 함수
    void HideImageImmediately(Image img)
    {
        Color color = img.color;
        color.a = 0f; // 알파 값을 즉시 0으로 설정
        img.color = color;
    }

    // 모든 이미지를 즉시 숨기는 함수
    void HideAllImages()
    {
        foreach (Image img in images)
        {
            Color color = img.color;
            color.a = 0f; // 알파 값을 즉시 0으로 설정
            img.color = color;
        }
    }

    // 텍스트를 즉시 사라지게 설정하는 함수
    void HideTextImmediately(TextMeshProUGUI txt)
    {
        Color color = txt.color;
        color.a = 0f; // 알파 값을 즉시 0으로 설정
        txt.color = color;
    }
}
