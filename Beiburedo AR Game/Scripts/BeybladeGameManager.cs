using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class BeybladeGameManager : MonoBehaviourPunCallbacks
{
    [Header("UI")] 
    public GameObject uI_InformPanelGameObject;
    public TextMeshProUGUI uI_InformText;
    public GameObject SearchForGamesButtonGameObject;
    public GameObject adjust_Button;
    public GameObject raycastCenter_Image;
    
    
    
    #region UNITY Methods

    // Start is called before the first frame update
    void Start()
    {
        uI_InformPanelGameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    
    #region UI Callback Methods

    public void JoinRandomRoom() // The 1st method when user clicks the "search for games" button
    {
        uI_InformText.text = "Searching for available rooms...";
        
        /*
         * When this JoinRandomRoom method is called, Photon will try to find a room to join.
         * If there is no room, Photon will react to this situation with a callback method.
         */
        
        PhotonNetwork.JoinRandomRoom();
        
        SearchForGamesButtonGameObject.SetActive(false);
        
    }

    public void OnQuitMatchButtonClicked()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneLoader.Instance.LoadScene("Scene_Lobby");
        }
        
        
        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region PHOTON Callback Methods

    public override void OnJoinRandomFailed(short returnCode, string message) // Failed to join
    {
        Debug.Log(message);
        uI_InformText.text = message;
        CreateAndJoinRoom();
    }

    public override void OnJoinedRoom() // Successfully joined
    {
        adjust_Button.SetActive(false);
        raycastCenter_Image.SetActive(false);
        
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            uI_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name + ". Waiting for other players...";
        }
        else
        {
            uI_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name;
            StartCoroutine(DeactivateAfterSeconds(uI_InformPanelGameObject, 2f));
        }
        
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
    }

    
    // This method is called when a remote player joins the room that another player is in
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " 
                + PhotonNetwork.CurrentRoom.Name + " Player count " 
                + PhotonNetwork.CurrentRoom.PlayerCount);
        
        

        uI_InformText.text = newPlayer.NickName + " joined to "
                                    + PhotonNetwork.CurrentRoom.Name + " Player count "
                                    + PhotonNetwork.CurrentRoom.PlayerCount;
        
        

        StartCoroutine(DeactivateAfterSeconds(uI_InformPanelGameObject, 2f));
    }

    public override void OnLeftRoom()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }

    #endregion

    #region PRIVATE Methods

    void CreateAndJoinRoom() // Room creation
    {
        string randomRoomName = "Room" + Random.Range(0, 1000);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2; // Max player to play at one room

        // Creating the room
        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }

    IEnumerator DeactivateAfterSeconds(GameObject _gameObject, float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        _gameObject.SetActive(false);
    }
    

    #endregion
}
