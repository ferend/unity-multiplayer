using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Network
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public InputField roomNameInput;
        public Text errMessage;
        public Text roomName;
        public Transform roomlist;
        public GameObject roomlistPrefab;
        public Transform playerlist;
        public GameObject playerlistPrefab;

        
        private static NetworkManager _instance;

        public static NetworkManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<NetworkManager>();
                    if (_instance == null)
                    {
                        GameObject singletonGameObject = new GameObject("NetworkManager");
                        _instance = singletonGameObject.AddComponent<NetworkManager>();
                    }
                }

                return _instance;
            }
        }

        void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Connected to master server");
            Menu.Instance.OpenLoading();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
            base.OnConnectedToMaster();
        }
        
        public override void OnJoinedLobby()
        {
            // RoomOptions roomOpts = new RoomOptions()
            // {
            //     IsVisible = true,
            //     IsOpen = true,
            //     MaxPlayers = 8,
            //     PublishUserId = true
            // };

            Debug.Log("Joined Lobby");
            Menu.Instance.OpenMenu();
            //PhotonNetwork.JoinOrCreateRoom("Room", roomOptions: roomOpts, TypedLobby.Default);
            Menu.Instance.header.text = "THE GAME";
            PhotonNetwork.NickName = "Player " + Random.Range(0, 100).ToString("0000");
            Debug.Log("Player name is + " + PhotonNetwork.NickName);
        }

        public void CreateRoom()
        {
            if (!string.IsNullOrEmpty(roomNameInput.text))
            {
                PhotonNetwork.CreateRoom(roomNameInput.text);
                Debug.Log("Creating a new room with name : " + roomNameInput.text);
                Menu.Instance.CloseMenu();
            }
        }

        public override void OnJoinedRoom()
        {
            Menu.Instance.OpenRoomMenu();
            roomName.text = PhotonNetwork.CurrentRoom.Name;
            
            Player[] players = PhotonNetwork.PlayerList;
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Instantiate(playerlistPrefab,playerlist).GetComponent<PlayerList>().SetupPlayer(players[i]);

            }
        }

        public void JoinRoom(RoomInfo info)
        {
            PhotonNetwork.JoinRoom(info.Name);
            Debug.Log("Network : joninig room with name : " + info.Name);
   
        }
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            Debug.Log("Leaving room : " + roomName.text);
            Menu.Instance.OpenRoomMenu();
        }

        public override void OnLeftRoom()
        {
            Menu.Instance.CloseMenu();
            Menu.Instance.OpenLoading();
        } 

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            // add another menu comp
            Debug.Log("Create room failed ");
            errMessage.text = "Create room failed! Error type :" + message;
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (Transform trans in roomlist)
            {
                Destroy(trans.gameObject);
            }
            for (int i = 0; i < roomList.Count; i++)
            {
                Instantiate(roomlistPrefab , roomlist).GetComponent<RoomList>().Setup(roomList[i]);
                Debug.Log("Photon network on room list update " + " Create room list ");
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Instantiate(playerlistPrefab,playerlist).GetComponent<PlayerList>().SetupPlayer(newPlayer);
        }
    }
}

