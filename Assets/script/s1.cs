using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
public class s1 : MonoBehaviourPunCallbacks
{
    public GameObject connectedscreen;
    public InputField input;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void connectbtn()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        print("connect to server ");
        PhotonNetwork.JoinLobby();
        connectedscreen.SetActive(true);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected to server....! "+cause);
    }
    public override void OnJoinedLobby()
    {
        print("join lobby");
    }
    public override void OnLeftLobby()
    {
        print("left in lobby....!");
    }
    public void createrommbtnclick()
    {
        PhotonNetwork.CreateRoom(input.text,new RoomOptions { MaxPlayers=2,IsVisible=true,IsOpen=true});
    }
    public override void OnCreatedRoom()
    {
        print("room created ");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("room not created ! " + message);
    }
    public void joinbtnclick()
    {
        PhotonNetwork.JoinRoom(input.text);
    }
    public override void OnJoinedRoom()
    {
        print("join in room ");
        if(PhotonNetwork.CountOfPlayersInRooms==0)
        {
            PhotonNetwork.NickName = "player A";
        }
        else
        {
            PhotonNetwork.NickName = "player B";
        }
        PhotonNetwork.LoadLevel("PLAY");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("join room failed...!");
    }

}
