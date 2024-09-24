using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    private float turnSpeed = 30f;  // 초당 회전 속도
    public float walkSpeed = 2f;   // 걷기 속도
    private float waitTime = 3f;    // 대기 애니메이션 시간
    public float walkDuration = 3f; // 걷기 지속 시간

    private Animator animator;
    private bool isTurning = false;
    private bool isWalking = false;
    private float turnAmount = 0f;
    private float walkTimer = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();  // 자동으로 애니메이터 가져오기
        StartCoroutine(StateRoutine());
    }

    IEnumerator StateRoutine()
    {
        while (true)
        {
            // 1. 대기 애니메이션 재생
            animator.SetTrigger("Idle");
            yield return new WaitForSeconds(waitTime);  // 대기 시간

            // 2. 오른쪽으로 돌기 애니메이션 재생하면서 180도 회전
            isTurning = true;
            turnAmount = 0f;

            while (turnAmount < 180f)
            {
                // 오른쪽으로 돌기 애니메이션 상태 유지
                animator.SetBool("IsTurning", true);
                float turnStep = turnSpeed * Time.deltaTime;
                transform.Rotate(0f, turnStep, 0f);
                turnAmount += turnStep;
                yield return null;
            }
            isTurning = false;
            animator.SetBool("IsTurning", false); // 돌기가 끝난 후 상태 초기화

            // 3. 걷기 애니메이션 재생하면서 앞으로 이동
            isWalking = true;
            walkTimer = 0f;
            animator.SetBool("IsWalking", true); // 걷기 애니메이션 시작

            while (walkTimer < walkDuration)
            {
                transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
                walkTimer += Time.deltaTime;
                yield return null;
            }
            isWalking = false;
            animator.SetBool("IsWalking", false); // 걷기가 끝난 후 상태 초기화
        }
    }
}
