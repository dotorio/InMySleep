using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private float health = 100f;  // 보스 체력
    private float moveSpeed = 2f;  // 이동 속도
    public GameObject player;  // 플레이어 참조
    public Transform attackPoint;  // 근접 공격 지점
    private float attackRange = 10f;  // 근접 공격 범위
    private int meleeDamage = 10;  // 근접 공격 데미지
    public GameObject projectilePrefab;  // 원거리 공격 구체
    public Transform firePoint;  // 원거리 공격 발사 지점
    private float projectileSpeed = 10f;  // 초기 발사 속도
    private float projectileCooldown = 1f;  // 원거리 공격 쿨타임
    private float lastProjectileTime = 0f;  // 마지막 원거리 공격 시간
    private float projectileLifetime = 5f;  // 발사체의 생명주기 (초)

    private void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // 사정거리에 따라 다른 패턴 사용
        if (distanceToPlayer <= attackRange)
        {
            // 근접 공격
            MeleeAttack();
        }
        else
        {
            // 원거리 공격
            RangedAttack();
        }
    }

    void MeleeAttack()
    {
        Debug.Log("보스가 근접 공격을 시도합니다!");
        // 근접 공격 로직 구현 (예: 플레이어에게 데미지 입히기)
    }

    void RangedAttack()
    {
        if (Time.time - lastProjectileTime >= projectileCooldown)
        {
            lastProjectileTime = Time.time;

            Debug.Log("보스가 포물선 원거리 공격을 시도합니다!");
            LaunchProjectile();
        }
    }

    void LaunchProjectile()
    {
        // 발사체 생성
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // 발사 방향 계산
        //Vector3 launchDirection = CalculateLaunchDirection();
        Vector3 launchDirection = CalculateLaunchDirection(firePoint.position, player.transform.position, 30f);
        rb.velocity = launchDirection;

        // 발사체를 5초 후에 제거
        Destroy(projectile, projectileLifetime);
    }

    //Vector3 CalculateLaunchDirection()
    //{
    //    Vector3 targetPosition = player.transform.position;
    //    Vector3 direction = targetPosition - firePoint.position;

    //    float horizontalDistance = new Vector3(direction.x, 0, direction.z).magnitude;
    //    float verticalDistance = direction.y;

    //    // 발사각을 45도로 설정 (혹은 원하는 각도로 조정)
    //    float angleInRadians = 30 * Mathf.Deg2Rad;

    //    // XZ 평면에서의 발사 속도 계산
    //    float velocityXZ = Mathf.Sqrt(Mathf.Abs(Physics.gravity.y * horizontalDistance) / Mathf.Abs((2 * Mathf.Sin(angleInRadians) * Mathf.Cos(angleInRadians))));

    //    // NaN이 발생할 수 있는 경우를 대비해 속도가 양수인지 확인
    //    if (float.IsNaN(velocityXZ))
    //    {
    //        Debug.Log("제대로 안되고 있어");
    //        velocityXZ = projectileSpeed; // 기본 속도로 대체
    //    }

    //    // 수직 방향 속도 계산
    //    float velocityY = velocityXZ * Mathf.Sin(angleInRadians);

    //    // 최종 발사 방향 벡터 계산
    //    Vector3 directionXZ = new Vector3(direction.x, 0, direction.z).normalized;
    //    Vector3 launchVelocity = directionXZ * velocityXZ + Vector3.up * velocityY;

    //    return launchVelocity;
    //}

    Vector3 CalculateLaunchDirection(Vector3 player, Vector3 target, float initialAngle)
    {
        float gravity = Physics.gravity.magnitude;
        float angle = initialAngle * Mathf.Deg2Rad;

        Vector3 planarTarget = new Vector3(target.x, 0, target.z);
        Vector3 planarPosition = new Vector3(player.x, 0, player.z);

        float distance = Vector3.Distance(planarTarget, planarPosition);
        float yOffset = player.y - target.y;

        float initialVelocity
            = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity
            = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        float angleBetweenObjects
            = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (target.x > player.x ? 1 : -1);
        Vector3 finalVelocity
            = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        return finalVelocity;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("보스가 사망했습니다!");
        Destroy(gameObject);  // 보스 오브젝트 제거
    }
}
