using UnityEngine;

public class FixedCameraController : MonoBehaviour
{
    public Transform target; // 따라갈 대상 (캐릭터)
    public Vector3 offset; // 캐릭터와의 고정된 위치

    void LateUpdate()
    {
        // 고정된 오프셋을 유지하며 캐릭터를 따라 이동
        transform.position = target.position + offset;
        transform.LookAt(target); // 항상 캐릭터를 바라보게 설정
    }
}
