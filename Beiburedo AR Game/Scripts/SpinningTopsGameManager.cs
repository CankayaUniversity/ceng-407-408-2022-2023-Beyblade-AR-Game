using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class SpinningTopsGameManager : MonoBehaviourPunCallbacks
{

    [Header("UI")]
    public GameObject uI_InformPanelGameObject;
    public TextMeshProUGUI uI_InformText;
    public GameObject searchForGamesButtonGameObject;


    // Start is called before the first frame update
    void Start()
    {
        uI_InformPanelGameObject.SetActive(true);
        uI_InformText.text = "Search For Games to Battle!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    #region UI Callback Methods
    public void JoinRandomRoom(){

        uI_InformText.text = "Searching for available rooms...";

        PhotonNetwork.JoinRandomRoom();

        searchForGamesButtonGameObject.SetActive(false);
    }

    #endregion

    #region PHOTON Callback Methods

        public override void OnJoinRandomFailed(short returnCode, string message){
            CreateAndJoinRoom();
        }

        public override void OnJoinedRoom(){
            if(PhotonNetwork.CurrentRoom.PlayerCount == 1){
                uI_InformText.text = "Joined to "+ PhotonNetwork.CurrentRoom.Name + ". Waiting for other players...";
            }
            else {
                uI_InformText.text = "Joined to "+ PhotonNetwork.CurrentRoom.Name;
                StartCoroutine(DeactivateAfterSeconds(uI_InformPanelGameObject, 2.0f));
            }
            


            Debug.Log(PhotonNetwork.NickName + "joined to " + PhotonNetwork.CurrentRoom.Name);
        }


        public override void OnPlayerEnteredRoom(Player newPlayer){
            Debug.Log(newPlayer.NickName + "joined to " + PhotonNetwork.CurrentRoom.Name + "Player count" + PhotonNetwork.CurrentRoom.PlayerCount);

            uI_InformText.text = newPlayer.NickName + "joined to " + PhotonNetwork.CurrentRoom.Name + "Player count" + PhotonNetwork.CurrentRoom.PlayerCount;

            StartCoroutine(DeactivateAfterSeconds(uI_InformPanelGameObject, 2.0f));
        }



    #endregion


    #region 

    void CreateAndJoinRoom(){

        string randomRoomName = "Room" + Random.Range(0,1000);

        RoomOptions RoomOptions = new RoomOptions();
        RoomOptions.MaxPlayers = 2;

        //creating room
        PhotonNetwork.CreateRoom(randomRoomName,RoomOptions);

    }

    IEnumerator DeactivateAfterSeconds(GameObject _gameObject, float _seconds){
        yield return new WaitForSeconds(_seconds);
        _gameObject.SetActive(false);
    }


    #endregion
}
