using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    // Rigidbody를 추가할 대상 오브젝트를 설정합니다.
    public GameObject targetObject;
    public GameObject tileObject;
   
    public int objectType;
    public float forceAmount = 200f;
    public float moveSpeed = 5f;
    public GameObject[] targetObjects;


    // Trigger가 발생했을 때 호출되는 함수
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 "Player" 태그를 가지고 있는지 확인
        if (other.CompareTag("Player"))
        {
            // 장애물1 쓰러지기
            if (objectType == 0)
            {

                // targetObject에 Rigidbody 컴포넌트가 없으면 추가
                if (targetObject != null && targetObject.GetComponent<Rigidbody>() == null)
                {
                    targetObject.AddComponent<Rigidbody>();
                    Rigidbody rb = targetObject.GetComponent<Rigidbody>();
                    rb.mass = 100;
                    Debug.Log("Rigidbody가 추가되었습니다.");
                }

            }
            else if (objectType == 1)
            {
                // 대상 오브젝트에 Rigidbody가 있는지 확인하고 없다면 추가
               
                Debug.Log("z축 방향으로 힘이 적용되었습니다.");
            }
            else if (objectType == 2)
            {
                
                foreach (GameObject targetObject in targetObjects)
                {
                    // 각 오브젝트에 Rigidbody가 있는지 확인하고 없다면 추가
                    Rigidbody rb = targetObject.GetComponent<Rigidbody>();
                    if (rb == null)
                    {
                        rb = targetObject.AddComponent<Rigidbody>();
                    }
                    rb.mass = 200;
                    // z축 음수 방향으로 힘을 추가 (Vector3.back은 (0, 0, -1)을 의미)
                    rb.AddForce(Vector3.back * forceAmount, ForceMode.Impulse);

                    Debug.Log(targetObject.name + "에 z축 음수 방향으로 힘이 적용되었습니다.");
                }
            }
            else if (objectType == 3)
            {

                tileObject.SetActive(true);
                
            }
            else if (objectType == 4)
            {

                tileObject.SetActive(false);
                
            }
        }
    }

}
