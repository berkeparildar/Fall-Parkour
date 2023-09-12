
using Photon.Pun;
using UnityEngine;

public class CheckpointSpawn : MonoBehaviour
{
    [SerializeField] private Vector3[] spawnPositionsRight;
    [SerializeField] private Vector3[] checkPointPositionsRight;
    [SerializeField] private Vector3[] spawnPositionsLeft;
    [SerializeField] private Vector3[] checkPointPositionsLeft;
    [SerializeField] private int currentIndex;
    
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
            }
        }
    }

    private void CheckFall(Vector3[] arr)
    {
        var yPos = transform.position.y;
        if (yPos <= -10)
        {
            transform.position = arr[currentIndex];
        }
    }
}
