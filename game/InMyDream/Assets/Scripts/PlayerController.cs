using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;   // 이동 속도
    public float jumpForce = 10f;   // 점프 힘
    private bool isJumping = false; // 점프 중인지 확인하는 변수

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        // WASD 키로 이동
        float moveX = Input.GetAxis("Horizontal") * moveSpeed;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed;

        Vector3 movement = new Vector3(moveX, 0, moveZ);
        rb.MovePosition(transform.position + movement * Time.deltaTime);
    }

    void Jump()
    {
        // 스페이스바로 점프, 점프 중일 때는 다시 점프 불가
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 땅에 닿으면 다시 점프 가능
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}
