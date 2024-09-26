using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ImageSequenceAnimation : MonoBehaviourPunCallbacks
{
    public Image[] images; // 여러 개의 이미지를 드래그해서 연결
    public TextMeshProUGUI[] texts; // 여러 개의 TMP 텍스트를 드래그해서 연결
    public float fadeDuration = 2.0f; // 페이드 지속 시간
    private int currentIndex = 0; // 현재 표시 중인 이미지 및 텍스트 인덱스
    private int shownImageCount = 0; // 현재 화면에 보여진 이미지 개수
    public GameObject character4; // 캐릭터 오브젝트 4
    public GameObject character51; // 캐릭터 오브젝트 5-1
    public GameObject character52; // 캐릭터 오브젝트 5-2
    public GameObject character61; // 캐릭터 오브젝트 6-1
    public GameObject character62; // 캐릭터 오브젝트 6-2
   
    
    public AudioSource nextCutAudio;
    public AudioSource BGM1;
    public AudioSource BGM2;
    

    private Coroutine currentFadeInCoroutineImage; // 현재 진행 중인 이미지 페이드 인 코루틴
    private Coroutine currentFadeInCoroutineText; // 현재 진행 중인 텍스트 페이드 인 코루틴
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

        // 첫 번째 이미지와 텍스트를 서서히 나타내기 시작
        if (images.Length > 0 && texts.Length > 0)
        {
            currentFadeInCoroutineImage = StartCoroutine(FadeInImage(images[currentIndex]));
            currentFadeInCoroutineText = StartCoroutine(FadeInText(texts[currentIndex]));
            shownImageCount++;
        }
    }

    void Update()
    {
        //실제에 사용할 코드
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (isNext)
            {
                SceneManager.LoadScene("2s_Scene 1");
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
        // Spacebar가 눌렸을 때 다음 이미지와 텍스트를 나타냄
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    nextCutAudio.Play();
        //    NextImageSequence();
        //}
    }

    [PunRPC]
    public void NextImageSequence()
    {
        if (currentIndex < images.Length - 1 && currentIndex < texts.Length - 1)
        {
            // 현재 진행 중인 페이드 인 코루틴을 중단
            if (currentFadeInCoroutineImage != null)
            {
                StopCoroutine(currentFadeInCoroutineImage);
            }
            if (currentFadeInCoroutineText != null)
            {
                StopCoroutine(currentFadeInCoroutineText);
            }

            // 3개의 이미지가 보여졌다면 모두 숨기기
            if (shownImageCount >= 3)
            {
                HideAllImages();
                shownImageCount = 0; // 카운트 초기화
            }

            // 현재 텍스트를 즉시 사라지게 만듦
            HideTextImmediately(texts[currentIndex]);

            // 4번째 이미지 이후부터는 이전 이미지를 즉시 사라지게 함
            if (currentIndex >= 3)
            {
               
                HideImageImmediately(images[currentIndex]); // 이전 이미지 즉시 사라지게 함
            }

            // 다음 이미지 및 텍스트로 이동
            currentIndex++;

            currentFadeInCoroutineImage = StartCoroutine(FadeInImage(images[currentIndex])); // 다음 이미지를 페이드 인
            currentFadeInCoroutineText = StartCoroutine(FadeInText(texts[currentIndex]));   // 다음 텍스트를 페이드 인
            shownImageCount++; // 표시된 이미지 개수 증가

            if (currentIndex >= 6)
            {
                isNext = true;
            }

            // 캐릭터가 이미지 4번째(인덱스 3)일 때만 나타나도록 설정
            if (currentIndex == 3)
            {
                BGM1.Stop();
                BGM2.Play();
                character4.SetActive(true); // 캐릭터 활성화
            }
            else
            {
                character4.SetActive(false); // 캐릭터 비활성화
            }

            // 캐릭터가 이미지 5번째(인덱스 4)일 때
            if (currentIndex == 4)
            {
                character51.SetActive(true); // 캐릭터 활성화
                character52.SetActive(true); // 캐릭터 활성화
            }
            else
            {
                character51.SetActive(false); // 캐릭터 비활성화
                character52.SetActive(false); // 캐릭터 비활성화
            }

            // 캐릭터가 이미지 6번째(인덱스 5)일 때
            if (currentIndex == 5)
            {
                character61.SetActive(true); // 캐릭터 활성화
                character62.SetActive(true); // 캐릭터 활성화
            }
            else
            {
                character61.SetActive(false); // 캐릭터 비활성화
                character62.SetActive(false); // 캐릭터 비활성화
            }
        }
    }

    IEnumerator FadeInImage(Image img)
    {
        float elapsedTime = 0f;
        Color color = img.color;

        // 이미지 페이드 인 (서서히 나타남)
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            img.color = color;
            yield return null;
        }

        // 최종 알파값 1로 고정
        color.a = 1f;
        img.color = color;
    }

    IEnumerator FadeInText(TextMeshProUGUI txt)
    {
        float elapsedTime = 0f;
        Color color = txt.color;

        // 텍스트 페이드 인 (서서히 나타남)
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            txt.color = color;
            yield return null;
        }

        // 최종 알파값 1로 고정
        color.a = 1f;
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
