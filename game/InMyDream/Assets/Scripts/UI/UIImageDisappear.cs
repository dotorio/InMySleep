using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIImageDisappear : MonoBehaviour
{
    public GameObject imageObject; // 사라질 이미지 오브젝트
    public float disappearTime = 3f; // 이미지가 사라질 시간 (3초)

    void Start()
    {
        // 3초 후에 이미지를 비활성화하는 코루틴 실행
        StartCoroutine(HideImageAfterDelay(disappearTime));
    }

    IEnumerator HideImageAfterDelay(float delay)
    {
        // 지정된 시간만큼 대기
        yield return new WaitForSeconds(delay);

        // 이미지 오브젝트 비활성화
        imageObject.SetActive(false);
    }
}
