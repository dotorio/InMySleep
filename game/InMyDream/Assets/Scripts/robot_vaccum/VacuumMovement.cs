using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumMovement : MonoBehaviour
{
    public float moveSpeed = 5f;      // 기본 속도
    public float reducedSpeedFactor = 0.1f;  // 속도를 줄일 비율 (예: 0.5 = 절반 속도)
    private float currentSpeed;       // 현재 속도 저장

    void Start()
    {
        // 초기 속도를 기본 속도로 설정
        currentSpeed = moveSpeed;
        // 2번째 인자는 처음 실행까지의 대기 시간(초), 3번째 인자는 반복 주기(초)
        InvokeRepeating("RepeatFunction", 0f, 10f);
    }

    void Update()
    {
        // 물체를 Z축 양의 방향으로 일정한 속도로 계속 움직임
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        Debug.Log("현재 속도: " + currentSpeed);
        //Debug.Log("현재 시간: " + Time.deltaTime);
        //Debug.Log("Time.timeScale: " + Time.timeScale);
        //Debug.Log("Update 함수가 호출되었습니다.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            if (other.CompareTag("TileObject"))
            {
                Debug.Log("속도 느림!!");
                currentSpeed -= reducedSpeedFactor;
            }

            other.gameObject.SetActive(false);
        }
    }
    void RepeatFunction()
    {
        // 5초마다 실행되는 코드
        if (currentSpeed < 4)
        {
            currentSpeed = 4;
        }
    }

}
