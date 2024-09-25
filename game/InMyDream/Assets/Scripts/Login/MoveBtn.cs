using System.Collections;
using UnityEngine;

public class MoveBtn : MonoBehaviour
{
    public GameObject imageObject;  // 나타낼 이미지 오브젝트
    public float moveSpeed = 2.0f;  // 위아래로 움직일 속도
    public float moveDistance = 5.0f;  // 위아래로 움직일 최대 거리

    private Vector3 startPos;  // 이미지의 초기 위치
    private bool startMoving = false;  // 움직임을 시작했는지 여부

    void Start()
    {
        // 움직임 시작
        startMoving = true;
        startPos = new Vector3(0, -336, 0);
    }

    void Update()
    {
        if (startMoving)
        {
            // 시간에 따라 위아래로 움직이도록 함
            float newY = startPos.y + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
            imageObject.transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
        }
    }
}
