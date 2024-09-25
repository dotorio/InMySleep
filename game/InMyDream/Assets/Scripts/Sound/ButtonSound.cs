using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    private AudioSource audioSource;
    private Button button; // 버튼을 연결할 변수

    void Start()
    {
        button = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();
        button.onClick.AddListener(PlaySound); // 버튼 클릭 시 소리 재생
    }

    void PlaySound()
    {
        audioSource.Play(); // 효과음 재생
    }
}
