using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpUIImage : MonoBehaviour
{
    public RectTransform uiImage; // 움직일 UI Image의 RectTransform
    public float speed = 5f; // 이동 속도
    public float distance = 10f; // 이동할 거리

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = uiImage.localPosition; // 초기 위치 저장
    }

    void Update()
    {
        // X축과 Y축 모두 대각선 왼쪽 아래로 이동하도록 Sin 함수 사용
        float offset = Mathf.Sin(Time.time * speed) * distance;
        uiImage.localPosition = new Vector3(initialPosition.x + offset, initialPosition.y + offset, initialPosition.z);
    }
}
