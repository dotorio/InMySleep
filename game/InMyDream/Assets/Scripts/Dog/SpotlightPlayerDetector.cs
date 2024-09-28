using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpotlightPlayerDetector : MonoBehaviour
{
    [Header("Spotlight Settings")]
    public float viewRadius = 10f; // Spotlight의 Range와 일치
    [Range(0, 360)]
    public float viewAngle = 45f; // Spotlight의 Spot Angle과 일치

    [Header("Detection Settings")]
    public LayerMask obstacleMask; // 시야를 차단하는 레이어
    public List<Transform> players = new List<Transform>(); // 여러 플레이어의 Transform

    private Dictionary<Transform, bool> playerInViewDict = new Dictionary<Transform, bool>(); // 플레이어 감지 상태 저장
    private DogMovement dogMovement; // DogMovement 스크립트 참조

    void Start()
    {
        // 부모 오브젝트에서 DogMovement 컴포넌트를 찾음
        dogMovement = GetComponentInParent<DogMovement>();

        if (dogMovement == null)
        {
            Debug.LogError("부모 오브젝트에서 DogMovement 스크립트를 찾을 수 없습니다. DogMovement 컴포넌트가 부모에 있는지 확인하세요.");
        }
    }

    void Update()
    {
        foreach (Transform player in players)
        {
            if (player == null)
                continue;

            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= viewRadius)
            {
                float angleBetween = Vector3.Angle(transform.forward, dirToPlayer);
                if (angleBetween <= viewAngle / 2)
                {
                    if (!Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, obstacleMask))
                    {
                        if (!playerInViewDict[player])
                        {
                            playerInViewDict[player] = true;
                            Debug.Log($"{player.name}가 개의 시야에 감지되었습니다!");
                            dogMovement.StartBarking(); // 개가 짖는 동작 실행

                            PhotonView playerPhotonView = player.gameObject.GetComponent<PhotonView>();
                            if(playerPhotonView != null)
                            {
                                if(playerPhotonView.IsMine)
                                {
                                    ThirdPersonController controller = player.gameObject.GetComponent<ThirdPersonController>();
                                    controller.SetCharacterDowned();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (playerInViewDict[player])
                        {
                            playerInViewDict[player] = false;
                            Debug.Log($"{player.name}가 개의 시야에서 벗어났습니다.");
                            dogMovement.StopBarking(); // 개가 짖는 동작 멈춤
                        }
                    }
                }
                else
                {
                    if (playerInViewDict[player])
                    {
                        playerInViewDict[player] = false;
                        Debug.Log($"{player.name}가 개의 시야에서 벗어났습니다.");
                        //dogMovement.StopBarking();
                    }
                }
            }
            else
            {
                if (playerInViewDict[player])
                {
                    playerInViewDict[player] = false;
                    Debug.Log($"{player.name}가 개의 시야에서 벗어났습니다.");
                    //dogMovement.StopBarking();
                }
            }
        }
    }

    // 플레이어 추가
    public void addPlayer(GameObject playerObj)
    {
            players.Add(playerObj.transform);
            playerInViewDict[playerObj.transform] = false; // 초기 감지 상태는 false
    }

    // 이 함수는 Scene 뷰에서 감지 범위를 시각화합니다.
    void OnDrawGizmos()
    {
        // 시야 반경을 그립니다 (원형으로)
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        // 시야각을 표현하는 두 개의 선을 그립니다
        Vector3 leftBoundary = DirFromAngle(-viewAngle / 2, false);
        Vector3 rightBoundary = DirFromAngle(viewAngle / 2, false);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);

        // 플레이어가 감지된 경우 시각적으로 표시
        Gizmos.color = Color.red;
        foreach (Transform player in players)
        {
            if (playerInViewDict.ContainsKey(player) && playerInViewDict[player])
            {
                Gizmos.DrawLine(transform.position, player.position);
            }
        }
    }

    // 각도를 통해 방향 벡터를 계산하는 함수
    private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
