using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.Instantiate(player.name,new Vector3(10,21,-8),Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
