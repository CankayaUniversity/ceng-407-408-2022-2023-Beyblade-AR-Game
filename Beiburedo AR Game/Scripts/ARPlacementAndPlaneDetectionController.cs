using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ARPlacementAndPlaneDetectionController : MonoBehaviour
{
    private ARPlaneManager m_ARPlaneManager;
    private ARPlacementManager m_ARPlacementManager;

    public GameObject placeButton;
    public GameObject adjustButton;
    public GameObject searchForGameButton;
    public GameObject scaleSlider;
    
    public TextMeshProUGUI informUIPanel_Text;

    private void Awake()
    {
        m_ARPlacementManager = GetComponent<ARPlacementManager>();
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        placeButton.SetActive(true);
        scaleSlider.SetActive(true);
        
        adjustButton.SetActive(false);
        searchForGameButton.SetActive(false);
        

        informUIPanel_Text.text = "Move phone to detect planes and place the Battle Arena!";
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableARPlacementAndPlaneDetection() // When place button clicked
    {
        m_ARPlacementManager.enabled = false;
        m_ARPlacementManager.enabled = false;
        SetAllPlanesActiveOrDeactive(false);
        
        scaleSlider.SetActive(false);

        placeButton.SetActive(false);
        adjustButton.SetActive(true);
        searchForGameButton.SetActive(true);
        
        informUIPanel_Text.text = "Great! You placed the ARENA. Now, search for games to BATTLE!";

    }    
    
    public void EnableARPlacementAndPlaneDetection() // When adjust button clicked
    {
        m_ARPlacementManager.enabled = true;
        m_ARPlacementManager.enabled = true;
        SetAllPlanesActiveOrDeactive(true);
        scaleSlider.SetActive(true);
        
        placeButton.SetActive(true);
        adjustButton.SetActive(false);
        searchForGameButton.SetActive(false);
        
        informUIPanel_Text.text = "Move phone to detect planes and place the Battle Arena!";

    }

    private void SetAllPlanesActiveOrDeactive(bool value)
    {
        foreach (var plane in m_ARPlaneManager.trackables) // with this way, we will be able to access to detected planes
        {
            plane.gameObject.SetActive(value);
        }
    }
}
