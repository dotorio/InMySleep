using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CatController : MonoBehaviour
{
    private Animator animator;  // 애니메이터
    public GameObject bomb; // 캐릭터 팔에 연결된 공
    public GameObject bigBomb; // 캐릭터 팔에 연결된 공
    public GameObject ball; // 캐릭터 팔에 연결된 공
    public GameObject redBomb; // 캐릭터 팔에 연결된 공
    public Transform hand;  // 캐릭터의 손 위치
    public Transform hand2;  // 캐릭터의 손 위치
    public Transform hand3;  // 캐릭터의 손 위치
    public GameObject player1;  // 플레이어 참조
    public GameObject player2;  // 플레이어 참조
    private float cnt = 0f;
    public int phase = 1;
    private int randomNumber; // 랜덤 숫자
    private bool isDying = false; // Die1 애니메이션 실행 중 여부
    private int damage = 0;
    private float originalSpeed;

    // Start는 첫 프레임 업데이트 전에 호출됩니다.
    void Start()
    {
        // Animator 컴포넌트 가져오기
        animator = GetComponent<Animator>();
        originalSpeed = animator.speed;
        StartCoroutine(PlayRandomAnimation());
        SetAnimationSpeed();
    }

    private IEnumerator PlayRandomAnimation()
    {
        while (true) // 무한 루프
        {
            if (phase == 1)
            {
                randomNumber = Random.Range(1, 11);
                switch (randomNumber)
                {
                    case 1:
                    case 2:
                        animator.Play("Victory");
                        break;
                    case 3:
                    case 4:
                        animator.Play("rightBomb");
                        break;
                    case 5:
                        animator.Play("rightBall");
                        break;
                    case 6:
                    case 7:
                        animator.Play("leftBomb"); 
                        break;
                    case 8:
                        animator.Play("leftBall");
                        break;
                    case 9:
                    case 10:
                        animator.Play("BigBomb");
                        break;
                }
            }
            else if (phase == 2)
            {
                randomNumber = Random.Range(11, 21);
                switch (randomNumber)
                {
                    case 11:
                    case 12:
                        animator.Play("Victory");
                        break;
                    case 13:
                    case 14:
                        animator.Play("rightBomb");
                        break;
                    case 15:
                    case 16:
                        animator.Play("rightBall");
                        break;
                    case 17:
                    case 18:
                        animator.Play("leftBomb");
                        break;
                    case 19:
                    case 20:
                        animator.Play("leftBall");
                        break;
                }
            }
            else
            {
                randomNumber = Random.Range(21, 31);
                switch (randomNumber)
                {
                    case 21:
                    case 22:
                        animator.Play("Victory");
                        break;
                    case 23:
                    case 24:
                        animator.Play("rightBomb");
                        break;
                    case 25:
                        animator.Play("rightRed");
                        break;
                    case 26:
                    case 27:
                        animator.Play("leftBomb");
                        break;
                    case 28:
                        animator.Play("leftRed");
                        break;
                    case 29:
                    case 30:
                        animator.Play("BigBomb");
                        break;
                }
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
                break;
            case 2: // 페이즈 2
                animator.speed = originalSpeed * 1.3f; // 속도를 1.3배로 증가
                break;
            case 3: // 페이즈 3
                animator.speed = originalSpeed * 1.6f; // 속도를 1.6배로 증가
                break;
            default:
                animator.speed = originalSpeed; // 기본 속도로 설정
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Shelf") && !isDying)
        {
            animator.Play("Die1");
            isDying = true;

            BoxCollider shelfCollider = other.GetComponent<BoxCollider>();
            if (shelfCollider != null)
            {
                shelfCollider.center = new Vector3(0, 0, 0);
            }
            StartCoroutine(WaitForDieAnimationToEnd());
        }

        if (other.CompareTag("Stone") && !isDying) // isDying이 false일 때만 Damage 애니메이션 재생
        {
            damage++;
            if (damage == 3)
            {
                animator.Play("Die2");
                //yield return new WaitUntil(() => IsAnimationFinished(7)); // '5'로 설정하여 Die1 애니메이션 확인
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
        yield return new WaitUntil(() => IsAnimationFinished(31));

        Debug.Log("Damage 애니메이션 종료");
        isDying = false; // Damage 애니메이션이 끝나면 다시 초기화
        StartCoroutine(PlayRandomAnimation());
        // Damage 애니메이션이 끝난 후, PlayRandomAnimation 루프 재시작
        
    }



    private IEnumerator WaitForDieAnimationToEnd()
    {
        // Die1 애니메이션이 끝날 때까지 대기
        yield return new WaitUntil(() => IsAnimationFinished(32)); // '5'로 설정하여 Die1 애니메이션 확인

        Debug.Log("Die1 애니메이션 종료");
        isDying = false; // 애니메이션 실행 상태 초기화
    }

    private bool IsAnimationFinished(int animationIndex)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        switch (animationIndex)
        {
            case 1:
            case 2:
            case 11:
            case 12:
            case 21:
            case 22:
                return stateInfo.IsName("Victory") && stateInfo.normalizedTime >= 1f;
            case 3:
            case 4:
            case 13:
            case 14:
            case 23:
            case 24:
                return stateInfo.IsName("rightBomb") && stateInfo.normalizedTime >= 1f;
            case 5:
            case 15:
            case 16:
                return stateInfo.IsName("rightBall") && stateInfo.normalizedTime >= 1f;
            case 6:
            case 7:
            case 17:
            case 18:
            case 26:
            case 27:
                return stateInfo.IsName("leftBomb") && stateInfo.normalizedTime >= 1f;
            case 8:
            case 19:
            case 20:
                return stateInfo.IsName("leftBall") && stateInfo.normalizedTime >= 1f;
            case 9:
            case 10:
            case 29:
            case 30:
                return stateInfo.IsName("BigBomb") && stateInfo.normalizedTime >= 1f;
            case 25:
                return stateInfo.IsName("rightRed") && stateInfo.normalizedTime >= 1f;
            case 28:
                return stateInfo.IsName("leftRed") && stateInfo.normalizedTime >= 1f;
            case 31:
                return stateInfo.IsName("Damage") && stateInfo.normalizedTime >= 1f;
            case 32:
                return stateInfo.IsName("Die1") && stateInfo.normalizedTime >= 1f;
            default:
                return false;
        }
    }


    void BombThrow()
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

    void RedThrow()
    {
        // 발사체 생성
        GameObject projectile = Instantiate(cube, hand2.position, hand2.rotation);
        BombController bombController = projectile.GetComponent<BombController>();
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        //발사 방향 계산
        if (cnt % 2 == 0)
        {
            Vector3 launchDirection = CalculateLaunchDirection(hand2.position, player2.transform.position * 0.8f, 40f);
            rb.velocity = launchDirection * 0.8f;
        }
        else
        {
            Vector3 launchDirection = CalculateLaunchDirection(hand2.position, player1.transform.position * 0.8f, 40f);
            rb.velocity = launchDirection * 0.8f;
        }

        cnt++;

        bombController.StartDestroyCountdown(10f);
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

        //StartCoroutine(FadeOutAndDestroy(projectile, 5f, 1f)); // 코루틴 시작
    }

    //private IEnumerator FadeOutAndDestroy(GameObject projectile, float delay, float fadeDuration)
    //{
    //    // 5초 대기
    //    yield return new WaitForSeconds(delay);

    //    Renderer renderer = projectile.GetComponent<Renderer>();
    //    Material material = renderer.material;
    //    Color initialColor = material.color;

    //    // Alpha 값을 서서히 줄이기
    //    for (float t = 0; t < fadeDuration; t += Time.deltaTime)
    //    {
    //        float normalizedTime = t / fadeDuration;
    //        material.color = new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.Lerp(initialColor.a, 0f, normalizedTime));
    //        yield return null; // 다음 프레임까지 대기
    //    }

    //    // 완전히 사라진 후 삭제
    //    Destroy(projectile);
    //}





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
        if (phase == 2)
        {

            Vector3 finalVelocity
                = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
            return finalVelocity;
        }
        else
        {
            Vector3 finalVelocity
               = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity * 0.9f;
            return finalVelocity;
        }
    }

}
