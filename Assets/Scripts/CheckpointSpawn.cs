
using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class CheckpointSpawn : MonoBehaviour
{
    [SerializeField] private Vector3[] spawnPositionsRight;
    [SerializeField] private Vector3[] checkPointPositionsRight;
    [SerializeField] private Vector3[] spawnPositionsLeft;
    [SerializeField] private Vector3[] checkPointPositionsLeft;
    [SerializeField] private int currentIndex;
    [SerializeField] private GameObject checkPointUI;
    [SerializeField] private bool uiActivated;
    [SerializeField] private AudioSource checkPointSound;
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            UpdateIndex(checkPointPositionsLeft);
            CheckFall(spawnPositionsLeft);
        }
        else
        {
            UpdateIndex(checkPointPositionsRight);
            CheckFall(spawnPositionsRight);
        }
    }

    private void UpdateIndex(Vector3[] arr)
    {
        var zPos = transform.position.z;
        for (int i = currentIndex; i < arr.Length; i++)
        {
            if (zPos >= arr[i].z)
            {
                currentIndex = i;
                if (!uiActivated)
                {
                    uiActivated = true;
                }
            }
        }
    }

    private IEnumerator CheckPointUI()
    {
        checkPointUI.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        checkPointUI.SetActive(false);
        uiActivated = false;
    }

    private void CheckFall(Vector3[] arr)
    {
        var yPos = transform.position.y;
        if (yPos <= -10)
        {
            transform.position = arr[currentIndex];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CheckPoint"))
        {
            StartCoroutine(CheckPointUI());
            checkPointSound.Play();
        }
    }
}
