using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEffect : MonoBehaviour
{
    public GameObject childObject; // 활성화할 자식 오브젝트
    private Rigidbody rb; // 부모 오브젝트의 Rigidbody
    public float stopThreshold = 0.1f; // 속도 0으로 간주할 기준 값 (작은 값으로 설정)

    void Start()
    {
        // Rigidbody 컴포넌트를 가져옴
        rb = GetComponent<Rigidbody>();

        // 자식 오브젝트 비활성화 상태로 시작
        if (childObject != null)
        {
            childObject.SetActive(false);
        }
    }

    void Update()
    {
        // Rigidbody의 속도를 체크
        if (rb.velocity.magnitude < stopThreshold)
        {
            // 속도가 거의 0에 가까우면 자식 오브젝트 활성화
            if (childObject != null && !childObject.activeSelf)
            {
                childObject.SetActive(true);
                Debug.Log("오브젝트가 멈췄습니다. 자식 오브젝트 활성화.");
            }
        }
        else
        {
            // 속도가 다시 생기면 자식 오브젝트 비활성화
            if (childObject != null && childObject.activeSelf)
            {
                childObject.SetActive(false);
                Debug.Log("오브젝트가 움직이고 있습니다. 자식 오브젝트 비활성화.");
            }
        }
    }
}
