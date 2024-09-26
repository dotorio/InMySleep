using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageController : MonoBehaviour
{
    public List<GameObject> stageObjects; // 스테이지 오브젝트 리스트
    public Button prevButton; // 이전 버튼
    public Button nextButton; // 다음 버튼

    
    private int lastStage;
    public int currentStageIndex = 0; // 현재 스테이지 인덱스

    void Start()
    {
        // 변수 초기화
        lastStage = UserData.instance.lastStage - 1;

        // 1스테이지 시작
        UserData.instance.stage = 1;

        UpdateStageObjects(); // 초기 스테이지 설정
        UnlockStages(); // 스테이지 락 해제

        // 버튼에 클릭 이벤트 연결
        prevButton.onClick.AddListener(OnPrevButtonClick);
        nextButton.onClick.AddListener(OnNextButtonClick);
    }

    // lastStage 이하의 스테이지는 Lock 이미지를 비활성화
    void UnlockStages()
    {
        for (int i = 0; i <= lastStage && i < stageObjects.Count; i++)
        {
            // "Lock"이라는 이름의 자식 오브젝트에서 Image 컴포넌트를 찾음
            Image lockImage = stageObjects[i].transform.Find("InnerFrame").transform.Find("Lock").GetComponent<Image>();
            if (lockImage != null)
            {
                lockImage.enabled = false; // Lock 이미지를 비활성화
            }
        }
    }

    void UpdateStageObjects()
    {
        // 모든 스테이지 오브젝트를 비활성화
        foreach (var stageObject in stageObjects)
        {
            stageObject.SetActive(false);

        }

        // 현재 선택된 스테이지 오브젝트만 활성화
        if (currentStageIndex >= 0 && currentStageIndex < stageObjects.Count)
        {
            stageObjects[currentStageIndex].SetActive(true);
        }

        // 버튼 활성화/비활성화 처리 (첫 번째 또는 마지막 스테이지일 경우)
        prevButton.interactable = currentStageIndex > 0;
        nextButton.interactable = currentStageIndex < stageObjects.Count - 1;
    }

    void OnPrevButtonClick()
    {
        // 이전 스테이지로 이동
        if (currentStageIndex > 0)
        {
            currentStageIndex--;
            UpdateStageObjects();
            Debug.Log(currentStageIndex);
        }
    }

    void OnNextButtonClick()
    {
        // 다음 스테이지로 이동
        if (currentStageIndex < stageObjects.Count - 1)
        {
            currentStageIndex++;
            UpdateStageObjects();
            Debug.Log(currentStageIndex);
        }
    }
}
