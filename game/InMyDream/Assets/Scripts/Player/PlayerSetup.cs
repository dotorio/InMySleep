using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class PlayerSetup : MonoBehaviourPun
{
    public Camera playerCamera;  // 각 플레이어에 할당된 카메라
    public CinemachineBrain cinemachineBrain;  // Cinemachine 브레인
    public CinemachineVirtualCamera[] virtualCameras;  // 3개의 Cinemachine 가상 카메라 배열
    public GameObject playerCanvas; // 연결된 Canvas
    public float interactDistance = 3f; // 상호작용 거리
    private bool isNearby = false;
    private Transform objectToInteract = null; // 상호작용할 물체 저장

    private void Start()
    {
        if (photonView.IsMine)
        {
            string cleanName = gameObject.name.Replace("(Clone)", "").Trim();
            // 로컬 플레이어일 경우
            playerCamera.enabled = true;  // 메인 카메라 활성화
            playerCamera.cullingMask = LayerMask.GetMask(cleanName+"UI", "Default");
            cinemachineBrain.enabled = true;  // Cinemachine 브레인 활성화
            playerCanvas.SetActive(true); // 로컬 플레이어의 Canvas만 활성화

        foreach (var camera in virtualCameras)
            {
                camera.Priority = 1;
            }
        }
        else
        {
            // 원격 플레이어일 경우
            playerCamera.enabled = false;  // 메인 카메라 비활성화
            playerCamera.cullingMask = LayerMask.GetMask("Default");
            cinemachineBrain.enabled = false;  // Cinemachine 브레인 비활성화
            playerCanvas.SetActive(false); // 다른 플레이어의 Canvas는 비활성화

            // 모든 가상 카메라를 비활성화
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
            // 물체와의 거리 측정
            float distance = Vector3.Distance(transform.position, objectToInteract.position);

            // 상호작용 가능한 물체에 가까이 있을 때만 캔버스 활성화
            if (distance <= interactDistance)
            {
                isNearby = true;
                playerCanvas.SetActive(true); // 캔버스 활성화
            }
            else
            {
                isNearby = false;
                playerCanvas.SetActive(false); // 캔버스 비활성화
            }

            // E 키를 눌렀을 때 상호작용
            if (isNearby && Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }
        else
        {
            // 상호작용할 물체가 없으면 캔버스를 비활성화
            playerCanvas.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 상호작용 가능한 물체인지 확인 (예: Tag로 확인)
        if (other.CompareTag("Interactable"))
        {
            objectToInteract = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // 상호작용 가능한 물체에서 멀어졌을 때
        if (other.transform == objectToInteract)
        {
            objectToInteract = null;
            isNearby = false;
            playerCanvas.SetActive(false); // 캔버스 비활성화
        }
    }

    void Interact()
    {
        // 상호작용 로직 처리
        Debug.Log("Interacting with object: " + objectToInteract.name);
        // 원하는 동작을 여기서 구현
    }
}
