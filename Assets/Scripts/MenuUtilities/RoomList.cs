
using Network;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
public class RoomList : MonoBehaviourPunCallbacks
{
  public Text text;
  private RoomInfo _info;

  public void Setup(RoomInfo info)
  {
    _info = info;
    text.text = _info.Name;
    Debug.Log("Setup: " + " room info is " + text.text);
  }

  public void OnClick()
  {
    NetworkManager.Instance.JoinRoom(_info);
  }
  
}

