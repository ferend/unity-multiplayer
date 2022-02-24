using System;
using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Network
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        public InputField roomNameInput;
        public InputField playerCountInput;
        public Text errMessage;
        public Text roomName;

        private Dictionary<string, RoomInfo> _roomListInfo;
        private Dictionary<string, GameObject> _roomListGameObjects;
        private Dictionary<int, GameObject> _playerListGameobjects;
        public GameObject roomlistPrefab;
        public Transform roomListTransform;
        public GameObject playerListPrefab;
        public Transform playerListTransform;
        public GameObject startButton;

        private void Awake()
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Connected using settings");
        }

        void Start()
        {
            Menu.Instance.OpenLoading();
            _roomListInfo = new Dictionary<string, RoomInfo>();
            _roomListGameObjects = new Dictionary<string, GameObject>();
            ClearRoomList();
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            PhotonNetwork.JoinLobby();
            Debug.Log("Connected to master ");
        }
        public override void OnJoinedLobby()
        {
            Debug.Log("Joined Lobby");
            Menu.Instance.OpenMenu();
            //PhotonNetwork.JoinOrCreateRoom("Room", roomOptions: roomOpts, TypedLobby.Default);
            Menu.Instance.header.text = "THE GAME";
            PhotonNetwork.NickName = "Player " + Random.Range(0, 100).ToString("0000");
            Debug.Log("Player name is + " + PhotonNetwork.NickName);
        }


        public void CreateRoom()
        {
         
            string roomname = roomNameInput.text;
            if (!string.IsNullOrEmpty(roomNameInput.text))
            {
                 RoomOptions roomOpts = new RoomOptions()
                 {
                     IsVisible = true,
                     IsOpen = true,
                     MaxPlayers = (byte)int.Parse(playerCountInput.text),
                     PublishUserId = true
                 };
                PhotonNetwork.CreateRoom(roomname,roomOpts);
                Menu.Instance.CloseMenu();
            }
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("Creating a new room with name : " + roomNameInput.text);

        }
        public override void OnJoinedRoom()
        {
            Menu.Instance.CloseMenu();
            Menu.Instance.OpenRoomMenu();
            
            if (_playerListGameobjects == null)
            {
                _playerListGameobjects = new Dictionary<int, GameObject>();
            }
            
            Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined to room name " + PhotonNetwork.CurrentRoom.Name);
            roomName.text = "Room Name : " + PhotonNetwork.CurrentRoom.Name + " Player / Max Players " +
                            PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
            
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                startButton.SetActive(true);
            }
            else
            {
                startButton.SetActive(false);
            }
            
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                GameObject playerListObject = Instantiate(playerListPrefab,playerListTransform);
                playerListObject.transform.localScale = Vector3.one;

                playerListObject.transform.Find("Text").GetComponent<Text>().text = player.NickName;
    
                if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    playerListObject.transform.Find("Text_1").gameObject.SetActive(true);
                }
                else
                {
                    playerListObject.transform.Find("Text_1").gameObject.SetActive(false);
                }
                _playerListGameobjects.Add(player.ActorNumber,playerListObject);
            }
        }
        
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            GameObject playerListObject = Instantiate(playerListPrefab,playerListTransform);
            playerListObject.transform.localScale = Vector3.one;

            playerListObject.transform.Find("Text").GetComponent<Text>().text = newPlayer.NickName;
    
            if (newPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                playerListObject.transform.Find("Text_1").gameObject.SetActive(true);
            }
            else
            {
                playerListObject.transform.Find("Text_1").gameObject.SetActive(false);
            }
            _playerListGameobjects.Add(newPlayer.ActorNumber,playerListObject);
            
            Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined to room name " + PhotonNetwork.CurrentRoom.Name);
            roomName.text = "Room Name : " + PhotonNetwork.CurrentRoom.Name + " Player / Max Players " +
                            PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        }
        
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            // add another menu comp
            Debug.Log("Create room failed ");
            errMessage.text = "Create room failed! Error type :" + message;
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Destroy(_playerListGameobjects[otherPlayer.ActorNumber].gameObject);
            _playerListGameobjects.Remove(otherPlayer.ActorNumber);
            Debug.Log(PhotonNetwork.LocalPlayer.NickName + " left the room  " + PhotonNetwork.CurrentRoom.Name);
            roomName.text = "Room Name : " + PhotonNetwork.CurrentRoom.Name + " Player / Max Players " +
                            PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                startButton.SetActive(true);
            }
        }
        
        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            Debug.Log("Leaving room : " + roomName.text);
            foreach (var player in _playerListGameobjects.Values)
            {
                Destroy(player);
            } 
            _playerListGameobjects.Clear();
            _playerListGameobjects = null;
        } 
        public void OnJoinRandomRoomButtonClicked()
        {
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("Joining to randon room");
            Menu.Instance.CloseMenu();
            Menu.Instance.OpenRoomMenu();

        }
        public void OnJoinRandomRoomFailed(short returnCode, string message)
        {
            Debug.Log(message);
            string roomName = "Room " + Random.Range(1000, 10000);
            RoomOptions roomOpts = new RoomOptions()
            {
                IsVisible = true,
                IsOpen = true,
                PublishUserId = true
            };
            PhotonNetwork.CreateRoom(roomName, roomOpts);
        }
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
          
            foreach (RoomInfo room in roomList)
            {
               Debug.Log("Room list updated new room name : " + room.Name);

               if ((!room.IsOpen || !room.IsVisible || room.RemovedFromList) && _roomListInfo.ContainsKey(room.Name))
               {
                   _roomListInfo.Remove(room.Name);
               }
               else
               {
                   // update room list in new entry
                   if (_roomListInfo.ContainsKey(room.Name))
                   {
                       _roomListInfo[room.Name] = room;
                   }
                   // add new room to room list
                   else
                   {
                       _roomListInfo.Add(room.Name,room);
                   }
               }
            }

            foreach (var room in _roomListInfo.Values)
            {
                var roomListObject = Instantiate(roomlistPrefab, roomListTransform, true);
                roomListObject.transform.localScale = new Vector3(1, 1, 1);
                roomListObject.transform.Find("Text").GetComponent<Text>().text = room.Name;
                roomListObject.GetComponent<Button>().onClick.AddListener((() => OnJoinRoomButtonClicked(room.Name)));
                
                _roomListGameObjects.Add(room.Name,roomListObject);
            }

        }

        public override void OnLeftLobby()
        {
            ClearRoomList();
            _roomListInfo.Clear();
            Debug.Log("Left the lobby");
        }
 
        void OnJoinRoomButtonClicked(string _roomName)
        {
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }
            PhotonNetwork.JoinRoom(_roomName);
        }

        void OnBackButtonClicked()
       {
           if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }

           if (PhotonNetwork.InRoom) 
           {
               PhotonNetwork.LeaveRoom();
           }
           Menu.Instance.CloseMenu();
            Menu.Instance.OpenMenu();
        }

        public void OnStartGameButtonClicked()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Game");
            }
        }
       
       void ClearRoomList()
        {  
            foreach (var roomlistGameObject in _roomListGameObjects.Values)
            {
                Destroy(roomlistGameObject);
            }
            _roomListGameObjects.Clear();
            
        }
    }
}

