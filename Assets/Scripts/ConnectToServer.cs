using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    private bool _isConnected;
    public bool joinedRoom;
    public bool loadedGame;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        _isConnected = true;
       
        Debug.Log("Connected to Master");
    }

    public void QueueButton()
    {
        if (_isConnected)
        {
            PhotonNetwork.JoinLobby();
            Debug.Log("queue animation");

        }
    }

    public override void OnJoinedLobby()
    {
        var roomCount = PhotonNetwork.CountOfRooms;
        Debug.Log(roomCount);
        if (roomCount == 0)
        {
            RoomOptions opt = new RoomOptions();
            opt.MaxPlayers = 2;
            opt.IsOpen = true;
            PhotonNetwork.CreateRoom("random", opt);
            SpawnPlayers.IsFirstPlayer = true;
        }
        else
        {
            SpawnPlayers.IsFirstPlayer = false;
            PhotonNetwork.JoinRandomRoom();
        }
    }

    private void Update()
    {
        if (!loadedGame && joinedRoom && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            loadedGame = true;
            PhotonNetwork.LoadLevel("Game");
        }
    }

    public override void OnJoinedRoom()
    {
        joinedRoom = true;
    }
}
