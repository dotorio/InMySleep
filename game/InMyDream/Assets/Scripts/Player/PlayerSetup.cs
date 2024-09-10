using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class PlayerSetup : MonoBehaviourPun
{
    public Camera playerCamera;
    public CinemachineBrain cinemachineBrain;
    public CinemachineVirtualCamera[] virtualCameras;
    public GameObject playerCanvas;
    public float interactDistance = 3f;
    private bool isNearby = false;
    private Transform objectToInteract = null;
    public float pushForce = 5f;
    public GameObject breakEffectPrefab;
    public Animator animator;
    private string cleanName;
    private float holdTime = 2f;
    private float holdTimer = 0f;
    private bool isPushing = false; // 캐릭터가 물체를 밀고 있는지 여부
    public float moveSpeed = 3f; // 캐릭터가 물체를 밀 때의 이동 속도
    private Vector3 initialObjectPosition; // 물체의 초기 위치 저장

    private void Start()
    {
        if (photonView.IsMine)
        {
            cleanName = gameObject.name.Replace("(Clone)", "").Trim();
            playerCamera.enabled = true;
            playerCamera.cullingMask = LayerMask.GetMask(cleanName + "UI", "Default");
            cinemachineBrain.enabled = true;
            playerCanvas.SetActive(true);

            foreach (var camera in virtualCameras)
            {
                camera.Priority = 1;
            }
        }
        else
        {
            playerCamera.enabled = false;
            playerCamera.cullingMask = LayerMask.GetMask("Default");
            cinemachineBrain.enabled = false;
            playerCanvas.SetActive(false);

            foreach (var camera in virtualCameras)
            {
                camera.Priority = 0;
                camera.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (objectToInteract != null)
        {
            float distance = Vector3.Distance(transform.position, objectToInteract.position);

            if (distance <= interactDistance)
            {
                if (objectToInteract.CompareTag("Interactable"))
                {
                    isNearby = true;
                    playerCanvas.SetActive(true);

                    if (Input.GetKey(KeyCode.E))
                    {
                        if (holdTimer == 0f) // E 키를 처음 눌렀을 때
                        {
                            animator.SetInteger("animation", 8); // 상호작용 애니메이션
                            GetComponent<ThirdPersonController>().isInteracting = true;
                            initialObjectPosition = objectToInteract.position; // 물체의 초기 위치 저장

                            // 방향 즉시 설정
                            SetCharacterDirection(objectToInteract);
                        }

                        holdTimer += Time.deltaTime; // E 키를 누르고 있는 시간 계산

                        if (holdTimer >= holdTime) // 2초 이상 누르고 있으면 물체 밀기
                        {
                            isPushing = true; // 물체를 밀고 있는 상태
                        }

                        if (isPushing)
                        {
                            PushObjectContinuously(objectToInteract); // 캐릭터가 계속 밀게 함
                        }
                    }
                    else
                    {
                        holdTimer = 0f; // E 키를 떼면 타이머 초기화
                        isPushing = false; // 밀기 중지
                        GetComponent<ThirdPersonController>().isInteracting = false;
                    }
                }
                else
                {
                    holdTimer = 0f; // E 키를 떼면 타이머 초기화
                    isPushing = false; // 밀기 중지
                    GetComponent<ThirdPersonController>().isInteracting = false;

                    if (cleanName == "Player1" && Input.GetMouseButtonDown(0) && objectToInteract.CompareTag("Breakable"))
                    {
                        StartCoroutine(BreakObjectWithDelay(objectToInteract.gameObject, 0.5f));
                    }
                }
            }
            else
            {
                isNearby = false;
                playerCanvas.SetActive(false);
                GetComponent<ThirdPersonController>().isInteracting = false;
            }
        }
        else
        {
            playerCanvas.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable") || other.CompareTag("Breakable"))
        {
            objectToInteract = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == objectToInteract)
        {
            objectToInteract = null;
            isNearby = false;
            playerCanvas.SetActive(false);

            if (other.CompareTag("Interactable"))
            {
                GetComponent<ThirdPersonController>().isInteracting = false;
            }
        }
    }

    void SetCharacterDirection(Transform obj)
    {
        // 캐릭터의 방향을 즉시 설정
        Vector3 pushDirection = (obj.position - transform.position).normalized;
        Vector3 moveDirection = Vector3.ProjectOnPlane(pushDirection, Vector3.up).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        transform.rotation = targetRotation;
    }

    void PushObjectContinuously(Transform obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 pushDirection = (obj.position - transform.position).normalized;
            Vector3 moveDirection = Vector3.ProjectOnPlane(pushDirection, Vector3.up).normalized;

            // 물체에 힘을 계속 추가
            rb.AddForce(pushDirection * pushForce * Time.deltaTime, ForceMode.VelocityChange);

            // 캐릭터가 물체를 향해 이동
            Vector3 newCharacterPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
            transform.position = newCharacterPosition;

            // 물체가 캐릭터와 함께 이동
            obj.position = newCharacterPosition + pushDirection * 1f; // 1f는 물체와 캐릭터 간의 거리 조절
        }
    }

    IEnumerator BreakObjectWithDelay(GameObject obj, float delay)
    {
        BreakObject(obj);
        yield return new WaitForSeconds(delay);
        animator.SetInteger("animation", 1);
        GetComponent<ThirdPersonController>().isInteracting = false;
    }

    void BreakObject(GameObject obj)
    {
        animator.SetInteger("animation", 20);
        GetComponent<ThirdPersonController>().isInteracting = true;

        if (breakEffectPrefab != null)
        {
            Instantiate(breakEffectPrefab, obj.transform.position, Quaternion.identity);
        }

        Destroy(obj);
        Debug.Log("Object broken: " + obj.name);
    }
}
