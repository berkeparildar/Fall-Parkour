using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private bool game;
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    
    void Start()
    {
        targetPos = transform.position;
        targetPos.y += 300;
        game = true;
    }

    void Update()
    {
        if (game)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos,  0.2f *Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        //Raise event here......
        byte eventCode = 9;
        RaiseEventOptions eventOptions = RaiseEventOptions.Default;
        eventOptions.Receivers = ReceiverGroup.All;
        SendOptions sendOptions = SendOptions.SendReliable;
        PhotonNetwork.RaiseEvent(eventCode, other.GetComponent<PhotonView>().ViewID, eventOptions, sendOptions);
        GetComponent<BoxCollider>().enabled = false;
    }

    public void StopSlime()
    {
        game = false;
        targetPos = transform.position;
        
    }
}
