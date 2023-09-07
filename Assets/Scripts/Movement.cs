using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class Movement : MonoBehaviour
{
    private PhotonView _view;
    private Rigidbody _rigidbody;
    public float _speed = 5f;
    public float jumpForce = 5f;
    private bool isGrounded;
    private float _verticalInput;
    private Animator _animator;
    private static readonly int VerticalInput = Animator.StringToHash("verticalInput");

    void Start()
    {
        _view = GetComponent<PhotonView>();
        _rigidbody = GetComponent<Rigidbody>();
        var childCount = transform.GetChild(0).childCount;
        var modelChoice = Random.Range(0, childCount);
        var model = transform.GetChild(0).GetChild(modelChoice).gameObject;
        model.SetActive(true);
        _animator = model.GetComponent<Animator>();
        if (_view.IsMine)
        {
            transform.GetChild(1).GetComponent<CinemachineVirtualCamera>().enabled = true;
        }
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * 0.6f, Color.red);
        _animator.SetFloat(VerticalInput, _verticalInput);
        _animator.SetBool("jump", !isGrounded);
    }

    private void FixedUpdate()
    {
        if (_view.IsMine)
        {
            Move();
        }
    }

    private void Move()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.6f);
        _verticalInput = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");
        var movementVector = new Vector3(horizontal * _speed, _rigidbody.velocity.y, _verticalInput * _speed);
        _rigidbody.velocity = movementVector;
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}