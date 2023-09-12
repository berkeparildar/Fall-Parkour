using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeObstacle : MonoBehaviour
{
    [SerializeField] private Vector3 currentPos;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        currentPos = transform.position;
        targetPos = new Vector3(currentPos.x, currentPos.y, currentPos.z - 5f);
        StartCoroutine(MoveForward());
    }
    

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator MoveForward()
    {
        while (Mathf.Abs(transform.position.z - targetPos.z) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(MoveBackwards());
    }
    
    private IEnumerator MoveBackwards()
    {
        while (Mathf.Abs(transform.position.z - currentPos.z) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentPos, speed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(MoveForward());
    }
}
