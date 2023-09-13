using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class SurfaceBasedMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float maxVelocityDelta;
    [SerializeField] private float turnSpeed;
    [SerializeField] private GameObject model;
    [SerializeField] private Animator modelAnimator;
    [SerializeField] private PhotonView view;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rayLength = 0.6f;
    [SerializeField] private bool noMovement;
    private static readonly int VerticalInput = Animator.StringToHash("verticalInput");
    private static readonly int HorizontalInput = Animator.StringToHash("horizontalInput");
    private static readonly int Jump = Animator.StringToHash("jump");
    [SerializeField] private bool facing;
    [SerializeField] private Vector3 movementVector;
    [SerializeField] private GameObject vCamera;
    [SerializeField] private AudioSource runAudio;
    [SerializeField] private AudioSource jumpAudio;

    private void Start()
    {
        vCamera = transform.GetChild(1).gameObject;
        if (view.IsMine)
        {
            vCamera.GetComponent<CinemachineVirtualCamera>().enabled = true;
        }
    }

    private void FixedUpdate()
    {
        if (view.IsMine && !noMovement)
        {
            Move();
        }
    }

    private void Update()
    {
        if (view.IsMine && !noMovement)
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");
            if (!runAudio.isPlaying && (Mathf.Abs(verticalInput) > 0 || Mathf.Abs(horizontalInput) > 0))
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
    }

    private void Move()
    {
        var lookDirection = Vector3.zero;
        if (movementVector.x != 0 || movementVector.z != 0)
        {
            lookDirection = movementVector;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(lookDirection);
                model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRotation, turnSpeed * Time
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
            var targetVelocity = movementVector;
            targetVelocity *= speed;
            var currentVelocity = rb.velocity;
            if (targetVelocity.magnitude < currentVelocity.magnitude)
            {
                targetVelocity = currentVelocity;
                rb.velocity /= 1.1f;
            }

            var velocityDelta = targetVelocity - currentVelocity;
            velocityDelta.x = Mathf.Clamp(velocityDelta.x, -maxVelocityDelta, maxVelocityDelta);
            velocityDelta.z = Mathf.Clamp(velocityDelta.z, -maxVelocityDelta, maxVelocityDelta);
            velocityDelta.y = 0;
            if (!IsSlippery())
            {
                if (Mathf.Abs(rb.velocity.magnitude) < speed)
                {
                    rb.AddForce(velocityDelta, ForceMode.VelocityChange);
                }
            }
            else if (Mathf.Abs(rb.velocity.magnitude) < speed)
            {
                rb.AddForce(velocityDelta * 0.01f, ForceMode.VelocityChange);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (OnGround())
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private bool OnGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, rayLength);
    }

    private bool IsSlippery()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength))
        {
            if (hit.transform.CompareTag("Slippery"))
            {
                return true;
            }
        }
        return false;
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
