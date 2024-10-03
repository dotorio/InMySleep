using Photon.Pun.Demo.SlotRacer.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SkinGenerator : MonoBehaviour
{
    public string savePath = "Screenshots";
    public int width;
    public int height;
    public Camera camera;
    private Material[] bearMaterials;
    private Material[] bunnyMaterials;
    private Color baseColor;
    private int[] grade = { 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 };
    private int captureIndex = 0;

    void Start()
    {
        camera.aspect = (float)width / (float)height;
        LoadMaterials();

        StartCoroutine(CaptureScreenshots());
    }

    void LoadMaterials()
    {
        bearMaterials = Resources.LoadAll<Material>("Bear");
        bunnyMaterials = Resources.LoadAll<Material>("Bunny");

        Debug.Log($"Bear Materials Loaded: {bearMaterials.Length}");
        Debug.Log($"Bunny Materials Loaded: {bunnyMaterials.Length}");
    }

    void BgColorPicker()
    {
        Image line1 = GameObject.Find("Line1").GetComponent<Image>();
        Image line2 = GameObject.Find("Line2").GetComponent<Image>();
        Image line3 = GameObject.Find("Line3").GetComponent<Image>();
        Image line4 = GameObject.Find("Line4").GetComponent<Image>();

        baseColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));


        float brightness = (baseColor.r + baseColor.g + baseColor.b) / 3.0f;

        float brightnessThreshold = 0.5f;

        Color[] generatedColors = new Color[3];

        if (brightness < brightnessThreshold)
        {
            Debug.Log("Base Color is Dark. Generating Lighter Colors...");
            for (int i = 1; i <= 3; i++)
            {
                float factor = 1.0f - (0.25f * (3 - i));
                generatedColors[i - 1] = new Color(
                    Mathf.Clamp(baseColor.r * factor, 0f, 1f),
                    Mathf.Clamp(baseColor.g * factor, 0f, 1f),
                    Mathf.Clamp(baseColor.b * factor, 0f, 1f)
                );
                Debug.Log($"Lighter Color {i}: {generatedColors[i - 1]}");
            }
        }
        else
        {
            Debug.Log("Base Color is Bright. Generating Darker Colors...");
            for (int i = 1; i <= 3; i++)
            {
                float factor = 1.0f - (0.25f * i);
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

    IEnumerator CaptureScreenshots()
    {
        GameObject character = GameObject.Find("Character");
        Transform bearTransform = character.transform.Find("Bear");
        GameObject bearObject = bearTransform.gameObject;
        bearObject.SetActive(true);
        foreach (var material in bearMaterials)
        {
            yield return CaptureForMaterial(material, "Bear");
            captureIndex++;
        }
        bearObject.SetActive(false);

        Transform bunnyTransform = character.transform.Find("Bunny");
        GameObject bunnyObject = bunnyTransform.gameObject;
        bunnyObject.SetActive(true);
        foreach (var material in bunnyMaterials)
        {
            yield return CaptureForMaterial(material, "Bunny");
            captureIndex++;
        }
        bunnyObject.SetActive(false);
        Debug.Log("모든 스크린샷 촬영 완료");
    }

    IEnumerator CaptureForMaterial(Material material, string character)
    {
        Renderer skinRenderer = GameObject.Find($"{character}Skin").GetComponent<Renderer>();

        skinRenderer.material = material;
        yield return new WaitForEndOfFrame();

        BgColorPicker();

        SetGradeImage();

        Screenshot(material.name);

        yield return null;
    }

    void SetGradeImage()
    {
        Image gradeImage = GameObject.Find("Grade").GetComponent<Image>();
        string gradeFilePath = "";
        if (grade[captureIndex] == 1)
        {
            gradeFilePath = "grade/figma_star1";
        }
        else if (grade[captureIndex] == 2)
        {
            gradeFilePath = "grade/figma_star2";
        }
        else if (grade[captureIndex] == 3)
        {
            gradeFilePath = "grade/figma_star3";
        }

        Sprite gradeSprite = Resources.Load<Sprite>(gradeFilePath);
        if (gradeSprite != null)
        {
            gradeImage.sprite = gradeSprite;
        }
    }

    void Screenshot(string materialName)
    {
        RenderTexture rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGBA32, false);

        camera.Render();
        RenderTexture.active = rt;

        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();

        string directoryPath = $"{Application.dataPath}/{savePath}";
        if (!System.IO.Directory.Exists(directoryPath))
        {
            System.IO.Directory.CreateDirectory(directoryPath);
        }
        string rgb = $"{(int)(baseColor.r * 255)}_{(int)(baseColor.g * 255)}_{(int)(baseColor.b * 255)}";
        string filePath = $"{directoryPath}/{materialName}_RGB{rgb}_Grade{grade[captureIndex]}.png";

        byte[] bytes = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(filePath, bytes);

        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        Debug.Log($"Screenshot saved to: {filePath}");
    }
}