using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ScaleController : MonoBehaviour
{
    private ARSessionOrigin m_ARSessionOrigin;

    public Slider scaleSlider;

    private void Awake()
    {
        m_ARSessionOrigin = GetComponent<ARSessionOrigin>();
    }

    // Start is called before the first frame update
    void Start()
    {
        scaleSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public void OnSliderValueChanged(float value)
    {
        if (scaleSlider != null)
        {
            /*
             * With this way, whenever player uses the slider,
             * the scale of AR Session Origin will be changed to this value coming from the slider.
             */
            m_ARSessionOrigin.transform.localScale = Vector3.one / value; 
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
