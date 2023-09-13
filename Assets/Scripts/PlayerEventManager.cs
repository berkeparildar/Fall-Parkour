using Cinemachine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEventManager : MonoBehaviour, IOnEventCallback
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator canvasAnimator;
    [SerializeField] private Animator modelAnimator;
    [SerializeField] private Movement movement;
    [SerializeField] private SurfaceBasedMovement surfaceMovement;
    [SerializeField] private PhotonView view;
    [SerializeField] private CinemachineVirtualCamera vCamera;
    [SerializeField] private Slime slime;
    [SerializeField] private float finishLine;
    [SerializeField] private AudioSource endSound;
    private static readonly int Win = Animator.StringToHash("win");
    private static readonly int Lose = Animator.StringToHash("lose");
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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
                Cursor.lockState = CursorLockMode.None;
                vCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_MaxSpeed = 0;
                if (transform.position.z >= finishLine)
                {
                    if (!endSound.isPlaying)
                    {
                        endSound.clip = winSound;
                        endSound.Play();
                    }
                    canvasAnimator.SetTrigger(Win);
                    rb.isKinematic = true;
                    modelAnimator.enabled = false;
                }
                else
                {
                    if (!endSound.isPlaying)
                    {
                        endSound.clip = loseSound;
                        endSound.Play();
                    }

                    if (SceneManager.GetActiveScene().name == "SlimeClimb")
                    {
                        surfaceMovement.SetMovementStatus(true);
                    }
                    else
                    {
                        movement.SetMovementStatus(true);
                    }
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
                slime = GameObject.Find("Slime(Clone)").GetComponent<Slime>();
                slime.StopSlime();
                Cursor.lockState = CursorLockMode.None;
                vCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_MaxSpeed = 0;
                var content = photonEvent.CustomData;
                if (view.ViewID == (int)content)
                {
                    if (!endSound.isPlaying)
                    {
                        endSound.clip = loseSound;
                        endSound.Play();
                    }
                    surfaceMovement.SetMovementStatus(true);
                    Debug.Log("You Lost!!");
                    canvasAnimator.SetTrigger(Lose);
                    rb.isKinematic = true;
                    modelAnimator.enabled = false;
                }
                else
                {
                    if (!endSound.isPlaying)
                    {
                        endSound.clip = winSound;
                        endSound.Play();
                    }
                    canvasAnimator.SetTrigger(Win);
                    rb.isKinematic = true;
                    modelAnimator.enabled = false;
                }
            }
        }
    }
    
    void CheckIfFinished()
    {
        var movementStatus = false;
        if (SceneManager.GetActiveScene().name == "SlimeClimb")
        {
            movementStatus = surfaceMovement.GetMovementStatus();
        }
        else
        {
            movementStatus = movement.GetMovementStatus();
        }
        if (movementStatus == false && transform.position.z >= finishLine)
        {
            if (SceneManager.GetActiveScene().name == "SlimeClimb")
            {
                surfaceMovement.SetMovementStatus(true);
            }
            else
            {
                movement.SetMovementStatus(true);
            }
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
