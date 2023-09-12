using System;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

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
    [SerializeField] private bool facing;

    private void Start()
    {
        //if (view.IsMine)
        //{
            transform.GetChild(1).GetComponent<CinemachineVirtualCamera>().enabled = true;
        //}
    }

    private void Update()
    {
        //if (view.IsMine && !noMovement)
        //{
            DemoMove();
        //}
    }

    private void DemoMove()
    {
        var horizontalInput = Input.GetAxis("Horizontal") * speed;
        var verticalInput = Input.GetAxis("Vertical") * speed;
        modelAnimator.SetFloat(VerticalInput, verticalInput);
        modelAnimator.SetFloat(HorizontalInput, horizontalInput);
        var lookDirection = Vector3.zero;
        if (verticalInput >= 0.1f)
        {
            lookDirection = transform.GetChild(1).forward;
        }
        else if (verticalInput <= -0.1f)
        {
            lookDirection = -transform.GetChild(1).forward;
        }
        else if (horizontalInput >= 0.1f)
        {
            lookDirection = transform.GetChild(1).right;
        }
        else if (horizontalInput <= -0.1f)
        {
            lookDirection = -transform.GetChild(1).right;
        }
        
        lookDirection.y = 0f;
        
        if (lookDirection != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(lookDirection);
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRotation, 5 * Time
                .deltaTime);
        }
        
        if (Vector3.Dot(model.transform.forward, lookDirection) > 0.8f)
        {
            facing = true;
        }
        else
        {
            facing = false;
        }
        
        if (facing)
        {
            var moveDirection = transform.GetChild(1).forward * verticalInput + transform.GetChild(1).right * horizontalInput;
            if (moveDirection.magnitude > 5)
            {
                moveDirection.Normalize();
                moveDirection *= 5;
            }
            moveDirection.y = rb.velocity.y;
            rb.velocity = moveDirection;
        }
        else
        {
            var movement =  Vector3.zero;
            movement.y = rb.velocity.y;
            rb.velocity = movement;
        }
        
        if (Physics.Raycast(transform.position, Vector3.down, rayLength))
        {
            isJumping = false;
            modelAnimator.SetBool(Jump, isJumping);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJumping)
            {
                isJumping = true;
                modelAnimator.SetBool(Jump, isJumping);
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