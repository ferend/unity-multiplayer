using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public float maxHealth = 100F;
        public float currentHealth;
        public PhotonView pv;
   

        void Start()
        {
            currentHealth = maxHealth;
        }

        public void PlayerDeath()
        {
            if (!pv.IsMine) return;
            Debug.Log("You died");
            StartCoroutine(Respawn());
        }

        IEnumerator Respawn()
        {
            float respawnTime = 3.0f;
            while (respawnTime > 0.0f)
            {
                yield return new WaitForSeconds(1.0f);
                respawnTime -= 1.0f;
                transform.GetComponent<FirstPersonController>().enabled = false;
                // respawn text set active
                //healthbar
            }

            var playerTransform = NetworkManager.Instance.GetSpawnPoint().position;
            transform.SetPositionAndRotation(playerTransform,Quaternion.identity);
            transform.GetComponent<FirstPersonController>().enabled = true;
            pv.RPC(nameof(RPC_ResetPlayerStats),RpcTarget.AllBuffered);
        }

        [PunRPC]
        public void RPC_ResetPlayerStats()
        {
            currentHealth = maxHealth;
        }
    }

}
