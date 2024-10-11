using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCube : MonoBehaviour
{
    [Tooltip("개 캐릭터에 연결된 DogMovement 스크립트.")]
    public DogMovement dogMovement;

    private void Start()
    {
        if (dogMovement == null)
        {
            // 개가 DogMovement 스크립트를 가지고 있는 오브젝트를 찾아 자동으로 할당
            GameObject dog = GameObject.FindGameObjectWithTag("Dog");
            if (dog != null)
            {
                dogMovement = dog.GetComponent<DogMovement>();
            }

            if (dogMovement == null)
            {
                Debug.LogWarning("DogMovement 스크립트를 찾을 수 없습니다.");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("충돌 체크");
        // 장애물 태그를 "Obstacle"로 설정했다고 가정
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Vector3 eventPosition = collision.contacts[0].point;
            if (dogMovement != null)
            {
                Debug.Log("소음 이벤트");
                dogMovement.OnEventTriggered(eventPosition);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("소음 이벤트");
            dogMovement.OnEventTriggered(this.transform.position);
        }
    }
}
