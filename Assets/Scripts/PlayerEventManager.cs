using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerEventManager : MonoBehaviour, IOnEventCallback
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator canvasAnimator;
    [SerializeField] private Animator modelAnimator;
    [SerializeField] private Movement movement;
    [SerializeField] private PhotonView view;
    [SerializeField] private float finishLine;
    private static readonly int Win = Animator.StringToHash("win");
    private static readonly int Lose = Animator.StringToHash("lose");

    private void Start()
    {
        canvasAnimator = GameObject.Find("Canvas").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfFinished();
    }
    
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 2)
        {
            if (view.IsMine)
            {
                if (transform.position.z >= finishLine)
                {
                    canvasAnimator.SetTrigger(Win);
                    rb.isKinematic = true;
                    modelAnimator.enabled = false;
                }
                else
                {
                    movement.SetMovementStatus(true);
                    Debug.Log("You Lost!!");
                    canvasAnimator.SetTrigger(Lose);
                    rb.isKinematic = true;
                    modelAnimator.enabled = false;
                }
            }
        }
        else if (photonEvent.Code == 9)
        {
            if (view.IsMine)
            {
                var content = photonEvent.CustomData;
                if (view.ViewID == (int)content)
                {
                    movement.SetMovementStatus(true);
                    Debug.Log("You Lost!!");
                    canvasAnimator.SetTrigger(Lose);
                    rb.isKinematic = true;
                    modelAnimator.enabled = false;
                }
                else
                {
                    canvasAnimator.SetTrigger(Win);
                    rb.isKinematic = true;
                    modelAnimator.enabled = false;
                }
            }
        }
    }
    
    void CheckIfFinished()
    {
        if (movement.GetMovementStatus() == false && transform.position.z >= finishLine)
        {
            movement.SetMovementStatus(true);
            byte eventCode = 2;
            RaiseEventOptions eventOptions = RaiseEventOptions.Default;
            eventOptions.Receivers = ReceiverGroup.All;
            SendOptions sendOptions = SendOptions.SendReliable;
            PhotonNetwork.RaiseEvent(eventCode, null, eventOptions, sendOptions);
        }
    }

    public void SetFinishLine(float number)
    {
        finishLine = number;
    }
}
