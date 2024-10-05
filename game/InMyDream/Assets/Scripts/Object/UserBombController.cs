using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class UserBombController : MonoBehaviour
{
    public Transform target; // 목표 지점
    public float speed = 5f; // 이동 속도
    public float height = 5f; // 포물선의 최대 높이

    private Vector3 startPosition;
    private float progress = 0f;

    void Start()
    {
        startPosition = transform.position; // 시작 지점
    }

    void Update()
    {
        if (target != null && PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            // 0에서 1까지의 진행도 계산
            progress += Time.deltaTime * speed / Vector3.Distance(startPosition, target.position);
            progress = Mathf.Clamp01(progress); // 1을 넘지 않도록 제한

            // xz 평면상에서 선형적으로 물체 이동
            Vector3 currentPos = Vector3.Lerp(startPosition, target.position, progress);

            // y 축 방향으로 포물선 모양 적용
            float yOffset = height * Mathf.Sin(Mathf.PI * progress); // 포물선 곡선 적용
            currentPos.y += yOffset;

            // 물체의 위치를 업데이트
            transform.position = currentPos;

            // 목표 지점에 도달했을 때 처리
            if (progress >= 1f)
            {
                // 목표에 도착했을 때의 처리 (예: 충돌 처리 등)
                Debug.Log("Reached Target");
            }
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
