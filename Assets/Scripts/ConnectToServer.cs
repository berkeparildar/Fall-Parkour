using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    private bool _isConnected;
    public bool joinedRoom;
    public bool loadedGame;
    public TextMeshProUGUI master;
    public TextMeshProUGUI roomcount;
    public TextMeshProUGUI room;
    public TextMeshProUGUI joined;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        _isConnected = true;
        PhotonNetwork.JoinLobby();
        master.text = ("Connected to Master");
        Debug.Log("Connected to Master");
    }

    public void QueueButton()
    {
        if (_isConnected && !joinedRoom)
        {
            var roomCount = PhotonNetwork.CountOfRooms;
            roomcount.text = ("There are " + roomCount + " available rooms");
            Debug.Log("There are " + roomCount + " available rooms");
            if (roomCount == 0)
            {
                room.text = ("No rooms, so creating a room");
                Debug.Log("No rooms, so creating a room");
                PhotonNetwork.CreateRoom("random");
            }
            else
            {
                room.text = ("There is a room, joining to it.");
                Debug.Log("There is a room, joining to it.");
                PhotonNetwork.JoinRandomRoom();
            }
        }
    }

    public override void OnJoinedLobby()
    {
    }

    private void Update()
    {
        if (!loadedGame && joinedRoom && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            loadedGame = true;
            PhotonNetwork.LoadLevel("DoorDash");
        }
    }

    public override void OnJoinedRoom()
    {
        joined.text = ("Joined a room");
        Debug.Log("Joined a room");
        joinedRoom = true;
    }
}
