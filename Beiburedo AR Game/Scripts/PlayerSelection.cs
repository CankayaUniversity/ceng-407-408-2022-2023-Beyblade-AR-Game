using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    public GameObject[] gameObjects;
    public GameObject Enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ActivateGameObject(int index)
    {
        // Deactivate all game objects
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(false);
        }

        // Activate the specified game object
        if (index >= 0 && index < gameObjects.Length)
        {
            gameObjects[index].SetActive(true);
        }

    }

    public void PlaceButton()
    {
        int playerPrefValue = PlayerPrefs.GetInt("PlayerSelectionNumber");

        // Check the player pref value and activate the corresponding game object
        if (playerPrefValue == 0)
        {
            ActivateGameObject(0);
        }
        else if (playerPrefValue == 1)
        {
            ActivateGameObject(1);
        }
        else if (playerPrefValue == 2)
        {
            ActivateGameObject(2);
        }
        else if (playerPrefValue == 3)
        {
            ActivateGameObject(3);
        }
        Enemy.SetActive(true);
    }
}
