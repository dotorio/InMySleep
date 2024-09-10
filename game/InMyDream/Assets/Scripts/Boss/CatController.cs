using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    //public float speed = 2.0f;  // 고양이의 이동 속도
    //private bool isMovingForward = true;  // 앞으로 이동 중인지 확인하는 변수
    //private float moveTime = 1.0f;  // 1초 동안 이동
    //private float timer = 0.0f;  // 시간 측정용 타이머
    private Animator animator;  // 애니메이터
    public GameObject bomb; // 캐릭터 팔에 연결된 공
    public GameObject cube; // 캐릭터 팔에 연결된 공
    public Transform hand;  // 캐릭터의 손 위치
    public Transform hand2;  // 캐릭터의 손 위치
    public GameObject player1;  // 플레이어 참조
    public GameObject player2;  // 플레이어 참조
    private float projectileCooldown = 1f;  // 원거리 공격 쿨타임
    private float lastProjectileTime = 0f;  // 마지막 원거리 공격 시간
    private float cnt = 0;
    private float cnt2 = 0;

    // Start는 첫 프레임 업데이트 전에 호출됩니다.
    void Start()
    {
        // Animator 컴포넌트 가져오기
        animator = GetComponent<Animator>();
        //animator.SetBool("Move", true);
        Debug.Log("시작");
    }

    // Update는 매 프레임마다 호출됩니다.
    void Update()
    {
        // 타이머 업데이트
        //timer += Time.deltaTime;

        //// 1초가 지났을 때 방향을 바꿈
        //if (timer >= moveTime)
        //{
        //    isMovingForward = !isMovingForward;  // 방향 반전
        //    timer = 0.0f;  // 타이머 리셋
        //}

        ////고양이의 이동 처리
        //if (isMovingForward)
        //{
        //    transform.position += (new Vector3(5, 0, 0) * speed * Time.deltaTime);  // 왼쪽으로 이동

        //}
        //else
        //{
        //    transform.position += (new Vector3(-5, 0, 0) * speed * Time.deltaTime);  // 오른쪽으로 이동
        //}
        //if (Time.time - lastProjectileTime >= projectileCooldown)
        //{
        //    lastProjectileTime = Time.time;

        //    Debug.Log("보스가 포물선 원거리 공격을 시도합니다!");
        //    BallThrow();
        //}
        
        // 애니메이션 트리거 설정
    }

    void BallThrow()
    {
        //animator.SetBool("isATK", true);

        // 발사체 생성
        GameObject projectile = Instantiate(bomb, hand.position, hand.rotation);
        BombController bombController = projectile.GetComponent<BombController>();
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // 발사 방향 계산
        if (cnt % 2 == 0)
        {
            Vector3 launchDirection = CalculateLaunchDirection(hand.position, player2.transform.position, 5f);
            rb.velocity = launchDirection;
        }
        else
        {
            Vector3 launchDirection = CalculateLaunchDirection(hand.position, player1.transform.position, 10f);
            rb.velocity = launchDirection;
        }

        cnt++;

        // 4초 후에 발사체를 제거하고 폭발 효과를 해당 위치에 생성
        bombController.StartDestroyCountdown(4f);
    }

    void CubeThrow()
    {
        //animator.SetBool("isATK", true);

        // 발사체 생성
        GameObject projectile = Instantiate(cube, hand2.position, hand2.rotation);
        //BombController bombController = projectile.GetComponent<BombController>();
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // 발사 방향 계산
        if (cnt2 % 2 == 1)
        {
            Vector3 launchDirection = CalculateLaunchDirection(hand.position, player2.transform.position, 5f);
            rb.velocity = launchDirection;
        }
        else
        {
            Vector3 launchDirection = CalculateLaunchDirection(hand.position, player1.transform.position, 10f);
            rb.velocity = launchDirection;
        }

        cnt2++;

        // 4초 후에 발사체를 제거하고 폭발 효과를 해당 위치에 생성
        //bombController.StartDestroyCountdown(4f);
    }



    Vector3 CalculateLaunchDirection(Vector3 player, Vector3 target, float initialAngle)
    {
        float gravity = Physics.gravity.magnitude;
        float angle = initialAngle * Mathf.Deg2Rad;

        Vector3 planarTarget = new Vector3(target.x, 0, target.z);
        Vector3 planarPosition = new Vector3(player.x, 0, player.z);

        float distance = Vector3.Distance(planarTarget, planarPosition);
        float yOffset = player.y - target.y;

        float initialVelocity
            = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity
            = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        float angleBetweenObjects
            = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (target.x > player.x ? 1 : -1);
        Vector3 finalVelocity
            = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity * 0.8f;

        return finalVelocity;
    }
}
