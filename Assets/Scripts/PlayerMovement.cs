using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Vector2 input;
    [SerializeField] Vector3 moveDirection;

    float verticalInput;
    float horizontalInput;
    [SerializeField] float jumpSpeed = 5;
    [SerializeField] float jumpHeight;
    [Header("Speed Setting")]
    [SerializeField] float currentMoveSpeed;
    [SerializeField] float runSpeed = 8;
    [SerializeField] float walkSpeed = 5;
    [SerializeField] float accelerationSpeed = 5;
    [SerializeField] float decelerationSpeed = 10;
    [SerializeField] float fallSpeed;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] float verticalMove;
    [SerializeField] float horizontalMove;

    [Header("Ground Check")]
    [SerializeField] bool isGrounded;
    [SerializeField] Transform footTransform;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckDistance = 0.5f;
    [SerializeField] float groundCheckRadius = 0.5f;

    [Header("Head Check")]
    [SerializeField] Transform headTransform;
    [SerializeField] LayerMask headHitLayer;
    [SerializeField] float headCheckDistance = 0.5f;

    //[Space]
    //[SerializeField] float angleCheckDist = 1f;
    //[SerializeField] float groundDot;
    
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        currentMoveSpeed = walkSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(footTransform.position, groundCheckRadius);
        //Ray ray = new Ray(footTransform.position, -transform.up * groundCheckDistance);
        //Gizmos.DrawSphere(ray.GetPoint(groundCheckDistance), groundCheckRadius);
        
    }

    bool IsGrounded()
    {
        //Ray ray = new Ray(footTransform.position, transform.forward);
        //if (Physics.Raycast(ray, out RaycastHit hit, angleCheckDist, groundLayer))
        //{

        //    if (isGrounded)
        //    {
        //        groundDot = Vector3.Dot(transform.forward, hit.normal);

        //    }
        //}



        if (Physics.CheckSphere(footTransform.position, groundCheckRadius, groundLayer))
        {
            if (isGrounded)
            {
                fallSpeed = Mathf.MoveTowards(fallSpeed,0, gravity * Time.deltaTime);
            }
            isGrounded = true;
            return true;

        }




        Ray _headRay = new Ray(headTransform.position, transform.up);
        Debug.DrawRay(_headRay.origin, _headRay.direction * headCheckDistance, Color.red);
        if (Physics.Raycast(_headRay, headCheckDistance, headHitLayer))
        {
            if (fallSpeed > 0)
            {
                fallSpeed = 0;
            }
        }
        fallSpeed -= gravity * Time.deltaTime;
        isGrounded = false;

        return false;
    }

    // Update is called once per frame
    void Update()
    {

        IsGrounded();
        GetInputs();
        CalculateSpeed();
        MovePlayer();




    }
    private void MovePlayer()
    {
        moveDirection = transform.forward * verticalMove + transform.right * horizontalMove;
        moveDirection.y = fallSpeed;
        rb.linearVelocity = moveDirection;
    }

    private void GetInputs()
    {
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");
        input = new Vector2(verticalInput, horizontalInput).normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentMoveSpeed = runSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentMoveSpeed = walkSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //jumpHeight = gravity
            fallSpeed = jumpSpeed;
        }
    }

    private void CalculateSpeed()
    {
        //Vertical
        if (verticalInput != 0)
        {
            if (verticalInput * verticalMove > 0)
            {
                verticalMove = Mathf.MoveTowards(verticalMove, input.x * currentMoveSpeed, Time.deltaTime * accelerationSpeed);
            }
            else
            {
                verticalMove = Mathf.MoveTowards(verticalMove, input.x * currentMoveSpeed, Time.deltaTime * decelerationSpeed);
            }
        }
        else
        {
            verticalMove = Mathf.MoveTowards(verticalMove, 0, Time.deltaTime * decelerationSpeed);
        }
        //Horizontal
        if (horizontalInput != 0)
        {
            if (horizontalInput * horizontalMove > 0)
            {
                horizontalMove = Mathf.MoveTowards(horizontalMove, input.y * currentMoveSpeed, Time.deltaTime * accelerationSpeed);
            }
            else
            {
                horizontalMove = Mathf.MoveTowards(horizontalMove, input.y * currentMoveSpeed, Time.deltaTime * decelerationSpeed);
            }
        }
        else
        {
            horizontalMove = Mathf.MoveTowards(horizontalMove, 0, Time.deltaTime * decelerationSpeed);
        }


    }
}
