using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float turnAngle = 30;
    [SerializeField] private GameObject model;
    [SerializeField] private Animator modelAnimator;
    [SerializeField] private Vector3 movementVector;
    [SerializeField] private PhotonView view;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rayLength = 0.6f;
    [SerializeField] private bool noMovement;
    [SerializeField] private AudioSource runAudio;
    [SerializeField] private AudioSource jumpAudio;
    private static readonly int VerticalInput = Animator.StringToHash("verticalInput");
    private static readonly int HorizontalInput = Animator.StringToHash("horizontalInput");
    private static readonly int Jump = Animator.StringToHash("jump");
    [SerializeField] private bool facing;
    [SerializeField] private GameObject vCamera;

    private void Start()
    {
        if (view.IsMine)
        {
            vCamera.GetComponent<CinemachineVirtualCamera>().enabled = true;
        }
    }

    private void Update()
    {
        if (view.IsMine && !noMovement)
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");
            if (!runAudio.isPlaying && (Mathf.Abs(verticalInput) > 0 || Mathf.Abs(horizontalInput) > 0) && OnGround())
            {
                runAudio.Play();
            }
            if (runAudio.isPlaying && (Mathf.Abs(verticalInput) == 0 && Mathf.Abs(horizontalInput) == 0) && !OnGround())
            {
                runAudio.Stop();
            }
            modelAnimator.SetFloat(VerticalInput, verticalInput);
            modelAnimator.SetFloat(HorizontalInput, horizontalInput);
            modelAnimator.SetBool(Jump, !OnGround());
            var horizontalVector = horizontalInput * vCamera.transform.right;
            var verticalVector = verticalInput * vCamera.transform.forward;
            movementVector = horizontalVector + verticalVector;
            movementVector.Normalize();
        }
        if (view.IsMine && !noMovement)
        {
            DemoMove();
        }
    }

    private void DemoMove()
    {
        var lookDirection = Vector3.zero;
        if (movementVector.x != 0 || movementVector.z != 0)
        {
            lookDirection = movementVector;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(lookDirection);
                model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRotation, turnAngle * Time
                    .deltaTime);
            }

            if (Vector3.Dot(model.transform.forward, lookDirection) > 0.9f)
            {
                facing = true;
            }
            else
            {
                facing = false;
            }
        }
        
        if (facing)
        {
            var moveDirection = movementVector;
            moveDirection *= speed;
            moveDirection.y = rb.velocity.y;
            rb.velocity = moveDirection;
        }
        else
        {
            var movement =  Vector3.zero;
            movement.y = rb.velocity.y;
            rb.velocity = movement;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (OnGround())
            {
                jumpAudio.Play();
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        
    }
    
    private bool OnGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, rayLength);
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