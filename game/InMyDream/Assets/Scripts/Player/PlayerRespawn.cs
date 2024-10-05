using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerRespawn : MonoBehaviour
{
    public TextMeshProUGUI timerText; // TextMeshProUGUI를 참조하기 위한 변수

    void OnEnable()
    {
        StartCoroutine(UpdateTimerText());
    }

    IEnumerator UpdateTimerText()
    {
        float duration = 3f; // 총 시간 3초
        float currentTime = 0f;

        // 3초 동안 시간이 흐르는 것을 텍스트에 반영
        while (currentTime < duration)
        {
            //Debug.Log("시간아~~");
            currentTime += Time.deltaTime; // 델타 타임만큼 시간 추가
            timerText.text = currentTime.ToString("F2") + " 초"; // 소수점 두 자리까지 표시
            yield return null; // 다음 프레임까지 대기
        }

        // 3초가 지나면 최종 시간을 고정
        timerText.text = "3.00 초";
    }
}
