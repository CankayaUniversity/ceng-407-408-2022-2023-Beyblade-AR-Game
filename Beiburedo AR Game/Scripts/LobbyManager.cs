/*
 * This script will be responsible for Lobby operations like connecting to servers
 * and loading player selection
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;



public class LobbyManager : MonoBehaviourPunCallbacks
{
    // UI OBJECTS
    
    [Header("Login UI")] 
    public InputField playerNameInputField;
    public GameObject uI_LoginGameObject;

    [Header("Lobby UI")] 
    public GameObject uI_LobbyGameObject;
    public GameObject uI_3DGameObject;
    
    [Header("Connection Status UI")] 
    public GameObject uI_ConnectionStatusGameObject;
    public Text connectionStatusText;
    public bool showConnectionStatus = false;
    
    #region Default Unity Methods
    // Start is called before the first frame update
    void Start()
    {

        if (PhotonNetwork.IsConnected)
        {
            // Activating only Lobby UI
            uI_LobbyGameObject.SetActive(true);
            uI_3DGameObject.SetActive(true);
            
            uI_ConnectionStatusGameObject.SetActive(false);
            uI_LoginGameObject.SetActive(false);
        }
        else 
        {
            /*
             * First time player gets into lobby part of the game, only Login UI should be visible,
             * and all other UI game objects should be disabled
             */

            // Activating only Login UI since user didn't connect to Photon yet
            uI_LobbyGameObject.SetActive(false);
            uI_3DGameObject.SetActive(false);
            uI_ConnectionStatusGameObject.SetActive(false);
        
            uI_LoginGameObject.SetActive(true);
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {

        if (showConnectionStatus)
        { 
            connectionStatusText.text = "Connection Status: " + PhotonNetwork.NetworkClientState;
        }
       
    }

    #endregion

    #region UI Callback Methods

    public void OnEnterGameButtonClicked()
    {

        string playerName = playerNameInputField.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            
            uI_LobbyGameObject.SetActive(false);
            uI_3DGameObject.SetActive(false);
            uI_LoginGameObject.SetActive(false);
            
            uI_ConnectionStatusGameObject.SetActive(true);
            showConnectionStatus = true;
            
            if (!PhotonNetwork.IsConnected)
            {
                // Assigning player name to Photon server
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }

        }
        else
        {
            Debug.Log("Player name is invalid or empty!");
        }
    }

    public void OnQuickMatchButtonClicked()
    {
        // No more using that above code line because we created SceneLoader class
        // SceneManager.LoadScene("Scene_Loading");  
        
        SceneLoader.Instance.LoadScene("Scene_PlayerSelection");
    }
     
    

    #endregion

    #region PHOTON Callback Methods

    // This callback method is called when the internet connection is established
    public override void OnConnected() 
    {
        Debug.Log("Game is connected to Internet");
        
    }


    // This callback method is called when the user is successfully connected to Photon server
    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon Server");
        Debug.Log(PhotonNetwork.NickName + " is connected to Photon Server");

        /*
         * When player connects to the servers, lobby and 3D UI should be visible
         */
        
        uI_LoginGameObject.SetActive(false);
        uI_ConnectionStatusGameObject.SetActive(false);
        
        uI_LobbyGameObject.SetActive(true);
        uI_3DGameObject.SetActive(true);
    }

    public void OfflineMatch()
    {
        SceneLoader.Instance.LoadScene("Scene_AIPlayerSelection");
    }

    #endregion
}
