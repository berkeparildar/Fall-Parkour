using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;

public class DoorSpawner : MonoBehaviour
{
    public Vector3[] firstRow;
    public Vector3[] secondRow;
    public Vector3[] thirdRow;
    public Vector3[] fourthRow;
    public Vector3[] fifthRow;
    [SerializeField] private GameObject cooldownCanvas;
    [SerializeField] private TimelineAsset timelineAsset;
    [SerializeField] private PlayableDirector playableDirector;
    private bool _sentEvent;
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

    private void Update()
    {
        if (!_sentEvent)
        {
            CheckCooldown();
        }
    }
    
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    

    private void CheckCooldown()
    {
        if (cooldownCanvas.activeSelf && cooldownCanvas.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime
            >= 0.99f)
        {
            _sentEvent = true;
            byte eventCode = 10;
            RaiseEventOptions eventOptions = RaiseEventOptions.Default;
            SendOptions sendOptions = SendOptions.SendReliable;
            PhotonNetwork.RaiseEvent(eventCode, null, eventOptions, sendOptions);
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
