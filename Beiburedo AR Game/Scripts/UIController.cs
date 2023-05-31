using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject[] objectsToSetFalse;
    public GameObject[] objectsToSetActive;

    public void Place()
    {
        StartCoroutine(SetObjectsActive());
    }

    IEnumerator SetObjectsActive()
    {
        // Set objects to false after 1 second
        foreach (GameObject obj in objectsToSetFalse)
        {
            yield return new WaitForSeconds(3f);
            obj.SetActive(false);
        }

        // Set objects to active after 2 seconds
        foreach (GameObject obj in objectsToSetActive)
        {
            yield return new WaitForSeconds(2f);
            obj.SetActive(true);
        }
    }
}
