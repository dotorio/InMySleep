using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("시작");
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 충돌이 발생했을 때 호출되는 메서드
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("충돌한 물체 태그: " + collision.gameObject.tag);

        if (collision.gameObject.tag == "Shelf")
        {
            Debug.Log("Box가 Shelf와 충돌했습니다!");
        }
    }

}
