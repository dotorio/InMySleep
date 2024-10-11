using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlider : MonoBehaviour
{
    public RectTransform warningText; // 위쪽 텍스트의 RectTransform
    public float speed = 200f;  // 위쪽 텍스트 슬라이드 속도
    public bool isTop; 
    
    private float startPos;  // 위쪽 텍스트의 시작 위치

    void Start()
    {
        // 각 텍스트의 시작 위치 저장
        startPos = warningText.anchoredPosition.x;
        
    }

    void Update()
    {
        if (isTop)
        {
            warningText.anchoredPosition += Vector2.right * speed * Time.deltaTime;
        }

        else
        {
            warningText.anchoredPosition += Vector2.left * speed * Time.deltaTime;
        }
    }
}
