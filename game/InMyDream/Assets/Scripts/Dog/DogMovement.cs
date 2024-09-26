using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogMovement : MonoBehaviour
{
    public List<Transform> waypoints; // 이동 포인트 리스트
    public Transform eventLocation; // 이벤트 발생 시 이동할 위치
    public float rotationDuration = 1.0f; // 회전 시간
    public float closeEnoughDistance = 2.0f; // 이벤트 위치 근처 도착 판정 거리
    private NavMeshAgent agent;
    private Animator animator;
    private int currentWaypointIndex = 0;
    private bool eventTriggered = false;
    private bool isRotating = false; // 회전 상태를 추적
    private bool isBarking = false; // 짖는 상태 여부

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        MoveToNextWaypoint();
    }

    void Update()
    {
        // 회전 중, 이벤트 중, 혹은 짖는 중일 때는 이동하지 않음
        if (isRotating || eventTriggered || isBarking) return;

        if (!agent.pathPending && agent.remainingDistance <= 0f)
        {
            StartCoroutine(RotateDog()); // 180도 회전
        }
        else
        {
            if (agent.velocity.magnitude > 0.1f)
            {
                animator.SetInteger("moveDirection", 1); // 이동 애니메이션
            }
            else
            {
                animator.SetInteger("moveDirection", 0); // 멈춤 애니메이션
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
        isRotating = true;
        agent.isStopped = true;

        // 현재 위치에서 다음 목적지까지의 방향을 계산
        Vector3 directionToNextWaypoint = (waypoints[currentWaypointIndex].position - transform.position).normalized;

        // 현재 회전 각도와 목표 회전 각도를 계산
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(directionToNextWaypoint);

        // 왼쪽과 오른쪽 중 어느 쪽이 더 가까운지 판단
        float angleDifference = Quaternion.Angle(initialRotation, targetRotation);
        Vector3 crossProduct = Vector3.Cross(transform.forward, directionToNextWaypoint);

        // 각도 차이가 음수면 왼쪽으로 회전, 양수면 오른쪽으로 회전
        if (crossProduct.y > 0)
        {
            animator.SetInteger("moveDirection", 3); // 왼쪽 회전 애니메이션
        }
        else
        {
            animator.SetInteger("moveDirection", 2); // 오른쪽 회전 애니메이션
        }

        float timeElapsed = 0f;
        while (timeElapsed < rotationDuration)
        {
            // Slerp를 사용하여 부드럽게 회전
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, timeElapsed / rotationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // 최종적으로 목표 회전 각도에 도달
        transform.rotation = targetRotation;

        agent.isStopped = false;
        isRotating = false;

        animator.SetInteger("moveDirection", 1); // 다시 이동 애니메이션
        MoveToNextWaypoint();
    }



    // 개가 짖도록 하는 메서드
    public void StartBarking()
    {
        isBarking = true;
        agent.isStopped = true;
        animator.SetInteger("moveDirection", 10); // 짖는 애니메이션
        StartCoroutine(StopBarkingAfterDelay(3.0f)); // 2초 후 짖는 것을 멈춤
    }

    IEnumerator StopBarkingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StopBarking(); // 2초 후 짖는 것을 멈추는 메서드 호출
    }


    public void StopBarking()
    {
        isBarking = false;
        agent.isStopped = false;
        MoveToNextWaypoint();
    }

    public void OnEventTriggered()
    {
        eventTriggered = true;
        agent.destination = eventLocation.position;
        StartCoroutine(CheckProximityToEventLocation());
    }

    IEnumerator CheckProximityToEventLocation()
    {
        while (agent.pathPending || Vector3.Distance(transform.position, eventLocation.position) > closeEnoughDistance)
        {   
            yield return null;
        }

        eventTriggered = false;
        MoveToNextWaypoint();
    }
}
