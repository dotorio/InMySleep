using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam; // 카메라 참조

    private void Start()
    {
        cam = transform.parent.gameObject.transform;
    }

    void LateUpdate()
    {
        // Canvas가 카메라를 향하도록 회전
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                         cam.transform.rotation * Vector3.up);
    }
}

