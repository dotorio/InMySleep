using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAllow : MonoBehaviour
{
    public RectTransform allow;  // UI 오브젝트는 RectTransform을 사용해야 함
    public float floatAmplitude = 20f;   // 위아래로 움직일 범위 (UI는 값이 크기 때문에 조정 필요)
    public float floatSpeed = 4f;        // 위아래로 움직이는 속도

    private Vector2 initialPosition;     // 초기 위치 저장
    private bool isFloating = false;     // 움직임 상태를 제어하는 플래그

    void Start()
    {
        // 초기 위치 설정
        initialPosition = allow.anchoredPosition;
        allow.gameObject.SetActive(false);  // 초기에는 비활성화 상태
    }

    void Update()
    {
        // allow 오브젝트가 활성화되었을 때만 움직임 시작
        if (allow.gameObject.activeSelf && !isFloating)
        {
            StartCoroutine(FloatArrow());
            isFloating = true;  // 중복 실행 방지
        }
        else if (!allow.gameObject.activeSelf)
        {
            isFloating = false;  // 비활성화되면 다시 초기화
        }
    }

    IEnumerator FloatArrow()
    {
        while (allow.gameObject.activeSelf)
        {
            // 위아래로 움직이는 효과 (anchoredPosition 사용)
            float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            allow.anchoredPosition = new Vector2(initialPosition.x, newY);

            // 매 프레임마다 대기
            yield return null;
        }

        // allow가 비활성화되면 원래 위치로 복구
        allow.anchoredPosition = initialPosition;
    }


}
