using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatController : MonoBehaviourPunCallbacks
{
    private Animator animator;  // 애니메이터
    public Transform rightHand;  // 캐릭터의 손 위치
    public Transform leftHand;  // 캐릭터의 손 위치
    public Transform jumphand;  // 캐릭터의 손 위치
    public List<Transform> players;  // 플레이어 참조

    // 컷신용
    public GameObject bomb; // 캐릭터 팔에 연결된 공
    public GameObject bigBomb; // 캐릭터 팔에 연결된 공
    public GameObject ball; // 캐릭터 팔에 연결된 공
    public GameObject redBomb; // 캐릭터 팔에 연결된 공
    public GameObject player1;  // 플레이어 참조
    public GameObject player2;  // 플레이어 참조
    public AudioSource jumpSound;

    // 쓰러질 때 생성할 목적지
    public GameObject Goal;

    private float cnt = 0f;
    public int phase;
    private int randomNumber; // 랜덤 숫자
    private bool isDying = false; // Die1 애니메이션 실행 중 여부
    private int damage = 0;
    private float originalSpeed;

    // Start는 첫 프레임 업데이트 전에 호출됩니다.
    void Start()
    {
        // Animator 컴포넌트 가져오기
        animator = GetComponent<Animator>();
        originalSpeed = animator.speed;
        StartCoroutine(PlayRandomAnimation());
    }

    private void Update()
    {
        SetAnimationSpeed();

        if (damage >= 3)
        {
            animator.Play("Die2");
            //yield return new WaitUntil(() => IsAnimationFinished(7)); // '5'로 설정하여 Die1 애니메이션 확인
            isDying = true; // 애니메이션 실행 상태 초기화

            Goal.SetActive(true);
        }
    }

    private IEnumerator PlayRandomAnimation()
    {
        while (!isDying && PhotonNetwork.LocalPlayer.IsMasterClient) // 살아 있는 한 무한 루프
        {
            if (phase == 1)
            {
                randomNumber = Random.Range(1, 11);
                switch (randomNumber)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        photonView.RPC("AnimationPlay", RpcTarget.AllBuffered, "Victory");
                        break;
                    case 6:
                        photonView.RPC("AnimationPlay", RpcTarget.AllBuffered, "rightBomb");
                        break;
                    case 7:
                        photonView.RPC("AnimationPlay", RpcTarget.AllBuffered, "rightBall");
                        break;
                    case 8:
                        photonView.RPC("AnimationPlay", RpcTarget.AllBuffered, "leftBomb");
                        break;
                    case 9:
                        photonView.RPC("AnimationPlay", RpcTarget.AllBuffered, "leftBall");
                        break;
                    case 10:
                        photonView.RPC("AnimationPlay", RpcTarget.AllBuffered, "BigBomb");
                        break;
                }
            }
            else if (phase == 2)
            {
                randomNumber = Random.Range(11, 21);
                switch (randomNumber)
                {
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                        photonView.RPC("AnimationPlay", RpcTarget.AllBuffered, "Victory");
                        break;
                    case 15:
                    case 16:
                    case 17:
                        photonView.RPC("AnimationPlay", RpcTarget.AllBuffered, "rightBomb");
                        break;
                    case 18:
                    case 19:
                    case 20:
                        photonView.RPC("AnimationPlay", RpcTarget.AllBuffered, "leftBomb");
                        break;
                }
            }
            else
            {
                randomNumber = Random.Range(21, 31);
                switch (randomNumber)
                {
                    case 21:
                    case 22:
                        photonView.RPC("AnimationPlay", RpcTarget.AllBuffered, "Victory");
                        break;
                    case 23:
                    case 24:
                    case 25:
                    case 26:
                    case 27:
                    case 28:
                        photonView.RPC("AnimationPlay", RpcTarget.AllBuffered, "BigBomb");
                        break;
                    case 29:
                        photonView.RPC("AnimationPlay", RpcTarget.AllBuffered, "rightRed");
                        break;
                    case 30:
                        photonView.RPC("AnimationPlay", RpcTarget.AllBuffered, "leftRed");
                        break;
                }
            }

            // 현재 재생 중인 애니메이션이 끝날 때까지 대기
            yield return new WaitUntil(() => IsAnimationFinished(randomNumber));
        }
    }

    [PunRPC]
    public void AnimationPlay(string motionName)
    {
        animator.Play(motionName);
    }

    private void SetAnimationSpeed()
    {
        switch (phase)
        {
            case 1: // 페이즈 1
                animator.speed = originalSpeed; // 원래 속도
                break;
            case 2: // 페이즈 2
                animator.speed = originalSpeed * 1.3f; // 속도를 1.3배로 증가
                break;
            case 3: // 페이즈 3
                animator.speed = originalSpeed * 1.6f; // 속도를 1.6배로 증가
                break;
            default:
                animator.speed = originalSpeed; // 기본 속도로 설정
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDying)
        {
            if (other.CompareTag("Shelf"))
            {
                animator.Play("Die1");
                isDying = true;

                Goal.SetActive(true);

                //BoxCollider shelfCollider = other.GetComponent<BoxCollider>();
                //if (shelfCollider != null)
                //{
                //    shelfCollider.center = new Vector3(0, 0, 0);
                //}
                StartCoroutine(WaitForDieAnimationToEnd());
            }

            else if (other.CompareTag("Stone")) 
            {
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    damage++;
                    photonView.RPC("SyncDamage", RpcTarget.AllBuffered, damage);

                    if(damage <3)
                    {
                        animator.Play("Damage");

                        isDying = true; // Damage 애니메이션 실행 상태로 변경
                        StartCoroutine(WaitForDamageAnimationToEnd());
                    }
                }
            }
        }
    }

    [PunRPC]
    public void SyncDamage(int newDamage)
    {
        damage = newDamage;
    }

    private IEnumerator WaitForDamageAnimationToEnd()
    {
        // Damage 애니메이션이 끝날 때까지 대기
        yield return new WaitUntil(() => IsAnimationFinished(31));

        Debug.Log("Damage 애니메이션 종료");
        isDying = false; // Damage 애니메이션이 끝나면 다시 초기화
        StartCoroutine(PlayRandomAnimation());
        // Damage 애니메이션이 끝난 후, PlayRandomAnimation 루프 재시작
        
    }



    private IEnumerator WaitForDieAnimationToEnd()
    {
        // Die1 애니메이션이 끝날 때까지 대기
        yield return new WaitUntil(() => IsAnimationFinished(32)); // '5'로 설정하여 Die1 애니메이션 확인

        Debug.Log("Die1 애니메이션 종료");
        isDying = false; // 애니메이션 실행 상태 초기화
    }

    private bool IsAnimationFinished(int animationIndex)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        switch (animationIndex)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 11:
            case 12:
            case 13:
            case 14:
            case 21:
            case 22:
                return stateInfo.IsName("Victory") && stateInfo.normalizedTime >= 1f;
            case 6:
            case 15:
            case 16:
            case 17:
                return stateInfo.IsName("rightBomb") && stateInfo.normalizedTime >= 1f;
            case 7:
                return stateInfo.IsName("rightBall") && stateInfo.normalizedTime >= 1f;
            case 8:
            case 18:
            case 19:
            case 20:
                return stateInfo.IsName("leftBomb") && stateInfo.normalizedTime >= 1f;
            case 9:
                return stateInfo.IsName("leftBall") && stateInfo.normalizedTime >= 1f;
            case 10:
            case 23:
            case 24:
            case 25:
            case 26:
            case 27:
            case 28:
                return stateInfo.IsName("BigBomb") && stateInfo.normalizedTime >= 1f;
            case 29:
                return stateInfo.IsName("rightRed") && stateInfo.normalizedTime >= 1f;
            case 30:
                return stateInfo.IsName("leftRed") && stateInfo.normalizedTime >= 1f;
            case 31:
                return stateInfo.IsName("Damage") && stateInfo.normalizedTime >= 1f;
            case 32:
                return stateInfo.IsName("Die1") && stateInfo.normalizedTime >= 1f;
            default:
                return false;
        }
    }


    void BombThrow(string dir)
    {
        Debug.Log(dir);

        // 컷신용 코드
        if (SceneManager.GetActiveScene().name == "CutScene4")
        {
            if (dir == "left")
            {
                // 발사체 생성
                GameObject projectile = Instantiate(bomb, leftHand.position, leftHand.rotation);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                // 발사 방향 계산
                if (cnt % 2 == 0)
                {
                    Vector3 launchDirection = CalculateLaunchDirection(leftHand.position, player2.transform.position, 20f);
                    rb.velocity = launchDirection;
                }
                else
                {
                    Vector3 launchDirection = CalculateLaunchDirection(leftHand.position, player1.transform.position, 20f);
                    rb.velocity = launchDirection;
                }

                cnt++;
            }
            else
            {
                // 발사체 생성
                GameObject projectile = Instantiate(bomb, rightHand.position, rightHand.rotation);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                // 발사 방향 계산
                if (cnt % 2 == 0)
                {
                    Vector3 launchDirection = CalculateLaunchDirection(rightHand.position, player2.transform.position, 20f);
                    rb.velocity = launchDirection;
                }
                else
                {
                    Vector3 launchDirection = CalculateLaunchDirection(rightHand.position, player1.transform.position, 20f);
                    rb.velocity = launchDirection;
                }

                cnt++;
            }
        }
        
        // 게임 코드
        else if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (dir == "left")
            {
                // 발사체 생성
                GameObject projectile = PhotonNetwork.Instantiate("Boss/Bomb", leftHand.position, leftHand.rotation);
                BombController bombController = projectile.GetComponent<BombController>();
                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                // 발사 방향 계산
                if (cnt % 2 == 0)
                {
                    Vector3 launchDirection = CalculateLaunchDirection(leftHand.position, players[1].position, 20f);
                    rb.velocity = launchDirection;
                }
                else
                {
                    Vector3 launchDirection = CalculateLaunchDirection(leftHand.position, players[0].position, 20f);
                    rb.velocity = launchDirection;
                }

                cnt++;

                // 4초 후에 발사체를 제거하고 폭발 효과를 해당 위치에 생성
                bombController.StartDestroyCountdown(4f);
            }
            else
            {
                // 발사체 생성
                GameObject projectile = PhotonNetwork.Instantiate("Boss/Bomb", rightHand.position, rightHand.rotation);
                BombController bombController = projectile.GetComponent<BombController>();
                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                // 발사 방향 계산
                if (cnt % 2 == 0)
                {
                    Vector3 launchDirection = CalculateLaunchDirection(rightHand.position, players[1].position, 20f);
                    rb.velocity = launchDirection;
                }
                else
                {
                    Vector3 launchDirection = CalculateLaunchDirection(rightHand.position, players[0].position, 20f);
                    rb.velocity = launchDirection;
                }

                cnt++;

                // 4초 후에 발사체를 제거하고 폭발 효과를 해당 위치에 생성
                bombController.StartDestroyCountdown(4f);
            }
        }
    }

    void RedThrow(string dir)
    {
        // 컷신용 코드
        if (SceneManager.GetActiveScene().name == "CutScene4")
        {
            float randomValue;

            // 1.0f ~ 2.0f 또는 3.0f ~ 4.0f 범위에서 랜덤 값 선택
            if (Random.value < 0.5f)
            {
                // 1.0f ~ 2.0f 사이의 랜덤 값
                randomValue = Random.Range(0.6f, 0.8f);
            }
            else
            {
                // 3.0f ~ 4.0f 사이의 랜덤 값
                randomValue = Random.Range(1.2f, 1.4f);
            }
            if (dir == "left")
            {
                // 발사체 생성
                GameObject projectile = Instantiate(redBomb, leftHand.position, leftHand.rotation);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                // 발사 방향 계산
                if (cnt % 2 == 0)
                {
                    Vector3 launchDirection = CalculateLaunchDirection(leftHand.position, player2.transform.position, 20f);
                    rb.velocity = launchDirection * randomValue;
                }
                else
                {
                    Vector3 launchDirection = CalculateLaunchDirection(leftHand.position, player1.transform.position, 20f);
                    rb.velocity = launchDirection * randomValue;
                }

                cnt++;
            }
            else
            {
                // 발사체 생성
                GameObject projectile = Instantiate(redBomb, rightHand.position, rightHand.rotation);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                // 발사 방향 계산
                if (cnt % 2 == 0)
                {
                    Vector3 launchDirection = CalculateLaunchDirection(rightHand.position, player2.transform.position, 20f);
                    rb.velocity = launchDirection * randomValue;
                }
                else
                {
                    Vector3 launchDirection = CalculateLaunchDirection(rightHand.position, player1.transform.position, 20f);
                    rb.velocity = launchDirection * randomValue;
                }

                cnt++;
            }
        }

        // 게임용 코드
        else if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (dir == "left")
            {
                // 발사체 생성
                GameObject projectile = PhotonNetwork.Instantiate("Boss/RedBomb", leftHand.position, leftHand.rotation);
                BombController bombController = projectile.GetComponent<BombController>();
                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                // 발사 방향 계산
                if (cnt % 2 == 0)
                {
                    Vector3 launchDirection = CalculateLaunchDirection(leftHand.position, players[1].position, 20f);
                    rb.velocity = launchDirection;
                }
                else
                {
                    Vector3 launchDirection = CalculateLaunchDirection(leftHand.position, players[0].position, 20f);
                    rb.velocity = launchDirection;
                }

                cnt++;

                // 4초 후에 발사체를 제거하고 폭발 효과를 해당 위치에 생성
                bombController.StartDestroyCountdown(20f);
            }
            else
            {
                // 발사체 생성
                GameObject projectile = PhotonNetwork.Instantiate("Boss/RedBomb", rightHand.position, rightHand.rotation);
                BombController bombController = projectile.GetComponent<BombController>();
                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                // 발사 방향 계산
                if (cnt % 2 == 0)
                {
                    Vector3 launchDirection = CalculateLaunchDirection(rightHand.position, players[1].position, 20f);
                    rb.velocity = launchDirection;
                }
                else
                {
                    Vector3 launchDirection = CalculateLaunchDirection(rightHand.position, players[0].position, 20f);
                    rb.velocity = launchDirection;
                }

                cnt++;

                // 4초 후에 발사체를 제거하고 폭발 효과를 해당 위치에 생성
                bombController.StartDestroyCountdown(20f);
            }
        }
    }

    void BallThrow(string dir)
    {
        Debug.Log(dir);

        // 컷신용 코드
        if (SceneManager.GetActiveScene().name == "CutScene4")
        {
            if (dir == "left")
            {
                // 발사체 생성
                GameObject projectile = Instantiate(ball, leftHand.position, leftHand.rotation);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                // 발사 방향 계산
                if (cnt % 2 == 0)
                {
                    Vector3 launchDirection = CalculateLaunchDirection(leftHand.position, player2.transform.position, 20f);
                    rb.velocity = launchDirection * 1.2f;

                }
                else
                {
                    Vector3 launchDirection = CalculateLaunchDirection(leftHand.position, player1.transform.position, 20f);
                    rb.velocity = launchDirection * 1.2f;
                }

                cnt++;
            }
            else
            {
                // 발사체 생성
                GameObject projectile = Instantiate(ball, rightHand.position, rightHand.rotation);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                // 발사 방향 계산
                if (cnt % 2 == 0)
                {
                    Vector3 launchDirection = CalculateLaunchDirection(rightHand.position, player2.transform.position, 20f);
                    rb.velocity = launchDirection * 1.2f;
                }
                else
                {
                    Vector3 launchDirection = CalculateLaunchDirection(rightHand.position, player1.transform.position, 20f);
                    rb.velocity = launchDirection * 1.2f;
                }

                cnt++;
            }
        }

        // 게임용 코드
        else if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (dir == "left")
            {
                // 발사체 생성
                GameObject projectile = PhotonNetwork.Instantiate("Boss/Ball", leftHand.position, leftHand.rotation);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                // 발사 방향 계산
                if (cnt % 2 == 0)
                {
                    Vector3 launchDirection = CalculateLaunchDirection(leftHand.position, players[1].position, 20f);
                    rb.velocity = launchDirection * 1.2f;

                }
                else
                {
                    Vector3 launchDirection = CalculateLaunchDirection(leftHand.position, players[0].position, 20f);
                    rb.velocity = launchDirection * 1.2f;
                }

                cnt++;

                photonView.RPC("FadeOutAndDestroyRPC",
                    RpcTarget.AllBuffered,
                    projectile.GetComponent<PhotonView>().ViewID);
            }
            else
            {
                // 발사체 생성
                GameObject projectile = PhotonNetwork.Instantiate("Boss/Ball", rightHand.position, rightHand.rotation);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                // 발사 방향 계산
                if (cnt % 2 == 0)
                {
                    Vector3 launchDirection = CalculateLaunchDirection(rightHand.position, players[1].position, 20f);
                    rb.velocity = launchDirection * 1.2f;
                }
                else
                {
                    Vector3 launchDirection = CalculateLaunchDirection(rightHand.position, players[0].position, 20f);
                    rb.velocity = launchDirection * 1.2f;
                }

                cnt++;

                photonView.RPC("FadeOutAndDestroyRPC", 
                    RpcTarget.AllBuffered, 
                    projectile.GetComponent<PhotonView>().ViewID);
            }
        }
    }

    void BigThrow()
    {
        jumpSound.Play();
        // 컷신용 코드
        if (SceneManager.GetActiveScene().name == "CutScene4")
        {
            // 발사체 생성
            GameObject projectile = Instantiate(bigBomb, jumphand.position, jumphand.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            // 발사 방향 계산
            if (cnt % 2 == 0)
            {
                Vector3 launchDirection = CalculateLaunchDirection(jumphand.position, player2.transform.position, 20f);
                rb.velocity = launchDirection;
            }
            else
            {
                Vector3 launchDirection = CalculateLaunchDirection(jumphand.position, player1.transform.position, 20f);
                rb.velocity = launchDirection;
            }

            cnt++;
        }

        // 게임용 코드
        else if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            // 발사체 생성
            GameObject projectile = PhotonNetwork.Instantiate("Boss/BigBomb", jumphand.position, jumphand.rotation);
            BombController bombController = projectile.GetComponent<BombController>();
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            // 발사 방향 계산
            if (cnt % 2 == 0)
            {
                Vector3 launchDirection = CalculateLaunchDirection(jumphand.position, players[1].position, 20f);
                rb.velocity = launchDirection;

                if (phase == 3)
                {
                    rb.velocity *= 1.1f;
                }
            }
            else
            {
                Vector3 launchDirection = CalculateLaunchDirection(jumphand.position, players[0].position, 20f);
                rb.velocity = launchDirection;

                if(phase == 3)
                {
                    rb.velocity *= 1.1f;
                }
            }

            cnt++;

            bombController.StartDestroyCountdown(4f);
        }
    }

    [PunRPC]
    public void FadeOutAndDestroyRPC(int ballId)
    {
        PhotonView ballPhoton = PhotonView.Find(ballId);

        if (ballPhoton != null)
        {
            GameObject projectile = ballPhoton.gameObject;

            StartCoroutine(FadeOutAndDestroy(projectile, 20f, 1f));
        }
    }

    private IEnumerator FadeOutAndDestroy(GameObject projectile, float delay, float fadeDuration)
    {
        // 5초 대기
        yield return new WaitForSeconds(delay);

        Renderer renderer = projectile.GetComponent<Renderer>();
        Material material = renderer.material;
        Color initialColor = material.color;

        // Alpha 값을 서서히 줄이기
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            material.color = new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.Lerp(initialColor.a, 0f, normalizedTime));
            yield return null; // 다음 프레임까지 대기
        }

        // 완전히 사라진 후 삭제
        Destroy(projectile);
    }

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
        if (phase == 2)
        {

            Vector3 finalVelocity
                = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
            return finalVelocity;
        }
        else
        {
            Vector3 finalVelocity
               = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity * 0.9f;
            return finalVelocity;
        }
    }
}
