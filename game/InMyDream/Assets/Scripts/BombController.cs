using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("폭탄이 어딘가에 닿았습니다.");
        // 폭발 효과 또는 다른 로직 추가 가능
    }
}
