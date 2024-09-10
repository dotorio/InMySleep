using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 0, 0); // x, y, z 축의 회전 속도 설정

    void Update()
    {
        // 매 프레임마다 로컬 좌표계를 기준으로 회전
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
    }
}