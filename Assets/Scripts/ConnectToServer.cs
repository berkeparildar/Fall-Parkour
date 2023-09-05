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
    private bool _joinedRoom;
    private bool _loadedGame;
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
        if (roomCount == 0)
        {
            PhotonNetwork.CreateRoom("random");
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
        if (!_loadedGame && _joinedRoom && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            _loadedGame = true;
            PhotonNetwork.LoadLevel("Game");
        }
    }

    public override void OnJoinedRoom()
    {
        _joinedRoom = true;
    }
}
