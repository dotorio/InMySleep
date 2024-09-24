using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public int health = 3;
    public AudioClip destructionSound; // 파괴 시 소리
    public GameObject destructionEffect; // 파괴 시 이펙트


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Grabable"))
        {
            health--;

            if(health <= 0)
            {
                // 파괴 이펙트 생성
                if (destructionEffect != null)
                {
                    Instantiate(destructionEffect, transform.position, transform.rotation);
                }

                // 파괴 사운드 재생
                if (destructionSound != null)
                {
                    AudioSource.PlayClipAtPoint(destructionSound, transform.position);
                }

                Destroy(gameObject);
                Debug.Log("책장 받침이 파괴되었습니다.");
            }
        }
    }
}
