using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private Vector3[] firstDoorRow;
    [SerializeField] private Vector3[] secondDoorRow;
    [SerializeField] private Vector3[] thirdDoorRow;
    [SerializeField] private Vector3[] fourthDoorRow;
    [SerializeField] private Vector3[] fifthDoorRow;
    [SerializeField] private Vector3 playerOneSpawnPosition;
    [SerializeField] private Vector3 playerTwoSpawnPosition;
    
    
    private void Start()
    {
        SpawnPlayers();
        if (SceneManager.GetActiveScene().name == "DoorDash")
        {
            DoorDashInitialize();
        }
        if (SceneManager.GetActiveScene().name == "BigFans")
        {
            BigsFansInitialize();
        }
    }
    
    private void SpawnPlayers()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            spawnPosition = playerOneSpawnPosition;
        }
        else
        {
            spawnPosition = playerTwoSpawnPosition;
        }
        var player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
        if (SceneManager.GetActiveScene().name == "DoorDash")
        {
            player.GetComponent<PlayerEventManager>().SetFinishLine(134f);
        }
    }

    private void DoorDashInitialize()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnDoor(firstDoorRow);
            SpawnDoor(secondDoorRow);
            SpawnDoor(thirdDoorRow);
            SpawnDoor(fourthDoorRow);
            SpawnDoor(fifthDoorRow);
        }
    }
    
    private void BigsFansInitialize()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("AllFans", Vector3.zero, Quaternion.identity);
        }
    }

    private void SpawnDoor(Vector3[] arr)
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
        for (var i = 0; i < arr.Length; i++)
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

    
    public void GoToMenu()
    {
        PhotonNetwork.LoadLevel("Menu");
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
    }
}