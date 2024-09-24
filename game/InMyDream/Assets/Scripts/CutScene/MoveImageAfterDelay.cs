using System.Collections;
using UnityEngine;

public class MoveImageAfterDelay : MonoBehaviour
{
    public GameObject imageObject;  // 나타낼 이미지 오브젝트
    public float moveSpeed = 2.0f;  // 위아래로 움직일 속도
    public float moveDistance = 50.0f;  // 위아래로 움직일 최대 거리

    private Vector3 startPos;  // 이미지의 초기 위치
    private bool startMoving = false;  // 움직임을 시작했는지 여부

    void Start()
    {
        // 이미지 오브젝트 초기 위치 저장
        startPos = imageObject.transform.localPosition;

        // 코루틴을 사용해 3초 후 이미지 나타나기
        StartCoroutine(ShowAndMoveImage(3.0f));
    }

    IEnumerator ShowAndMoveImage(float seconds)
    {
        // 지정한 시간(3초) 동안 대기
        yield return new WaitForSeconds(seconds);

        // 이미지 오브젝트 활성화
        imageObject.SetActive(true);

        // 움직임 시작
        startMoving = true;
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
