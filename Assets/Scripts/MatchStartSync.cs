using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MatchStartSync : MonoBehaviour, IOnEventCallback
{
    [SerializeField] private GameObject cooldownCanvas;
    [SerializeField] private TimelineAsset timelineAsset;
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private bool sentEvent;
    [SerializeField] private GameObject countDownPrefab;
    [SerializeField] private Animator countDownAnimator;
    [SerializeField] private bool animatorSet;
    [SerializeField] private bool initializedCountdown;
    [SerializeField] private int readyPlayers;
    [SerializeField] private bool stopped;
    [SerializeField] private bool start;
    
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        readyPlayers = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckTimeLine();
        CheckReadyPlayers();
        CheckCountDownFinish();
        CountDownFinish();
    }

    private void CheckTimeLine()
    {
        if (playableDirector.state != PlayState.Playing && !stopped)
        {
            stopped = true;
            byte eventCode = 6;
            var eventOptions = RaiseEventOptions.Default;
            eventOptions.Receivers = ReceiverGroup.All;
            var sendOptions = SendOptions.SendReliable;
            PhotonNetwork.RaiseEvent(eventCode, null, eventOptions, sendOptions);
        }
    }

    private void CheckReadyPlayers()
    {
        if (readyPlayers != 2 || initializedCountdown) return;
        initializedCountdown = true;
        var createdCanvas = Instantiate(countDownPrefab, Vector3.zero, Quaternion.identity);
        countDownAnimator = createdCanvas.transform.GetChild(0).GetComponent<Animator>();
        animatorSet = true;
    }

    private void CountDownFinish()
    {
        if (readyPlayers != 4 || start) return;
        start = true;
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            player.GetComponent<Movement>().SetMovementStatus(false);
        }
    }

    private void CheckCountDownFinish()
    {
        if (animatorSet && countDownAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f && !sentEvent)
        {
            sentEvent = true;
            byte eventCode = 6;
            RaiseEventOptions eventOptions = RaiseEventOptions.Default;
            eventOptions.Receivers = ReceiverGroup.All;
            SendOptions sendOptions = SendOptions.SendReliable;
            PhotonNetwork.RaiseEvent(eventCode, null, eventOptions, sendOptions);
        }
    }
    
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 6)
        {
            readyPlayers++;
        }
    }
}
