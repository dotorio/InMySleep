using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// NavMeshAgent를 사용하여 개 캐릭터의 이동과 행동을 제어합니다.
/// 개는 웨이포인트를 따라 이동하고, 이벤트를 처리하며, 부드럽게 회전하고 짖을 수 있습니다.
/// </summary>
[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class DogMovement : MonoBehaviourPunCallbacks
{
    [Header("이동 설정")]
    [Tooltip("개가 따라갈 웨이포인트 리스트.")]
    public List<Transform> waypoints = new List<Transform>();

    [Tooltip("개가 회전하는 데 걸리는 시간.")]
    public float rotationDuration = 1.5f;

    [Tooltip("이벤트 위치 근처에 도착했다고 판단하는 거리.")]
    public float closeEnoughDistance = 0.5f;

    [Header("짖기 설정")]
    [Tooltip("개가 짖는 시간.")]
    public float barkingDuration = 3.0f;

    private NavMeshAgent agent;
    private Animator animator;

    private int currentWaypointIndex = 0;
    private bool eventTriggered = false;
    private bool isRotating = false;
    private bool isBarking = false;

    // 현재 이벤트 위치 저장
    private Vector3 currentEventPosition;
    private Transform bone;

    // 현재 실행 중인 proximity check 코루틴을 추적
    private Coroutine proximityCoroutine;

    // 애니메이터 파라미터 해시 캐싱 (성능 향상)
    private int moveDirectionHash;

    // 이동 방향을 나타내는 열거형 (가독성 향상)
    private enum MoveDirection
    {
        Idle = 0,         // 정지
        Moving = 1,       // 이동 중
        RotateRight = 2,  // 오른쪽으로 회전
        RotateLeft = 3,   // 왼쪽으로 회전
        Searching = 4,    // 좌우 탐색
        Barking = 10      // 짖기
    }

    private void Awake()
    {
        // 컴포넌트 참조 캐싱
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // 애니메이터 파라미터 해시 초기화
        moveDirectionHash = Animator.StringToHash("moveDirection");
    }

    private void Start()
    {
        // Rigidbody가 있을 경우 물리 간섭을 피하기 위해 키네마틱으로 설정
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // 순찰 시작
        MoveToNextWaypoint();
    }

    private void Update()
    {
        if(bone != null)
        {
            float distance = Vector3.Distance(bone.position, transform.position);

            if(distance <= 3f)
            {
                bone.GetComponent<DogBone>().ResetPosition();
                bone = null;
            }
        }

        // 회전 중이거나 짖는 중일 때는 이동 상태를 업데이트하지 않음
        if (isRotating || isBarking) return;

        // 에이전트가 목적지에 도착했는지 확인
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (eventTriggered)
            {
                // 이벤트 도착 시 추가 로직 (예: 이벤트 완료 후 다시 웨이포인트 순찰 시작)
                //Debug.Log("Dog reached event location.");
                eventTriggered = false;
                MoveToNextWaypoint();
                return;
            }

            // 웨이포인트에 도착하면 다음 웨이포인트을 향해 회전 시작
            StartCoroutine(RotateDog());
        }
        else
        {
            // 이동 상태에 따라 애니메이션 업데이트
            UpdateMovementAnimation();
        }
    }

    /// <summary>
    /// 다음 웨이포인트로 이동을 설정합니다.
    /// </summary>
    private void MoveToNextWaypoint()
    {
        if (waypoints.Count == 0)
        {
            //Debug.LogWarning("DogMovement에 설정된 웨이포인트가 없습니다.");
            return;
        }

        agent.destination = waypoints[currentWaypointIndex].position;
        //Debug.Log($"Dog moving to waypoint {currentWaypointIndex}: {waypoints[currentWaypointIndex].position}");
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
    }

    /// <summary>
    /// 에이전트의 속도에 따라 애니메이션을 업데이트합니다.
    /// </summary>
    private void UpdateMovementAnimation()
    {
        bool isMoving = agent.velocity.sqrMagnitude > 0.01f;
        animator.SetInteger(moveDirectionHash, isMoving ? (int)MoveDirection.Moving : (int)MoveDirection.Idle);
    }

    /// <summary>
    /// 개를 자연스럽게 회전시키는 코루틴입니다.
    /// </summary>
    private IEnumerator RotateDog()
    {
        isRotating = true;
        agent.isStopped = true;

        // 다음 웨이포인트까지의 방향 계산
        Vector3 directionToNextWaypoint = (waypoints[currentWaypointIndex].position - transform.position).normalized;

        // 목표 회전 각도 설정
        Quaternion targetRotation = Quaternion.LookRotation(directionToNextWaypoint);

        // 회전 방향 결정 (왼쪽 또는 오른쪽)
        Vector3 crossProduct = Vector3.Cross(transform.forward, directionToNextWaypoint);
        MoveDirection rotationDirection = crossProduct.y > 0 ? MoveDirection.RotateLeft : MoveDirection.RotateRight;

        animator.SetInteger(moveDirectionHash, (int)rotationDirection);
        //Debug.Log($"Dog starts rotating {(rotationDirection == MoveDirection.RotateLeft ? "left" : "right")}.");

        // 회전 시작
        Quaternion initialRotation = transform.rotation;
        float timeElapsed = 0f;

        while (timeElapsed < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, timeElapsed / rotationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // 최종 회전 각도 설정
        transform.rotation = targetRotation;

        // 이동 애니메이션으로 전환
        animator.SetInteger(moveDirectionHash, (int)MoveDirection.Moving);
        //Debug.Log("Dog completed rotation and resumes moving.");

        agent.isStopped = false;
        isRotating = false;

        // 다음 웨이포인트로 이동 설정
        MoveToNextWaypoint();
    }

    /// <summary>
    /// 개가 짖기 시작합니다.
    /// </summary>
    public void StartBarking()
    {
        if (isBarking) return;

        isBarking = true;
        agent.isStopped = true;
        animator.SetInteger(moveDirectionHash, (int)MoveDirection.Barking);
        Debug.Log("Dog started barking.");
        StartCoroutine(StopBarkingAfterDelay(barkingDuration));
    }

    /// <summary>
    /// 일정 시간 후에 짖는 것을 멈추는 코루틴입니다.
    /// </summary>
    private IEnumerator StopBarkingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StopBarking();
    }

    /// <summary>
    /// 개의 짖기를 멈추고 이동을 재개합니다.
    /// </summary>
    public void StopBarking()
    {
        if (!isBarking) return;

        isBarking = false;
        animator.SetInteger(moveDirectionHash, (int)MoveDirection.Idle);
        agent.isStopped = false;
        Debug.Log("Dog stopped barking.");

        // 현재 이벤트가 활성화된 상태라면 이벤트 위치로 이동을 재개
        if (eventTriggered)
        {
            agent.destination = currentEventPosition;
            Debug.Log($"Dog resumes movement to event position: {currentEventPosition}");
        }
        else
        {
            // 그렇지 않다면 다음 웨이포인트로 이동
            MoveToNextWaypoint();
        }
    }

    /// <summary>
    /// 이벤트가 트리거되면 호출되어 개를 이벤트 위치로 이동시킵니다.
    /// </summary>
    /// <param name="eventPosition">이벤트가 발생한 위치.</param>
    public void OnEventTriggered(Vector3 eventPosition)
    {
        // 새로운 이벤트가 발생하면 기존 이벤트를 무시하고 새로운 이벤트를 처리
        //Debug.Log($"New event triggered at position: {eventPosition}");

        // 현재 진행 중인 proximity check 코루틴이 있다면 중단
        if (proximityCoroutine != null)
        {
            StopCoroutine(proximityCoroutine);
            proximityCoroutine = null;
            //Debug.Log("Stopped previous proximity check coroutine.");
        }

        // 이벤트 상태 업데이트
        eventTriggered = true;
        currentEventPosition = eventPosition; // 새로운 이벤트 위치 저장
        agent.destination = currentEventPosition;
        //Debug.Log($"Dog moving to new event position: {currentEventPosition}");

        // 새로운 proximity check 코루틴 시작
        proximityCoroutine = StartCoroutine(CheckProximityToEventLocation(currentEventPosition));
    }

    /// <summary>
    /// 개가 이벤트 위치에 가까워졌는지 확인하는 코루틴입니다.
    /// </summary>
    /// <param name="eventPosition">이벤트가 발생한 위치.</param>
    private IEnumerator CheckProximityToEventLocation(Vector3 eventPosition)
    {
        while (agent.pathPending || Vector3.Distance(transform.position, eventPosition) > closeEnoughDistance)
        {
            yield return null;
        }

        //Debug.Log("Dog reached event location.");
        eventTriggered = false;
        proximityCoroutine = null; // 코루틴 완료 후 참조 해제
        MoveToNextWaypoint();
    }

    /// <summary>
    /// 에디터에서 웨이포인트과 이벤트 위치를 시각화합니다.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (waypoints != null)
        {
            Gizmos.color = Color.green;
            foreach (var waypoint in waypoints)
            {
                if (waypoint != null)
                {
                    Gizmos.DrawSphere(waypoint.position, 0.3f);
                }
            }
        }
    }

    public void addPlayer(int playerPhotonID)
    {
        photonView.RPC("addPlayerRPC", RpcTarget.AllBuffered, playerPhotonID);
    }

    [PunRPC]
    public void addPlayerRPC(int playerPhotonID)
    {
        SpotlightPlayerDetector spotlightPlayerDetector = GetComponentInChildren<SpotlightPlayerDetector>();
        PhotonView playerPhotonView = PhotonView.Find(playerPhotonID);

        spotlightPlayerDetector.addPlayer(playerPhotonView.gameObject);
    }

    // 개 유인 이벤트
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Grabable"))
        {
            bone = other.transform;
            OnEventTriggered(other.transform.position);
        }
    }
}
