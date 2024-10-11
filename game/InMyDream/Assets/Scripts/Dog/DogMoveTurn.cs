using UnityEngine;

public class DogMoveTurn : MonoBehaviour
{
    public float speed = 1f;
    public float rotationSpeed = 35f;
    public float directionChangeInterval = 3f;
    public float idleTime = 3f;
    private float directionTimer;
    private float idleTimer = 0f;
    private Vector3 moveDirection;
    private Rigidbody rb;
    private Animator animator;

    private bool isRotating = false;
    private bool isIdle = true;
    private float targetRotationAngle;
    private int currentDirection = -1;

    private int currentAnimationState = -1;  // Track current animation state

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        directionTimer = directionChangeInterval;
        animator.SetInteger("moveDirection", -1);
        SetIdleState();
    }

    void Update()
    {
        if (isIdle)
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0f)
            {
                ChooseNewDirection();
            }
        }
        else if (isRotating)
        {
            RotateDog();
        }
        else
        {
            MoveDog();
            directionTimer -= Time.deltaTime;
            if (directionTimer <= 0f)
            {
                SetIdleState();
                directionTimer = directionChangeInterval;
            }
        }
    }

    void MoveDog()
    {
        rb.MovePosition(transform.position + moveDirection * speed * Time.deltaTime);
    }

    void RotateDog()
    {
        float rotationStep = rotationSpeed * Time.deltaTime;
        float currentAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotationAngle, rotationStep);
        transform.eulerAngles = new Vector3(0, currentAngle, 0);

        if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetRotationAngle)) < 0.1f)
        {
            isRotating = false;
            SetIdleState();
        }
    }

    void SetIdleState()
    {
        isIdle = true;
        SetAnimatorState(-1);  // Set to idle 
        idleTimer = idleTime;
        Debug.Log("상태변경 - 정지");
    }

    void ChooseNewDirection()
    {
        isIdle = false;
        int direction = Random.Range(0, 3);

        if (direction == 0)
        {
            moveDirection = transform.forward;
            SetAnimatorState(0);  // Set to move forward animation
            Debug.Log("상태변경 - 직진");
        }
        else
        {
            currentDirection = direction;
            isRotating = true;

            float angleOffset = (direction == 1) ? -90f : 90f;
            targetRotationAngle = (transform.eulerAngles.y + angleOffset) % 360f;

            if (direction == 1)
            {
                SetAnimatorState(1);  // Set to rotate left animation
                Debug.Log("상태변경 - 좌회전");
            }
            else if (direction == 2)
            {
                SetAnimatorState(2);  // Set to rotate right animation
                Debug.Log("상태변경 - 우회전");
            }
        }
    }

    // Method to update the animator state only if the state changes
    void SetAnimatorState(int newState)
    {
        if (currentAnimationState != newState)
        {
            animator.SetInteger("moveDirection", newState);
            currentAnimationState = newState;
        }
    }
}
