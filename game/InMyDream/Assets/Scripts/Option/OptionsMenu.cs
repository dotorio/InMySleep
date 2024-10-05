using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public Slider volumeSlider; // 볼륨 슬라이더
    public AudioMixer audioMixer; // 오디오 믹서

    public Button closeButton;

    private const string VolumeKey = "MasterVolume"; // PlayerPrefs에 사용할 키

    void Start()
    {
        // 자식 Canvas의 Sorting Order를 조정하여 다른 UI보다 앞에 오게 설정
        Canvas childCanvas = GetComponentInChildren<Canvas>();
        if (childCanvas != null)
        {
            childCanvas.sortingOrder = 10;
        }

        // PlayerPrefs에 저장된 볼륨 값이 있는지 확인
        if (PlayerPrefs.HasKey(VolumeKey))
        {
            // 저장된 볼륨 값을 불러와 슬라이더와 오디오 믹서에 적용
            float savedVolume = PlayerPrefs.GetFloat(VolumeKey);
            volumeSlider.value = savedVolume;
            audioMixer.SetFloat("Master", savedVolume);
            Debug.Log("Saved volume loaded: " + savedVolume);
        }
        else
        {
            // 저장된 값이 없으면 현재 오디오 믹서의 볼륨 값을 슬라이더에 적용
            if (volumeSlider != null && audioMixer != null)
            {
                float currentVolume;
                audioMixer.GetFloat("Master", out currentVolume);
                volumeSlider.value = currentVolume;
                Debug.Log("Default volume loaded: " + currentVolume);
            }
        }

        // 볼륨 슬라이더의 값이 변경될 때마다 SetVolume 함수를 호출하도록 리스너 추가
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(delegate { SetVolume(); });
        }

        // 닫기 버튼에 대한 리스너 설정
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseOptionsMenu);
        }
    }

    // 볼륨 변경 함수
    public void SetVolume()
    {
        if (audioMixer != null && volumeSlider != null)
        {
            float volume = volumeSlider.value;
            Debug.Log("SetVolume: " + volume);
            audioMixer.SetFloat("Master", volume);

            // 볼륨 값을 PlayerPrefs에 저장
            PlayerPrefs.SetFloat(VolumeKey, volume);
            PlayerPrefs.Save(); // 즉시 저장
            Debug.Log("Volume saved: " + volume);
        }
    }

    // 옵션 메뉴 닫기 함수
    private void CloseOptionsMenu()
    {
        gameObject.SetActive(false);
    }

    // 옵션 메뉴가 비활성화될 때 PlayerPrefs를 저장
    void OnDisable()
    {
        PlayerPrefs.Save();
    }
}
