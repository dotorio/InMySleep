using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

public class ThirdPersonController : MonoBehaviourPun
{
    public Transform followCamera; // 활성화된 카메라의 Transform
    public float moveSpeed = 5f; // 기본 이동 속도
    public float sprintSpeedMultiplier = 1.5f; // 스프린트 시 속도 배율
    public float turnSmoothTime = 0.1f; // 회전 부드러움 시간
    private float turnSmoothVelocity;

    public float gravity = -9.81f; // 중력 값
    public float jumpHeight = 1.5f; // 점프 높이

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    public Animator animator;
    public bool isInteracting = false; // 상호작용 중인지 여부 확인

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!photonView.IsMine || isInteracting) // 원격 플레이어나 상호작용 중일 때는 업데이트 중단
        {
            return;
        }

        // 땅에 있는지 확인
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 작은 값으로 설정하여 땅에 붙어있게 함
            animator.SetInteger("animation", 1); // Idle 모션
        }

        // 입력 처리
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // 스프린트 처리
        bool isSprinting = Input.GetKey(KeyCode.LeftShift); // 스프린트 키를 눌렀는지 확인
        float currentMoveSpeed = moveSpeed;

        if ((horizontal != 0f || vertical != 0f) && isGrounded)
        {
            animator.SetInteger("animation", 18); // 걷기 모션
        }
        if (isSprinting)
        {
            currentMoveSpeed = moveSpeed * sprintSpeedMultiplier;

            if ((horizontal != 0f || vertical != 0f) && isGrounded)
            {
                animator.SetInteger("animation", 15); // 달리기 모션
            }
        }

        if (direction.magnitude >= 0.1f)
        {
            // 활성화된 카메라 방향을 기준으로 캐릭터의 이동 방향 계산
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + followCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            // 캐릭터 회전
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // 이동 방향
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * currentMoveSpeed * Time.deltaTime);
        }

        // 점프 처리
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetInteger("animation", 9); // 점프 모션
        }

        // 중력 적용
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // 현재 활성화된 카메라를 설정하는 메서드
    public void SetActiveCamera(Transform activeCameraTransform)
    {
        followCamera = activeCameraTransform;
    }
}

