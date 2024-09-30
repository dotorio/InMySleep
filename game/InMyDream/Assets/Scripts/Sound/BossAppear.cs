using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAppear : MonoBehaviour
{
    public AudioSource audioSource; // 오디오 소스
    public float stopTime = 3f; // 오디오가 꺼질 시간 (3초)

    void Start()
    {
        // 3초 후에 오디오를 중지하는 코루틴 실행
        StartCoroutine(StopAudioAfterDelay(stopTime));
    }

    IEnumerator StopAudioAfterDelay(float delay)
    {
        // 지정된 시간만큼 대기
        yield return new WaitForSeconds(delay);

        // 오디오 중지
        audioSource.Stop();
    }
}
