using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DoorSpawner : MonoBehaviour
{
    public Vector3[] firstRow;
    public Vector3[] secondRow;
    public Vector3[] thirdRow;
    public Vector3[] fourthRow;
    public Vector3[] fifthRow;
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnDoors(firstRow);
            SpawnDoors(secondRow);
            SpawnDoors(thirdRow);
            SpawnDoors(fourthRow);
            SpawnDoors(fifthRow);
        }
    }

    void SpawnDoors(Vector3[] arr)
    {
        var randomNoOne = Random.Range(0, arr.Length);
        var randomNoTwo = Random.Range(0, arr.Length);
        if (randomNoOne == randomNoTwo)
        {
            if (randomNoOne != arr.Length - 1)
            {
                randomNoTwo = randomNoOne + 1;
            }
            else
            {
                randomNoTwo = randomNoOne - 1;
            }
        }
        Debug.Log(randomNoOne);
        for (int i = 0; i < arr.Length; i++)
        {
            if (i == randomNoOne || i == randomNoTwo)
            {
                PhotonNetwork.Instantiate("DoorKinematic", arr[i], Quaternion.identity);
            }
            else
            {
                var door = PhotonNetwork.Instantiate("Door", arr[i], Quaternion.identity);
            }
        }
    }
}
