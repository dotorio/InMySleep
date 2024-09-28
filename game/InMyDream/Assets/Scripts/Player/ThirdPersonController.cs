using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ThirdPersonController : MonoBehaviourPun
{
    public Transform followCamera; // 활성화된 카메라의 Transform
    public float moveSpeed = 5f; // 기본 이동 속도
    public float sprintSpeedMultiplier = 1.5f; // 스프린트 시 속도 배율
    public float turnSmoothTime = 0.1f; // 회전 부드러움 시간
    private float turnSmoothVelocity;
    public AudioSource runAudio;
    public AudioSource jumpAudio;
    bool isJumping = false;
    public float gravity = -9.81f; // 중력 값
    public float jumpHeight = 1.5f; // 점프 높이

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private bool isDowned = false;
    private StageManager stageManager;
    public float respawnDelay = 3f; // 리스폰 시간 설정
    public CanvasGroup screenDarkness; // 화면 어둡게 하기위한 canvas

    public Animator animator;
    public bool isInteracting = false; // 상호작용 중인지 여부 확인

    // 물건 던지기 관련 변수
    public Transform throwPosition; // 물건을 던질 위치
    public GameObject objectToThrow; // 던질 물체
    public float throwForce = 10f; // 던질 힘
    private GameObject heldObject = null; // 들고 있는 물체
    private bool canPickUp = true; // 물체를 주울 수 있는지 여부

    // 조준점 UI 요소
    public GameObject crosshair; // 조준점 이미지

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // 시작 시 화면이 투명하게 설정
        if (screenDarkness != null)
        {
            screenDarkness.alpha = 0; // 처음에 화면을 투명하게 설정
        }

        // 게임 시작 시 조준점을 비활성화
        if (crosshair != null)
        {
            crosshair.SetActive(false);
        }
    }

    void Update()
    {
        if (!photonView.IsMine || isInteracting || isDowned) // 원격 플레이어나 상호작용 중일 때는 업데이트 중단
        {
            return;
        }

        HandleMovement();
        playAudio();
        HandleAimingAndThrowing(); // 조준 및 던지기 처리

        // 물체를 들고 있을 때 위치 업데이트
        if (heldObject != null)
        {
            UpdateHeldObjectPosition();
        }
    }
    
    void UpdateHeldObjectPosition()
    {
        // 부모 위치에 맞춰 물체 위치 업데이트
        if (heldObject != null)
        {
            // 부모 위치에 따라 오프셋을 조정
            heldObject.transform.position = throwPosition.position; // 부모 위치에 맞춰 설정
                                                                    // 또한 heldObject의 회전도 동기화할 수 있음
            heldObject.transform.rotation = throwPosition.rotation; // 필요시 회전 동기화
        }
    }

    void playAudio()
    {
        if (isJumping)
        {
            runAudio.Pause();
            return;
        }


        if (Input.GetKey(KeyCode.LeftShift) && !isJumping)
        {
            if (runAudio != null && !runAudio.isPlaying)
            {
                runAudio.Play(); // 소리 재생
            }
        }
        else
        {
            // Shift 키를 떼면 소리 멈춤
            if (runAudio != null && runAudio.isPlaying)
            {
                runAudio.Pause(); // 소리 일시정지
            }
        }
    }

    // 물건 집기
    void OnTriggerEnter(Collider other)
    {
        if(!photonView.IsMine)
        {
            return;
        }

        if (canPickUp && other.CompareTag("Grabable") && heldObject == null)
        {
            GrabableObject grabableObject = other.GetComponent<GrabableObject>();

            if(grabableObject != null)
            {
                if(!grabableObject.isHeld)
                {
                    PickUpObject(other.gameObject);
                }
                else
                {
                    Debug.Log("이 물체는 이미 다른 플레이어가 잡고 있습니다.");
                }
            }
        }
        else if(other.CompareTag("Bomb"))
        {
            SetCharacterDowned();
        }
        else if(other.CompareTag("Battery"))
        {
            PhotonView batteryPhotonView = other.GetComponent<PhotonView>();
            if (batteryPhotonView != null)
            {
                StageManager_1_2 stage1Manager = (StageManager_1_2)stageManager;

                stage1Manager.CollectBattery(batteryPhotonView.ViewID);
            }
        }
    }

    // 캐릭터 이동 처리
    void HandleMovement()
    {
        // 땅에 있는지 확인
        isGrounded = controller.isGrounded;
        if (canPickUp && isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 작은 값으로 설정하여 땅에 붙어있게 함
            animator.SetInteger("animation", 1); // Idle 모션
            isJumping = false;
        }

        // 입력 처리
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // 스프린트 처리
        bool isSprinting = Input.GetKey(KeyCode.LeftShift); // 스프린트 키를 눌렀는지 확인
        float currentMoveSpeed = moveSpeed;

        if ((horizontal != 0f || vertical != 0f) && isGrounded && canPickUp)
        {
            animator.SetInteger("animation", 18); // 걷기 모션
            //runAudio.Stop();

        }
        if (isSprinting)
        {
            currentMoveSpeed = moveSpeed * sprintSpeedMultiplier;

            if ((horizontal != 0f || vertical != 0f) && isGrounded && canPickUp)
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
            isJumping = true;
            jumpAudio.Play();
        }

        // 중력 적용
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // 조준 및 던지기 처리
    void HandleAimingAndThrowing()
    {
        // 마우스 오른쪽 버튼을 눌렀을 때 조준
        if (Input.GetMouseButton(1))
        {
            // 조준점 활성화
            if (crosshair != null)
            {
                crosshair.SetActive(true);
            }
            
            // 던질 물체가 있고, 우클릭 상태에서 좌클릭을 눌렀을 때 던지기
            if (heldObject != null && Input.GetMouseButtonDown(0))
            {
                ThrowObject();
            }

        }
        else // 마우스 오른쪽 버튼을 떼었을 때 조준 해제
        {
            // 조준점 비활성화
            if (crosshair != null)
            {
                crosshair.SetActive(false);
            }

        }
    }

    // 물체를 잡을 때 실행하는 메서드
    void PickUpObject(GameObject objectToPickUp)
    {
        if (objectToPickUp != null)
        {
            // 모든 클라이언트에 물체가 들렸다는 사실과 부모 설정 동기화
            photonView.RPC("RPC_PickUp", RpcTarget.All, objectToPickUp.GetComponent<PhotonView>().ViewID, throwPosition.position);

            // 로컬에서 물건을 들고 있는 상태로 설정
            heldObject = objectToPickUp;
        }
    }

    // 물체의 줍는 동작을 동기화하는 RPC 메서드
    [PunRPC]
    void RPC_PickUp(int objectViewID, Vector3 parentPosition)
    {
        GameObject obj = PhotonView.Find(objectViewID).gameObject;
        obj.GetComponent<GrabableObject>().isHeld = true;
        
        obj.GetComponent<Rigidbody>().isKinematic = true;
        Collider[] colliders = obj.GetComponents<Collider>();
        foreach(var collider in colliders)
        {
            collider.enabled = false;
        }

        obj.transform.localPosition = parentPosition;
    }


    // 물건을 던질 때 실행하는 메서드
    void ThrowObject()
    {
        if (heldObject != null)
        {
            canPickUp = false;
            StartCoroutine(EnablePickUpAfterDelay(0.5f));

            // 던질 위치 및 방향 설정
            Vector3 throwStartPosition = throwPosition.position + followCamera.forward * 0.5f; // 앞쪽으로 0.5f 이동

            Vector3 throwDirection = followCamera.forward + Vector3.up; // 0.5f는 조정 가능한 높이
            throwDirection.Normalize(); // 방향 벡터 정규화

            // 던지기 동작을 동기화
            photonView.RPC("RPC_ThrowObject", RpcTarget.All, heldObject.GetComponent<PhotonView>().ViewID, throwStartPosition, throwDirection, throwForce);

            animator.SetInteger("animation", 20);

            // 로컬에서 물건을 초기화
            heldObject = null;
        }
    }

    // 물건을 던지는 동작을 동기화하는 RPC 메서드
    [PunRPC]
    void RPC_ThrowObject(int objectViewID, Vector3 throwStartPosition, Vector3 direction, float force)
    {
        GameObject obj = PhotonView.Find(objectViewID).gameObject;

        Rigidbody objectRb = obj.GetComponent<Rigidbody>();
        objectRb.isKinematic = false;
        Collider[] colliders = obj.GetComponents<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }

        GrabableObject grabableObject = obj.GetComponent<GrabableObject>();
        grabableObject.isHeld = false;

        // 던질 위치 및 방향 설정
        obj.transform.position = throwStartPosition;

        // 던진 후 물리적 상호작용 활성화
        objectRb.isKinematic = false;
        objectRb.AddForce(direction * force, ForceMode.VelocityChange);
    }

    private IEnumerator EnablePickUpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canPickUp = true; // 일정 시간 후 줍기 기능 다시 활성화
    }


    // 현재 활성화된 카메라를 설정하는 메서드
    public void SetActiveCamera(Transform activeCameraTransform)
    {
        followCamera = activeCameraTransform;
    }

    // stage manager 설정 메소드
    public void SetStageManager(StageManager stage)
    {
        stageManager = stage;
    }

    // 캐릭터 쓰러짐 -> 리스폰 동작 수행하는 메소드
    public void SetCharacterDowned()
    {
        if (!isDowned)
        {
            isDowned = true;
            animator.SetInteger("animation", 6); // 쓰러지는 애니메이션
            ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
            playerProps["isDowned"] = true;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
            StartCoroutine(HandleRespawn()); // 리스폰 처리
        }
    }

    // 리스폰 메소드
    IEnumerator HandleRespawn()
    {
        yield return new WaitForSeconds(1.5f);

        if (screenDarkness != null)
        {
            float fadeDuration = 1f;
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                screenDarkness.alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        yield return new WaitForSeconds(respawnDelay);

        Transform spawnPoint = stageManager.getSpawnPoint();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        if (screenDarkness != null)
        {
            float fadeDuration = 1f;
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                screenDarkness.alpha = Mathf.Lerp(1, 0, elapsed / fadeDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        isDowned = false;
        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
        playerProps["isDowned"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
        Debug.Log("플레이어가 리스폰되었습니다.");
    }
}
