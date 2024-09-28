using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    // 커지고 줄어드는 속도
    public float scaleSpeed = 2.0f;

    // 최소 크기와 최대 크기
    public Vector3 minScale = new Vector3(0.8f, 0.8f, 0.8f);
    public Vector3 maxScale = new Vector3(1.2f, 1.2f, 1.2f);

    private bool isGrowing = true;

    void Update()
    {
        if (isGrowing)
        {
            // 커지는 애니메이션
            transform.localScale = Vector3.Lerp(transform.localScale, maxScale, Time.deltaTime * scaleSpeed);
            if (Vector3.Distance(transform.localScale, maxScale) < 0.01f)
            {
                isGrowing = false;
            }
        }
        else
        {
            // 작아지는 애니메이션
            transform.localScale = Vector3.Lerp(transform.localScale, minScale, Time.deltaTime * scaleSpeed);
            if (Vector3.Distance(transform.localScale, minScale) < 0.01f)
            {
                isGrowing = true;
            }
        }
    }
}
