using Cinemachine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private bool isJumping;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float turnAngle = 30;
    [SerializeField] private GameObject model;
    [SerializeField] private Animator modelAnimator; 
    //[SerializeField] private PhotonView view;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rayLength = 0.6f;
    [SerializeField] private bool justJumped;
    [SerializeField] private bool noMovement;
    private static readonly int VerticalInput = Animator.StringToHash("verticalInput");
    private static readonly int HorizontalInput = Animator.StringToHash("horizontalInput");
    private static readonly int Jump = Animator.StringToHash("jump");
    private static readonly int OnGround = Animator.StringToHash("onGround");

    private void Start()
    {
        noMovement = true;
        //view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        model = transform.GetChild(0).gameObject;
        model.SetActive(true);
        modelAnimator = model.GetComponent<Animator>();
        //if (view.IsMine)
        //{
            transform.GetChild(1).GetComponent<CinemachineVirtualCamera>().enabled = true;
        //}
    }
    
    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, Vector3.down * rayLength);
        //if (view.IsMine && !noMovement)
        //{
            Move();
        //}
    }
    
    private void Move()
    {
        var horizontalInput = Input.GetAxis("Horizontal") * speed;
        var verticalInput = Input.GetAxis("Vertical") * speed;
        modelAnimator.SetFloat(VerticalInput, verticalInput);
        modelAnimator.SetFloat(HorizontalInput, horizontalInput);
        var moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        if (moveDirection.magnitude > 5)
        {
            moveDirection.Normalize();
            moveDirection *= 5;
        }

        moveDirection.y = rb.velocity.y;
        rb.velocity = moveDirection;
        transform.Rotate(Vector3.up, turnAngle * Input.GetAxis("Horizontal") * Time.deltaTime);
        if (Physics.Raycast(transform.position, Vector3.down, rayLength))
        {
            isJumping = false;
            modelAnimator.SetBool(OnGround, !isJumping);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJumping)
            {
                isJumping = true;
                justJumped = true;
                modelAnimator.SetBool(Jump, justJumped);
                justJumped = false;
                modelAnimator.SetBool(Jump, justJumped);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    public bool GetMovementStatus()
    {
        return noMovement;
    }
    
    public void SetMovementStatus(bool status)
    {
        noMovement = status;
    }
}