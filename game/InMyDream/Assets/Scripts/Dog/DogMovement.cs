using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogMovement : MonoBehaviour
{
    public List<Transform> waypoints; // 이동 포인트를 리스트로 관리
    public Transform eventLocation; // 이벤트 발생 시 이동할 위치
    public float rotationDuration = 1.0f; // 회전에 걸리는 시간 (초)
    public float closeEnoughDistance = 2.0f; // 이벤트 위치 근처로 이동한 것으로 간주할 거리
    private NavMeshAgent agent;
    private Animator animator; // Animator 변수 추가
    private int currentWaypointIndex = 0;
    private bool eventTriggered = false;
    private bool isRotating = false; // 회전 상태를 추적하는 변수

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기

        // Rigidbody가 있을 경우, 물리 효과를 차단하기 위해 isKinematic 활성화
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        MoveToNextWaypoint();
    }

    void Update()
    {
        // 회전 중이거나 이벤트 중일 때는 다음 위치로 이동하지 않음
        if (isRotating || eventTriggered) return;

        // 현재 위치에서 다음 지점까지 도착했는지 확인
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            StartCoroutine(RotateDog()); // 180도 회전
        }
        else
        {
            // 이동 중일 때 moveDirection을 1로 설정하여 직진 애니메이션 재생
            if (agent.velocity.magnitude > 0.1f)
            {
                animator.SetInteger("moveDirection", 1);
            }
            else
            {
                // 멈춰있을 때 moveDirection을 0으로 설정
                animator.SetInteger("moveDirection", 0);
            }
        }
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Count == 0) return;

        agent.destination = waypoints[currentWaypointIndex].position;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
    }

    IEnumerator RotateDog()
    {
        isRotating = true; // 회전 중임을 표시
        agent.isStopped = true; // 이동을 멈춤

        // 회전 애니메이션을 실행 (moveDirection을 2로 설정)
        animator.SetInteger("moveDirection", 2);

        // 현재 회전각도에서 목표 회전각도로의 회전을 설정 (180도)
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0, 180, 0);

        float timeElapsed = 0f;

        // 회전이 rotationDuration 만큼 지속되도록 설정
        while (timeElapsed < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, timeElapsed / rotationDuration);
            timeElapsed += Time.deltaTime;
            yield return null; // 프레임마다 대기
        }

        // 회전 완료 후 정확히 목표 각도로 설정
        transform.rotation = targetRotation;

        // 회전이 끝난 후 이동 재개
        agent.isStopped = false;
        isRotating = false; // 회전이 끝났음을 표시

        // 직진 애니메이션 재생을 위해 moveDirection을 1로 설정
        animator.SetInteger("moveDirection", 1);

        MoveToNextWaypoint();
    }

    // 이벤트가 발생했을 때 호출
    public void OnEventTriggered()
    {
        eventTriggered = true;
        agent.destination = eventLocation.position; // 큐브의 위치로 개를 이동
        StartCoroutine(CheckProximityToEventLocation());
    }

    IEnumerator CheckProximityToEventLocation()
    {
        // 이벤트 위치 근처로 도착할 때까지 대기
        while (agent.pathPending || Vector3.Distance(transform.position, eventLocation.position) > closeEnoughDistance)
        {
            yield return null;
        }

        // 이벤트 위치 근처에 도착하면 다시 원래 이동 경로로 복귀
        eventTriggered = false;
        MoveToNextWaypoint();
    }
}
