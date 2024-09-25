using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCube : MonoBehaviour
{
    public DogMovement dogMovement; // 개의 움직임을 제어하는 스크립트 (Inspector에서 설정)

    void Update()
    {
        // E 키를 눌렀을 때 개가 이 큐브의 위치로 이동
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetEventLocation();
        }
    }

    void SetEventLocation()
    {
        // 큐브의 위치를 event location으로 설정하고 개가 해당 위치로 이동하도록 트리거
        dogMovement.eventLocation = this.transform;
        dogMovement.OnEventTriggered();
    }
}
