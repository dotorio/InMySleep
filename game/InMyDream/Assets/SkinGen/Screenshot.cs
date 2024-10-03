using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotTaker : MonoBehaviour
{
    public Camera cameraToUse; // 스크린샷을 찍을 카메라
    public int width = 280; // 이미지 너비
    public int height = 360; // 이미지 높이
    public string savePath = "Screenshots"; // 저장할 폴더 이름

    void Start()
    {
        TakeScreenshot();
    }

    public void TakeScreenshot()
    {
        string fileName = "Test";
        RenderTexture rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        cameraToUse.targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGBA32, false);

        // 카메라 렌더링
        cameraToUse.Render();
        RenderTexture.active = rt;

        // 화면에서 픽셀을 읽어와 텍스처로 변환
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();

        // 파일 경로 설정
        string directoryPath = $"{Application.dataPath}/{savePath}";
        if (!System.IO.Directory.Exists(directoryPath))
        {
            System.IO.Directory.CreateDirectory(directoryPath);
        }
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string filePath = $"{directoryPath}/{fileName}_{timestamp}.png";

        // PNG로 저장
        byte[] bytes = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(filePath, bytes);

        // 정리 작업
        cameraToUse.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        Debug.Log($"Screenshot saved to: {filePath}");
    }

}
