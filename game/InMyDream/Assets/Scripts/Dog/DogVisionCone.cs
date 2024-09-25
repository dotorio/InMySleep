using UnityEngine;

public class DogVisionCone : MonoBehaviour
{
    // Projector 변수 (바닥에 원을 투사할 Projector)
    public Projector visionProjector;

    // 시야 콘의 기준이 되는 Transform (개 머리)
    public Transform coneOrigin;

    // 시야 범위 및 높이 (시야 콘을 설정할 때 사용)
    public float coneRange = 10f;
    public float coneHeight = 1.0f;

    // 바닥에 닿는 지점의 위치
    private Vector3 groundHitPosition;

    void Update()
    {
        // 바닥에 닿는 위치 감지
        CastVisionCone();
    }

    void CastVisionCone()
    {
        // 개의 시야 방향에 따라 Ray를 아래로 쏨
        Ray ray = new Ray(coneOrigin.position, Vector3.down);
        RaycastHit hit;

        // Raycast로 바닥을 감지
        if (Physics.Raycast(ray, out hit, coneRange))
        {
            // Ray가 바닥에 닿은 위치
            groundHitPosition = hit.point;

            // Projector를 닿은 위치로 이동시킴
            visionProjector.transform.position = groundHitPosition;

            // Projector의 회전 방향을 바닥의 법선에 맞추어 설정
            visionProjector.transform.rotation = Quaternion.LookRotation(hit.normal);
        }
        else
        {
            // Ray가 바닥에 닿지 않았을 경우 Projector를 비활성화
            visionProjector.enabled = false;
        }
    }

    // 필요시 콘을 개의 회전에 맞게 움직이게 하는 함수
    public void UpdateConeRotation(Quaternion rotation)
    {
        // 콘의 회전 방향을 개의 머리에 맞게 업데이트
        coneOrigin.rotation = rotation;
    }
}
