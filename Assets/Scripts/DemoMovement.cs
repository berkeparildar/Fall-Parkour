using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DemoMovement : MonoBehaviour
{
     private Rigidbody _rigidbody;
        private float _speed = 5.0f;
        private Vector3 currenJumpVelocity;
        private bool isJumping;
        private CharacterController _controller;
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _controller = GetComponent<CharacterController>();
        }
        
        private void Update()
        {
            
            Move();
            
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
}