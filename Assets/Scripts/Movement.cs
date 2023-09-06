using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _speed = 5.0f;
    private PhotonView _view;
    private Vector3 currenJumpVelocity;
    private bool isJumping;
    private CharacterController _controller;
    void Start()
    {
        _view = GetComponent<PhotonView>();
        _rigidbody = GetComponent<Rigidbody>();
        _controller = GetComponent<CharacterController>();
        if (_view.IsMine)
        {
            transform.GetChild(0).GetComponent<CinemachineVirtualCamera>().enabled = true;
        }
    }
    
    private void Update()
    {
        if (_view.IsMine)
        {
            Move();
        }
    }

    private void Move()
    {
        var moveVelocity = Vector3.zero;
        moveVelocity.x = Input.GetAxis("Horizontal") * _speed;
        moveVelocity.z = Input.GetAxis("Vertical") * _speed;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJumping)
            {
                isJumping = true;
                currenJumpVelocity = Vector3.up * 5;
            }
        }

        if (isJumping)
        {
            _controller.Move((moveVelocity + currenJumpVelocity) * Time.deltaTime);
            currenJumpVelocity += Physics.gravity * Time.deltaTime;
            if (_controller.isGrounded)
            {
                isJumping = false;
            }
        }
        else
        {
            _controller.SimpleMove(moveVelocity);
        }
    }
    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_rigidbody.position);
            stream.SendNext(_rigidbody.rotation);
            stream.SendNext(_rigidbody.velocity);
        }
        else
        {
            _rigidbody.position = (Vector3) stream.ReceiveNext();
            _rigidbody.rotation = (Quaternion) stream.ReceiveNext();
            _rigidbody.velocity = (Vector3) stream.ReceiveNext();

            float lag = Mathf.Abs((float) (PhotonNetwork.Time - info.timestamp));
            _rigidbody.position += _rigidbody.velocity * lag;
        }
    }*/
}