using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBigSmall : MonoBehaviour
{
    public RectTransform image; // 크기를 조절할 이미지의 RectTransform
    public float scaleSpeed = 2f; // 이미지 크기 변화 속도
    public float maxScale = 1.5f; // 이미지가 커질 최대 크기
    public float minScale = 0.5f; // 이미지가 작아질 최소 크기

    void Update()
    {
        // 시간에 따라 크기가 커졌다가 작아지는 값을 계산
        float scaleValue = Mathf.PingPong(Time.time * scaleSpeed, maxScale - minScale) + minScale;

        // 이미지의 크기를 설정
        image.localScale = new Vector3(scaleValue, scaleValue, 1f);
    }
}
