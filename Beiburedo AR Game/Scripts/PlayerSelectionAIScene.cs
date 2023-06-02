using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSelectionAIScene : MonoBehaviour
{
    public Transform playerSwitcherTransform;

    public GameObject[] spinnerTopModels;

    public int playerSelectionNumber;

    [Header("UI")]
    public TextMeshProUGUI playerModelType_Text;
    public Button next_Button;
    public Button previous_Button;

    public GameObject uI_Selection;
    public GameObject uI_AfterSelection;

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        uI_Selection.SetActive(true);
        uI_AfterSelection.SetActive(false);

        playerSelectionNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region UI Callback Methods

    public void NextPlayer()
    {
        playerSelectionNumber += 1;

        if (playerSelectionNumber >= spinnerTopModels.Length)
        {
            playerSelectionNumber = 0;
        }

        // Avoiding bugs
        next_Button.enabled = false;
        previous_Button.enabled = false;

        // Rotating the beyblades
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, 90, 1.0f));

        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            // This means the player model type is ATTACKER
            playerModelType_Text.text = "ATTACKER";
        }
        else
        {
            // This means the player model type is DEFENDER
            playerModelType_Text.text = "DEFENDER";
        }
    }

    public void PreviousPlayer()
    {
        playerSelectionNumber -= 1;

        if (playerSelectionNumber < 0)
        {
            playerSelectionNumber = spinnerTopModels.Length - 1;
        }

        // Avoiding bugs
        next_Button.enabled = false;
        previous_Button.enabled = false;

        // Rotating the beyblades
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90, 1.0f));

        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            // This means the player model type is ATTACKER
            playerModelType_Text.text = "ATTACKER";
        }
        else
        {
            // This means the player model type is DEFENDER
            playerModelType_Text.text = "DEFENDER";
        }
    }

    public void OnSelectButtonClicked()
    {
        uI_Selection.SetActive(false);
        uI_AfterSelection.SetActive(true);
       // playerSelectionNumber = PlayerPrefs.GetInt("PlayerSelectionNumber");
        PlayerPrefs.SetInt("PlayerSelectionNumber", playerSelectionNumber);
    }

    public void OnReSelectButtonClicked()
    {
        uI_Selection.SetActive(true);
        uI_AfterSelection.SetActive(false);
    }

    public void OnBattleButtonClicked()
    {
        // Load your battle scene here
        SceneLoader.Instance.LoadScene("Scene_vsAI");
    }

    public void OnBackButtonClicked()
    {
        // Load your lobby scene here
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }

    #endregion

    #region Private Methods

    IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1.0f)
    {
        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation * Quaternion.Euler(axis * angle);

        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transformToRotate.rotation = finalRotation;

        // Rotation completed, button can be shown again
        next_Button.enabled = true;
        previous_Button.enabled = true;

    }

    #endregion
}