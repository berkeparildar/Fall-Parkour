using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    public static bool IsFirstPlayer;
    public GameObject playerPrefab;

    private Vector3 _spawnPos;
    // Start is called before the first frame update
    void Start()
    {
        if (IsFirstPlayer)
        {
            _spawnPos = new Vector3(10, 2, 2);
        }
        else
        {
            _spawnPos = new Vector3(0, 2, 2);
        }
        PhotonNetwork.Instantiate(playerPrefab.name, _spawnPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
