using Photon.Pun.Demo.SlotRacer.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SkinGenerator : MonoBehaviour
{
    public string savePath = "Screenshots";
    public int width;
    public int height;

    private Material[] bearMaterials;
    private Material[] bunnyMaterials;
    private Material[] faces;

    private GameObject[] Character;
    private GameObject[] BackPack;
    private GameObject[] CrossBag;
    private GameObject[] Scarf;

    private GameObject[] Glasses;
    private GameObject[] Hat;
    private GameObject[] Mustache;

    private GameObject[] instantiatedPrefabs = new GameObject[100];
    private int instantiatedIndex = 0;
    
    private Color baseColor;
    //private int[] grade = { 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 };

    private int captureIndex = 0;

    void Start()
    {
        //camera.aspect = (float)width / (float)height;
        
        LoadMaterials();
        LoadPrefabs();
        //LoadGrade();

        StartCoroutine(StartCapture());
        Debug.Log("모든 스크린샷 촬영 완료");
    }

    void LoadMaterials()
    {
        bearMaterials = Resources.LoadAll<Material>("Skin/Bear");
        bunnyMaterials = Resources.LoadAll<Material>("Skin/Bunny");
        faces = Resources.LoadAll<Material>("Skin/Face");

        Debug.Log($"Bear Materials Loaded: {bearMaterials.Length}");
        Debug.Log($"Bunny Materials Loaded: {bunnyMaterials.Length}");
        Debug.Log($"Face Materials Loaded: {faces.Length}");
    }

    void LoadPrefabs()
    {
        Character = Resources.LoadAll<GameObject>("Skin/Character");
        BackPack = Resources.LoadAll<GameObject>("Skin/BackPack");
        CrossBag = Resources.LoadAll<GameObject>("Skin/CrossBag");
        Glasses = Resources.LoadAll<GameObject>("Skin/Glasses");
        Hat = Resources.LoadAll<GameObject>("Skin/Hat");
        Mustache = Resources.LoadAll<GameObject>("Skin/Mustache");
        Scarf = Resources.LoadAll<GameObject>("Skin/Scarf");

        Debug.Log($"Character Prefabs Loaded: {Character.Length}");
        Debug.Log($"BackPack Prefabs Loaded: {BackPack.Length}");
        Debug.Log($"CrossBag Prefabs Loaded: {CrossBag.Length}");
        Debug.Log($"Glasses Prefabs Loaded: {Glasses.Length}");
        Debug.Log($"Hat Prefabs Loaded: {Hat.Length}");
        Debug.Log($"Mustache Prefabs Loaded: {Mustache.Length}");
        Debug.Log($"Scarf Prefabs Loaded: {Scarf.Length}");
    }

    IEnumerator StartCapture()
    {

/*        Instantiate(Character[0], GameObject.Find("Character").transform);
        List<System.Array> categories = new List<System.Array>{
            bearMaterials,
            faces,
            AddNoneOption(BackPack),
            // AddNoneOption(CrossBag),
            // AddNoneOption(Scarf),
            AddNoneOption(Glasses),
            AddNoneOption(Hat),
            // AddNoneOption(Mustache),
        };
        yield return StartCoroutine(captureCombinations(0, categories));
        Destroy(GameObject.Find("Bear"));*/

        Instantiate(Character[1], GameObject.Find("Character").transform);
        List<System.Array>  categories = new List<System.Array>{
            bunnyMaterials,
            faces,
            AddNoneOption(BackPack),
            // AddNoneOption(CrossBag),
            // AddNoneOption(Scarf),
            AddNoneOption(Glasses),
            AddNoneOption(Hat),
            // AddNoneOption(Mustache),
        };
        yield return StartCoroutine(captureCombinations(0, categories));
        Destroy(GameObject.Find("Bunny"));
    }

    System.Array AddNoneOption(System.Array originalArray)
    {
        // 기존 배열에 null을 추가하여 새로운 배열을 만듦
        List<System.Object> extendedList = new List<System.Object>(originalArray.Cast<System.Object>());
        extendedList.Insert(0, null); // 맨 앞에 null 추가 (적용 안 함을 표현)
        return extendedList.ToArray();
    }

    IEnumerator captureCombinations(int index, List<System.Array> categories)
    {

        if (index >= categories.Count)
        {
            BgColorPicker();
            yield return new WaitForEndOfFrame();
            Screenshot();
            yield break;
        }

        var currentCategory = categories[index];
        foreach (var item in currentCategory)
        {
            Boolean isPrefab = false;
            isPrefab = ApplyCombination(item, index);
            yield return captureCombinations(index + 1, categories);
            if (isPrefab) {
                Destroy(instantiatedPrefabs[instantiatedIndex - 1]);
                instantiatedIndex--;
            }
        }
    }

    Boolean ApplyCombination(System.Object item, int index)
    {

        if (index == 0 && item is Material material) {
            ApplyMaterial(material);
            return false;
        }

        if (index == 1 && item is Material face) {
            ApplyFace(face);
            return false;
        }

        if (item is GameObject accessory)
        {
            ApplyAccessory(accessory, index);
            return true;
        }

        return false;
    }

    void ApplyMaterial(Material material)
    {
        Renderer skinRenderer = GameObject.Find($"Skin").GetComponent<Renderer>();
        skinRenderer.material = material;
    }

    void ApplyFace(Material face)
    {
        Renderer faceRenderer = GameObject.Find($"Face01").GetComponent<Renderer>();
        faceRenderer.material = face;
    }

    void ApplyAccessory(GameObject accessory, int index)
    {
        string locatorName = GetLocatorName(index);
        Transform locator = GameObject.Find($"{locatorName}").transform;
        instantiatedPrefabs[instantiatedIndex] = Instantiate(accessory, locator);
        instantiatedIndex++;
    }

    string GetLocatorName(int index) {
        switch (index)
        {
            case 2:
                return "Accessories_locator";
/*            case 3:
                return "Accessories_locator";
            case 4:
                return "Accessories_locator";*/
            case 3:
                return "Head_Accessories_locator";
            case 4:
                return "Head_Accessories_locator";
/*            case 7:
                return "Head_Accessories_locator";*/
            default:
                return "Head_Accessories_locator";
        }
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



    void Screenshot()
    {
        Camera camera = Camera.main;
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
        string filePath = $"{directoryPath}/_RGB{rgb}.png";

        byte[] bytes = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(filePath, bytes);

        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        Debug.Log($"Screenshot saved to: {filePath}");
    }


    //void SetGradeImage()
    //{
    //    Image gradeImage = GameObject.Find("Skin/Grade").GetComponent<Image>();
    //    string gradeFilePath = "";
    //    if (grade[captureIndex] == 1)
    //    {
    //        gradeFilePath = "grade/figma_star1";
    //    }
    //    else if (grade[captureIndex] == 2)
    //    {
    //        gradeFilePath = "grade/figma_star2";
    //    }
    //    else if (grade[captureIndex] == 3)
    //    {
    //        gradeFilePath = "grade/figma_star3";
    //    }

    //    Sprite gradeSprite = Resources.Load<Sprite>(gradeFilePath);
    //    if (gradeSprite != null)
    //    {
    //        gradeImage.sprite = gradeSprite;
    //    }
    //}
}