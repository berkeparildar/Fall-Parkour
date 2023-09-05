using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private float _speed = 5.0f;

    private PhotonView _view;
    // Start is called before the first frame update
    void Start()
    {
        _view = GetComponent<PhotonView>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var movementVector = new Vector3(horizontal * _speed, _rigidbody.velocity.y, vertical * _speed);
        _rigidbody.velocity = movementVector;
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
