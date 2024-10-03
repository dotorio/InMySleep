using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundColorPick : MonoBehaviour
{
    public Image line1;
    public Image line2;
    public Image line3;
    public Image line4;

    void Start()
    {
        // 1. 베이스 컬러 뽑기
        Color baseColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        // 베이스 컬러의 명도 계산
        float brightness = (baseColor.r + baseColor.g + baseColor.b) / 3.0f;

        // 기준 명도 값 설정 (0.5 기준으로 밝고 어두움을 나눈다)
        float brightnessThreshold = 0.5f;

        Color[] generatedColors = new Color[3];

        // 2. 베이스 컬러가 어두운 경우: 점차 밝아지는 3개의 색깔 생성
        if (brightness < brightnessThreshold)
        {
            Debug.Log("Base Color is Dark. Generating Lighter Colors...");
            for (int i = 1; i <= 3; i++)
            {
                float factor = 1.0f - (0.25f * (3 - i));  // 점차 밝아지는 계수 (0.5, 0.75, 1.0)
                generatedColors[i - 1] = new Color(
                    Mathf.Clamp(baseColor.r * factor, 0f, 1f),
                    Mathf.Clamp(baseColor.g * factor, 0f, 1f),
                    Mathf.Clamp(baseColor.b * factor, 0f, 1f)
                );
                Debug.Log($"Lighter Color {i}: {generatedColors[i - 1]}");
            }
        }
        // 3. 베이스 컬러가 밝은 경우: 점차 어두워지는 3개의 색깔 생성
        else
        {
            Debug.Log("Base Color is Bright. Generating Darker Colors...");
            for (int i = 1; i <= 3; i++)
            {
                float factor = 1.0f - (0.25f * i);  // 점차 어두워지는 계수 (0.75, 0.5, 0.25)
                generatedColors[i - 1] = new Color(
                    Mathf.Clamp(baseColor.r * factor, 0f, 1f),
                    Mathf.Clamp(baseColor.g * factor, 0f, 1f),
                    Mathf.Clamp(baseColor.b * factor, 0f, 1f)
                );
                Debug.Log($"Darker Color {i}: {generatedColors[i - 1]}");
            }
        }
        line1.color = baseColor;
        line2.color = generatedColors[0];
        line3.color = generatedColors[1];
        line4.color = generatedColors[2];
    }
}
