using UnityEngine;

public class DogVision : MonoBehaviour
{
    [Header("Vision Settings")]
    public float viewDistance = 15f;        // 시야 거리
    public float viewAngle = 90f;           // 시야 각도 (원뿔의 각도)
    public float viewAngleDown = 45f;       // 시야의 아래 각도
    public LayerMask targetMask;            // 감지할 대상의 레이어 (예: 플레이어)
    public LayerMask obstacleMask;          // 시야를 가로막는 장애물의 레이어

    [Header("Detection Settings")]
    public string playerTag = "Player";     // 감지할 대상의 태그

    void Update()
    {
        DetectTargets();
    }

    /// <summary>
    /// 시야 내의 타겟을 감지하는 함수
    /// </summary>
    void DetectTargets()
    {
        // 시야 범위 내의 모든 타겟을 찾음
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        foreach (Collider target in targetsInViewRadius)
        {
            // 대상이 플레이어인지 확인
            if (!target.CompareTag(playerTag))
                continue;

            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

            // 시야 각도를 고려하여 타겟이 시야 내에 있는지 확인
            float angle = Vector3.Angle(TransformDirectionWithDown(), directionToTarget);
            if (angle < viewAngle / 2f)
            {
                // Raycast를 사용하여 장애물이 없는지 확인
                if (!Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, viewDistance, obstacleMask))
                {
                    // 플레이어가 시야 내에 있음
                    Debug.Log("플레이어 발견!");
                    TriggerEvent(target);
                }
            }
        }
    }

    /// <summary>
    /// 시야의 방향을 설정 (45도 아래로 향하도록)
    /// </summary>
    /// <returns>시야의 방향 벡터</returns>
    Vector3 TransformDirectionWithDown()
    {
        // 기본 forward 방향을 45도 아래로 회전
        return Quaternion.Euler(-viewAngleDown, 0, 0) * transform.forward;
    }

    /// <summary>
    /// 플레이어를 발견했을 때 발생할 이벤트를 처리하는 함수
    /// </summary>
    /// <param name="target">감지된 타겟의 콜라이더</param>
    void TriggerEvent(Collider target)
    {
        // 원하는 이벤트를 처리하는 부분 (디버그 메시지 출력)
        Debug.Log("이벤트 발생: " + target.name);
    }

    /// <summary>
    /// 에디터에서 시야를 시각화하기 위한 함수
    /// </summary>
    void OnDrawGizmos()
    {
        // 시야 범위 원 그리기
        Gizmos.color = new Color(1, 1, 0, 0.2f); // 노란색, 투명도 20%
        Gizmos.DrawSphere(transform.position, viewDistance);

        // 시야 각도에 맞는 원뿔 그리기
        Gizmos.color = Color.yellow;
        Vector3 forwardDirection = TransformDirectionWithDown();
        Gizmos.DrawRay(transform.position, forwardDirection * viewDistance);

        // 시야의 왼쪽과 오른쪽 경계선 그리기
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2f, 0) * forwardDirection;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2f, 0) * forwardDirection;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, leftBoundary * viewDistance);
        Gizmos.DrawRay(transform.position, rightBoundary * viewDistance);
    }
}
