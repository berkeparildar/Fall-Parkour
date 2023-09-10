using System;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private bool inContact;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!inContact)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down,  out hit, rayLength,  obstacleLayer))
            {
                transform.SetParent(hit.transform);
            }
            else
            {
                transform.SetParent(null);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Fan"))
        {
            inContact = true;
            transform.SetParent(null);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Fan"))
        {
            inContact = true;
            transform.SetParent(null);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        // Check if the collision is with the fan object.
        if (other.gameObject.CompareTag("Fan"))
        {
            inContact = false;
        }
    }
}
