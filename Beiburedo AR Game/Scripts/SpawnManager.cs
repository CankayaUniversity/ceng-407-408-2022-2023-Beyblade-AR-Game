using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPunCallbacks
{

    public GameObject[] playerPrefabs;
    public Transform[] spawnPositions;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region Photon Callback Methods
    public override void OnJoinedRoom(){

        if(PhotonNetwork.IsConnectedAndReady){

            object playerSelectionNumber;
            if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerARSpinnerTopGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber)){
                Debug.Log("Player selection number is " + (int)playerSelectionNumber);

                int randomSpawnPoint = Random.Range(0,spawnPositions.Length -1 );
                Vector3 instantiatePosition = spawnPositions[randomSpawnPoint].position;

                PhotonNetwork.Instantiate(playerPrefabs[(int)playerSelectionNumber].name, instantiatePosition, Quaternion.identity);
            }
        }

        
    }

    #endregion

}
