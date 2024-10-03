using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialObject : MonoBehaviour
{
    private StageManager_3 stageManager;
    public GameObject Star;
    public GameObject keyPoint;
    public GameObject obj;
    public int isKey;
    

    public float duration = 2f; // 서서히 이동할 시간


    // Start is called before the first frame update
    void Start()
    {
        stageManager = FindObjectOfType<StageManager_3>();
        isKey = stageManager.RandomInt();


        if (stageManager != null)
        {
            Debug.Log("매니저 불러옴!");
        }
        else
        {
            Debug.Log("매니저 못 불러옴!");
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Star = GameObject.Find("Star(Clone)");
            GrabableObject grabableObject = Star.GetComponent<GrabableObject>();
            if (grabableObject != null)
            {
                if (!grabableObject.isHeld)
                {
                    Debug.Log("열쇠 안 들고 있음");
                }
                else
                {
                    Debug.Log("열쇠 들고 있음");
                    StartCoroutine(ReduceYByTwoOverTime());
                    Destroy(Star);
                    Destroy(keyPoint);

                }

            }

        }

    }


    IEnumerator ReduceYByTwoOverTime()
    {
        if (obj != null)
        {
            // 시작 위치 저장
            Vector3 startPosition = obj.transform.position;
            // 목표 위치는 Y값을 -2 한 값으로 설정
            Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y - 2f, startPosition.z);

            float elapsedTime = 0f;

            // duration 동안 서서히 위치를 변경
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                // Lerp를 사용해 위치를 서서히 변화시킴
                obj.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

                // 다음 프레임까지 대기
                yield return null;
            }

            // 목표 위치에 도달하면 정확히 목표 위치로 설정
            obj.transform.position = targetPosition;

        }
    }
}
