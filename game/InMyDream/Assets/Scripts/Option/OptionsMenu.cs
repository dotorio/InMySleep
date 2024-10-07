using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public Slider volumeSlider; // 볼륨 슬라이더
    public AudioMixer audioMixer; // 오디오 믹서

    private const string VolumeKey = "MasterVolume"; // PlayerPrefs에 사용할 키
    private const float minVolume = -40f; // 믹서에 적용할 최소 볼륨 (음소거 직전 볼륨)
    private const float maxVolume = 0f;   // 믹서에 적용할 최대 볼륨
    private const float muteVolume = -80f; // 완전 음소거 상태 (-80dB)

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
            volumeSlider.value = ConvertToLinear(savedVolume);
            SetVolume();
            Debug.Log("Saved volume loaded: " + savedVolume);
        }
        else
        {
            // 저장된 값이 없으면 현재 오디오 믹서의 볼륨 값을 슬라이더에 적용
            if (volumeSlider != null && audioMixer != null)
            {
                float currentVolume;
                audioMixer.GetFloat("Master", out currentVolume);
                volumeSlider.value = ConvertToLinear(currentVolume);
                Debug.Log("Default volume loaded: " + currentVolume);
            }
        }

        // 볼륨 슬라이더의 값이 변경될 때마다 SetVolume 함수를 호출하도록 리스너 추가
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(delegate { SetVolume(); });
        }
    }

    // 볼륨 변경 함수
    public void SetVolume()
    {
        if (audioMixer != null && volumeSlider != null)
        {
            float sliderValue = volumeSlider.value;
            float volume = ConvertToLogarithmic(sliderValue); // 로그 변환된 값 사용
            Debug.Log("SetVolume: " + volume);
            audioMixer.SetFloat("Master", volume);

            // 볼륨 값을 PlayerPrefs에 저장 (로그 변환된 값 저장)
            PlayerPrefs.SetFloat(VolumeKey, volume);
            PlayerPrefs.Save(); // 즉시 저장
            Debug.Log("Volume saved: " + volume);
        }
    }

    // 슬라이더 값(0~1 범위)을 로그 스케일로 변환하여 AudioMixer에 전달할 값으로 변환
    private float ConvertToLogarithmic(float linearValue)
    {
        if (linearValue <= 0.0001f)
        {
            // 슬라이더 값이 0에 가까우면 음소거 (-80dB)
            return muteVolume;
        }
        else
        {
            // 0이 아닌 경우는 minVolume ~ maxVolume 사이에서 로그 변환 적용
            return Mathf.Lerp(minVolume, maxVolume, linearValue);
        }
    }

    // AudioMixer에 저장된 값을 슬라이더의 선형 값(0~1 범위)으로 변환
    private float ConvertToLinear(float mixerValue)
    {
        // minVolume과 maxVolume 사이의 값을 슬라이더 0~1로 변환
        if (mixerValue <= muteVolume)
        {
            return 0f; // 음소거된 상태면 슬라이더는 0으로
        }
        return Mathf.InverseLerp(minVolume, maxVolume, mixerValue);
    }

    // 옵션 메뉴가 비활성화될 때 PlayerPrefs를 저장
    void OnDisable()
    {
        PlayerPrefs.Save();
    }
}
