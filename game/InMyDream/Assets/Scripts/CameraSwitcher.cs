using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera followCamera;
    public Cinemachine.CinemachineVirtualCamera fixedCamera1;
    public Cinemachine.CinemachineVirtualCamera fixedCamera2;
    public ThirdPersonController playerController;

    void Start()
    {
        // 기본적으로 이동 카메라 활성화
        SwitchToFollowCamera();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchToFollowCamera();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchToFixedCamera1();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchToFixedCamera2();
        }
    }

    void SwitchToFollowCamera()
    {
        followCamera.enabled = true;
        fixedCamera1.enabled = false;
        fixedCamera2.enabled = false;
        playerController.SetActiveCamera(followCamera.transform);
    }

    void SwitchToFixedCamera1()
    {
        followCamera.enabled = false;
        fixedCamera1.enabled = true;
        fixedCamera2.enabled = false;
        playerController.SetActiveCamera(fixedCamera1.transform);
    }

    void SwitchToFixedCamera2()
    {
        followCamera.enabled = false;
        fixedCamera1.enabled = false;
        fixedCamera2.enabled = true;
        playerController.SetActiveCamera(fixedCamera2.transform);
    }
}

