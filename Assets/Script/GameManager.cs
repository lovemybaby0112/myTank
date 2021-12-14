using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    public static GameObject localPlayer;
    string gameVersion = "1";
    // Start is called before the first frame update
    private void Awake()
    {
        //遊戲管理是唯一的，如果已經存在，就刪除新創的
        if (instance != null)
        {
            Debug.LogErrorFormat(gameObject, "Multiple instances of {0} is not allow", GetType().Name);
            DestroyImmediate(gameObject);
            return;
        }


        PhotonNetwork.AutomaticallySyncScene = true; //https://www.jianshu.com/p/fcef97c79a54
        DontDestroyOnLoad(gameObject); //不要刪掉這個物件，讓這個管理一直存在，因Unity場景載入會刪掉所有東西
        instance = this;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public override void OnConnected()
    {
        Debug.Log("PUN Connected");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Connected to Master");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Disconnected was called by PUN with reason {0}", cause);
    }
    // Update is called once per frame
    void Update()
    {
            
    }
    public void JoinGameRoom()
    {
        var options = new RoomOptions
        {
            MaxPlayers = 6
        };
        PhotonNetwork.JoinOrCreateRoom("Kingdom", options, null);
    }
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Created room!!");
            PhotonNetwork.LoadLevel("GameScene");
        }
        else
        {
            Debug.Log("Joined room!!");
        }
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarningFormat("Joined room failed :{0}", message);

    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!PhotonNetwork.InRoom) return;

        localPlayer = PhotonNetwork.Instantiate("TankPlayer", new Vector3(0, 0, 0), Quaternion.identity, 0);
        Debug.Log("Player Instance ID:" + localPlayer.GetInstanceID());
    }
}
