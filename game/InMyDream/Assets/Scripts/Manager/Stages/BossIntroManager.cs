using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // UI 관련 클래스 사용

public class BossIntroManager : MonoBehaviour
{
    public GameObject objectToActivate; // 3초 후에 활성화할 오브젝트
    public float delay = 3f; // 대기 시간 (기본값: 3초)

    void Start()
    {
        // 오브젝트 활성화를 위한 코루틴 실행
        StartCoroutine(ActivateObjectAfterDelayRoutine());
    }

    IEnumerator ActivateObjectAfterDelayRoutine()
    {
        // 설정된 시간(기본값 3초) 동안 대기
        yield return new WaitForSeconds(delay);

        // 오브젝트 활성화
        objectToActivate.SetActive(true);
    }
}
