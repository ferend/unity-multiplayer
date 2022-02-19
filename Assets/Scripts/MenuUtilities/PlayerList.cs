
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerList : MonoBehaviourPunCallbacks
{
   public Text playerText;
   private Player _player;

   public void SetupPlayer(Player player)
   {
      _player = player;
      playerText.text = player.NickName;
   }

   public override void OnPlayerLeftRoom(Player otherPlayer)
   {
      if (_player == otherPlayer)
      {
         Destroy(gameObject);
      }
   }

   public override void OnLeftRoom()
   {
      Destroy(gameObject);
   }
}
