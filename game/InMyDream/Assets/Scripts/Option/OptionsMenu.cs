using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public Slider volumeSlider; // 볼륨 슬라이더
    public AudioMixer audioMixer; // 오디오 믹서
    public Button exitButton; // 게임 종료 버튼

    public Button closeButton;

    void Start()
    {
        // 볼륨 슬라이더 초기값 설정
        if (volumeSlider != null && audioMixer != null)
        {
            float currentVolume;
            audioMixer.GetFloat("Master", out currentVolume);
            volumeSlider.value = currentVolume;
        }

        // Exit 버튼에 함수 연결
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(QuitGame);
        }
    }

    // 볼륨 변경 함수
    public void SetVolume()
    {
        Debug.Log("SetVolume" + volumeSlider.value);
        audioMixer.SetFloat("Master", volumeSlider.value);
    }

    // 게임 종료 함수
    public void QuitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 실행 중일 때
#else
        Application.Quit(); // 빌드된 게임에서
#endif
    }
}
