using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraController : MonoBehaviour
{
    public Transform target; // 따라갈 대상 (캐릭터)
    public float distance = 5f; // 대상과의 거리
    public float height = 2f; // 카메라의 높이
    public float rotationSpeed = 100f; // 마우스 회전 속도

    private float currentX = 0f;
    private float currentY = 0f;

    void Update()
    {
        // 마우스 입력을 받아 회전 값 계산
        currentX += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        currentY -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        currentY = Mathf.Clamp(currentY, -35f, 60f); // 상하 회전 제한

        // 카메라의 위치 및 회전 설정
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = target.position + rotation * direction + new Vector3(0, height, 0);
        transform.LookAt(target.position + new Vector3(0, height, 0));
    }
}
