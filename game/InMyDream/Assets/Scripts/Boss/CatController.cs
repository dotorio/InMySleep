using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    public GameObject big; // 캐릭터 팔에 연결된 공
    public Transform hand;  // 캐릭터의 손 위치
    public Transform hand2;  // 캐릭터의 손 위치
    public Transform hand3;  // 캐릭터의 손 위치
    public GameObject player1;  // 플레이어 참조
    public GameObject player2;  // 플레이어 참조
    private float projectileCooldown = 1f;  // 원거리 공격 쿨타임
    private float lastProjectileTime = 0f;  // 마지막 원거리 공격 시간
    private float cnt = 0f;
    public float phase = 1f;
    private int randomNumber; // 랜덤 숫자
    private bool isDying = false; // Die1 애니메이션 실행 중 여부
    private Rigidbody rb2;
    private int damage = 0;
    private float originalSpeed;


    // Start는 첫 프레임 업데이트 전에 호출됩니다.
    void Start()
    {
        // Animator 컴포넌트 가져오기
        animator = GetComponent<Animator>();
        originalSpeed = animator.speed;
        Debug.Log(originalSpeed);
        rb2 = GetComponent<Rigidbody>();
        StartCoroutine(PlayRandomAnimation());
        //animator.SetBool("Move", true);
        Debug.Log("시작a");
    }

    private IEnumerator PlayRandomAnimation()
    {
        while (true) // 무한 루프
        {
            // 1~4 사이의 랜덤 숫자 생성 (1 포함, 5는 미포함)
            randomNumber = Random.Range(1, 5);
            Debug.Log("랜덤 숫자: " + randomNumber);

            // 랜덤 숫자에 따라 애니메이션 재생
            switch (randomNumber)
            {
                case 1:
                    Debug.Log("1번");
                    animator.Play("Atk1"); // 첫 번째 애니메이션
                    break;
                case 2:
                    Debug.Log("2번");
                    animator.Play("Atk2"); // 두 번째 애니메이션
                    break;
                case 3:
                    Debug.Log("3번");
                    animator.Play("Atk3"); // 세 번째 애니메이션
                    break;
                case 4:
                    Debug.Log("4번");
                    animator.Play("Victory"); // 네 번째 애니메이션
                    break;
            }

            // 현재 재생 중인 애니메이션이 끝날 때까지 대기
            yield return new WaitUntil(() => IsAnimationFinished(randomNumber));
        }
    }

    private void SetAnimationSpeed()
    {
        switch (phase)
        {
            case 1: // 페이즈 1
                animator.speed = originalSpeed; // 원래 속도
                Debug.Log(originalSpeed);
                break;
            case 2: // 페이즈 2
                animator.speed = originalSpeed * 2f; // 속도를 1.3배로 증가
                Debug.Log(originalSpeed);
                break;
            case 3: // 페이즈 3
                animator.speed = originalSpeed * 3f; // 속도를 1.6배로 증가
                Debug.Log(originalSpeed);
                break;
            default:
                animator.speed = originalSpeed; // 기본 속도로 설정
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("만나긴 했음");

        if (other.CompareTag("Shelf") && !isDying)
        {
            Debug.Log("Shelf와 충돌함");
            animator.Play("Die1");
            isDying = true;

            BoxCollider shelfCollider = other.GetComponent<BoxCollider>();
            if (shelfCollider != null)
            {
                shelfCollider.center = new Vector3(0, 0, 0);
                Debug.Log("shelf의 콜라이더 센터 변경됨");
            }
            StartCoroutine(WaitForDieAnimationToEnd());
        }

        if (other.CompareTag("Stone") && !isDying) // isDying이 false일 때만 Damage 애니메이션 재생
        {
            Debug.Log("Stone과 충돌함");
            damage++;
            if (damage == 3)
            {
                animator.Play("Die2");
                //yield return new WaitUntil(() => IsAnimationFinished(7)); // '5'로 설정하여 Die1 애니메이션 확인

                Debug.Log("Die2 애니메이션 종료");
                isDying = false; // 애니메이션 실행 상태 초기화
            }
            else
            {
                animator.Play("Damage");

                isDying = true; // Damage 애니메이션 실행 상태로 변경
                StartCoroutine(WaitForDamageAnimationToEnd());
            }
        }
    }

    private IEnumerator WaitForDamageAnimationToEnd()
    {
        // Damage 애니메이션이 끝날 때까지 대기
        yield return new WaitUntil(() => IsAnimationFinished(6));

        Debug.Log("Damage 애니메이션 종료");
        isDying = false; // Damage 애니메이션이 끝나면 다시 초기화
        StartCoroutine(PlayRandomAnimation());
        // Damage 애니메이션이 끝난 후, PlayRandomAnimation 루프 재시작
        
    }



    private IEnumerator WaitForDieAnimationToEnd()
    {
        // Die1 애니메이션이 끝날 때까지 대기
        yield return new WaitUntil(() => IsAnimationFinished(5)); // '5'로 설정하여 Die1 애니메이션 확인

        Debug.Log("Die1 애니메이션 종료");
        isDying = false; // 애니메이션 실행 상태 초기화
    }

    private bool IsAnimationFinished(int animationIndex)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        switch (animationIndex)
        {
            case 1:
                return stateInfo.IsName("Atk1") && stateInfo.normalizedTime >= 1f;
            case 2:
                return stateInfo.IsName("Atk2") && stateInfo.normalizedTime >= 1f;
            case 3:
                return stateInfo.IsName("Atk3") && stateInfo.normalizedTime >= 1f;
            case 4:
                return stateInfo.IsName("Victory") && stateInfo.normalizedTime >= 1f;
            case 5:
                return stateInfo.IsName("Die1") && stateInfo.normalizedTime >= 1f;
            case 6:
                return stateInfo.IsName("Damage") && stateInfo.normalizedTime >= 1f;
            case 7:
                return stateInfo.IsName("Die2") && stateInfo.normalizedTime >= 1f;
            default:
                return false;
        }
    }



    //private void StopCharacterMovement()
    //{
    //    // 캐릭터의 움직임을 멈추는 로직
    //    rb2.velocity = Vector3.zero; // Rigidbody의 속도를 0으로 설정하여 캐릭터가 움직이지 않도록 함
    //}


    void BigThrow()
    {
        //animator.SetBool("isATK", true);

        // 발사체 생성
        GameObject projectile = Instantiate(bomb, hand3.position, hand3.rotation);
        BombController bombController = projectile.GetComponent<BombController>();
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // 발사 방향 계산
        if (cnt % 2 == 0)
        {
            Vector3 launchDirection = CalculateLaunchDirection(hand3.position, player2.transform.position, 20f);
            rb.velocity = launchDirection;
        }
        else
        {
            Vector3 launchDirection = CalculateLaunchDirection(hand3.position, player1.transform.position, 20f);
            rb.velocity = launchDirection;
        }

        cnt++;

        // 4초 후에 발사체를 제거하고 폭발 효과를 해당 위치에 생성
        bombController.StartDestroyCountdown(4f);
    }

    void CubeThrow()
    {
        // 발사체 생성
        GameObject projectile = Instantiate(cube, hand2.position, hand2.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // 발사 방향 계산
        if (cnt % 2 == 0)
        {
            Vector3 launchDirection = CalculateLaunchDirection(hand2.position, player2.transform.position, 20f);
            rb.velocity = launchDirection;
        }
        else
        {
            Vector3 launchDirection = CalculateLaunchDirection(hand2.position, player1.transform.position, 20f);
            rb.velocity = launchDirection;
        }

        cnt++;

        StartCoroutine(FadeOutAndDestroy(projectile, 5f, 1f)); // 코루틴 시작
    }

    void BallThrow()
    {
        // 발사체 생성
        GameObject projectile = Instantiate(big, hand.position, hand.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // 발사 방향 계산
        if (cnt % 2 == 0)
        {
            Vector3 launchDirection = CalculateLaunchDirection(hand.position, player2.transform.position, 20f);
            rb.velocity = launchDirection;
        }
        else
        {
            Vector3 launchDirection = CalculateLaunchDirection(hand.position, player1.transform.position, 20f);
            rb.velocity = launchDirection;
        }

        cnt++;

        StartCoroutine(FadeOutAndDestroy(projectile, 5f, 1f)); // 코루틴 시작
    }

    private IEnumerator FadeOutAndDestroy(GameObject projectile, float delay, float fadeDuration)
    {
        // 5초 대기
        yield return new WaitForSeconds(delay);

        Renderer renderer = projectile.GetComponent<Renderer>();
        Material material = renderer.material;
        Color initialColor = material.color;

        // Alpha 값을 서서히 줄이기
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            material.color = new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.Lerp(initialColor.a, 0f, normalizedTime));
            yield return null; // 다음 프레임까지 대기
        }

        // 완전히 사라진 후 삭제
        Destroy(projectile);
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
            = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity * 0.9f;

        return finalVelocity;
    }

    void Lie()
    {
        // 애니메이션이나 다른 행동을 정의
        Debug.Log("눕는다는 함수 실행");
        animator.SetBool("isDie", true);  // isDie 파라미터를 true로 설정
    }

}
