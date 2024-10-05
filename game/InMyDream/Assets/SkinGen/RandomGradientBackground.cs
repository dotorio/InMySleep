using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradientRandomizer : MonoBehaviour
{
    public Material gradientMaterial;

    void Start()
    {
        ApplyRandomGradient();
    }

    void ApplyRandomGradient()
    {
        // 각 색상을 랜덤으로 설정합니다.
        Color color1 = Random.ColorHSV();
        Color color2 = Random.ColorHSV();
        Color color3 = Random.ColorHSV();
        Color color4 = Random.ColorHSV();

        // Material에 있는 Shader의 색상 속성을 변경합니다.
        gradientMaterial.SetColor("_Color1", color1);
        gradientMaterial.SetColor("_Color2", color2);
        gradientMaterial.SetColor("_Color3", color3);
        gradientMaterial.SetColor("_Color4", color4);
    }
}

