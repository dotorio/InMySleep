using UnityEngine;
using UnityEngine.UI;

public class NotiSound : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        audioSource.Play(); // 효과음 재생
    }
}
