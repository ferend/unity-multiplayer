using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Network
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        public InputField roomNameInput;
        public InputField playerCountInput;
        public Text errMessage;
        public Text roomName;
        private static Launcher _instance;

        private Dictionary<string, RoomInfo> roomListInfo;
        private Dictionary<string, GameObject> roomListGameObjects;
        public GameObject roomlistPrefab;
        public Transform roomListTransform;

        public static Launcher Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<Launcher>();
                    if (_instance == null)
                    {
                        GameObject singletonGameObject = new GameObject("NetworkManager");
                        _instance = singletonGameObject.AddComponent<Launcher>();
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
            roomListInfo = new Dictionary<string, RoomInfo>();
            roomListGameObjects = new Dictionary<string, GameObject>();
            ClearRoomList();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
            base.OnConnectedToMaster();
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
                Debug.Log("Creating a new room with name : " + roomNameInput.text);
                Menu.Instance.CloseMenu();
            }
        }

        public override void OnCreatedRoom()
        {
            
        }
        public override void OnJoinedRoom()
        {
            Menu.Instance.CloseMenu();
            Menu.Instance.OpenRoomMenu();
            Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined to room name " + PhotonNetwork.CurrentRoom.Name);
        }
        
        public override void OnLeftRoom()
        {
            Debug.Log("Leaving room : " + roomName.text);
            Menu.Instance.CloseMenu();
            Menu.Instance.OpenMenu();
        } 
        

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            // add another menu comp
            Debug.Log("Create room failed ");
            errMessage.text = "Create room failed! Error type :" + message;
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
          
            foreach (RoomInfo room in roomList)
            {
               Debug.Log("Room list updated new room name : " + room.Name);

               if ((!room.IsOpen || !room.IsVisible || room.RemovedFromList) && roomListInfo.ContainsKey(room.Name))
               {
                   roomListInfo.Remove(room.Name);
               }
               else
               {
                   // update room list in new entry
                   if (roomListInfo.ContainsKey(room.Name))
                   {
                       roomListInfo[room.Name] = room;
                   }
                   // add new room to room list
                   else
                   {
                       roomListInfo.Add(room.Name,room);
                   }
               }
            }

            foreach (RoomInfo room in roomListInfo.Values)
            {
                GameObject roomListObject = Instantiate(roomlistPrefab, roomListTransform, true);
                roomListObject.transform.localScale = new Vector3(1, 1, 1);
                roomListObject.transform.Find("Text").GetComponent<Text>().text = room.Name;
                roomListObject.GetComponent<Button>().onClick.AddListener((() => OnJoinRoomButtonClicked(room.Name)));
                
                roomListGameObjects.Add(room.Name,roomListObject);
            }

        }

        public override void OnLeftLobby()
        {
            ClearRoomList();
            roomListInfo.Clear();
            Debug.Log("Left the lobby ");
        }

        void OnJoinRoomButtonClicked(string _roomName)
        {
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }
            PhotonNetwork.JoinRoom(_roomName);
        }

       public void OnBackButtonClicked()
        {
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }
            Menu.Instance.CloseMenu();
            Menu.Instance.OpenMenu();
        }

        void ClearRoomList()
        {  
            foreach (var roomlistGameObject in roomListGameObjects.Values)
            {
                Destroy(roomlistGameObject);
            }
            roomListGameObjects.Clear();
            
        }
    }
}

