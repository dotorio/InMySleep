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

    public Transform playerCameraRoot;  // 플레이어의 카메라 루트 (카메라가 따라갈 대상)

    private void Start()
    {
        if (photonView.IsMine)
        {
            // 로컬 플레이어일 경우
            playerCamera.enabled = true;  // 메인 카메라 활성화
            cinemachineBrain.enabled = true;  // Cinemachine 브레인 활성화

            // 모든 가상 카메라를 비활성화
            foreach (var camera in virtualCameras)
            {
                camera.Priority = 1;
            }
        }
        else
        {
            // 원격 플레이어일 경우
            playerCamera.enabled = false;  // 메인 카메라 비활성화
            cinemachineBrain.enabled = false;  // Cinemachine 브레인 비활성화

            // 모든 가상 카메라를 비활성화
            foreach (var camera in virtualCameras)
            {
                camera.Priority = 0;
                camera.gameObject.SetActive(false);
            }
        }
    }
}
