using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    private bool _isConnected;
    public bool joinedRoom;
    public bool loadedGame;
    private readonly string[] _levels = new[] { "DoorDash", "BigFans", "SlimeClimb" };
    [SerializeField] private Animator menuAnimator;
    [SerializeField] private Animator canvasAnimator;
    private static readonly int Jump = Animator.StringToHash("jump");
    [SerializeField] private bool onQueue;
    [SerializeField] private GameObject background;
    [SerializeField] private Button playButton;
    private static readonly int Queue1 = Animator.StringToHash("queue");

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        _isConnected = true;
        PhotonNetwork.JoinLobby();
    }

    public void QueueButton()
    {
        if (_isConnected && !joinedRoom)
        {
            menuAnimator.SetTrigger(Jump);
            canvasAnimator.SetTrigger(Queue1);
            StartCoroutine(QueueCoroutine());
        }
    }

    private void Update()
    {
        if (onQueue)
        {
            var currentOffset = background.GetComponent<Renderer>().material.GetTextureOffset("_MainTex");
            currentOffset.y += 2 * Time.deltaTime;
            background.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", currentOffset);
        }
        if (!loadedGame && joinedRoom && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            loadedGame = true;
            //PhotonNetwork.LoadLevel(PhotonNetwork.CurrentRoom.Name);
            PhotonNetwork.LoadLevel(PhotonNetwork.CurrentRoom.Name);
        }
    }

    public override void OnJoinedLobby()
    {
        playButton.interactable = true;
    }

    public override void OnJoinedRoom()
    {
        joinedRoom = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    private IEnumerator QueueCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        onQueue = true;
        yield return new WaitForSeconds(2);
        var roomCount = PhotonNetwork.CountOfRooms;
        Debug.Log("There are " + roomCount + " available rooms");
        if (roomCount == 0)
        {
            Debug.Log("No rooms, so creating a room");
            var random = Random.Range(0, _levels.Length);
            Debug.Log(random);
            PhotonNetwork.CreateRoom(roomName: _levels[random]);
        }
        else
        {
            Debug.Log("There is a room, joining to it.");
            PhotonNetwork.JoinRandomRoom();
        }

        yield return null;
    }
}
