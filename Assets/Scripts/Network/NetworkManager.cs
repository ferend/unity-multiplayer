using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public static NetworkManager Instance;
    public SpawnPoint[] spawnPoints;
    
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);

    }

    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            var randomSpawnPoints = GetSpawnPoint();
            PhotonNetwork.Instantiate(playerPrefab.name,randomSpawnPoints.position,Quaternion.identity);
        }
        
    }

    public Transform GetSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
    }
}
