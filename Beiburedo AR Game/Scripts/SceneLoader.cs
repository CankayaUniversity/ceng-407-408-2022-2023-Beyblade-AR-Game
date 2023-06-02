/*
 * -> We inherited Singleton class to implement Singleton pattern to SceneLoader script.
 * With this way, we will be able to access to SceneLoader's public methods from anywhere in the game.
 * -> Also, SceneLoader will be automatically be alive in all scenes thanks to DontDestroyOnLoad method of Singleton class.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    private string sceneNameToBeLoaded;

    public void LoadScene(string _sceneName)
    {
        sceneNameToBeLoaded = _sceneName;
        
        StartCoroutine(InitializeSceneLoading());
    }

    IEnumerator InitializeSceneLoading()
    {
        // Firstly, load the loading scene
        yield return SceneManager.LoadSceneAsync("Scene_Loading"); // LoadSceneAsync method will load the Scene asynchronously in the background
        
        // Load the actual scene
        StartCoroutine(LoadActualScene());
    }

    IEnumerator LoadActualScene()
    {
        // With this line of code the scene loading progress just started.
        var asyncSceneLoading = SceneManager.LoadSceneAsync(sceneNameToBeLoaded);
        
        // This code execution stops the scene from displaying when it is still loading
        asyncSceneLoading.allowSceneActivation = false;

        while (!asyncSceneLoading.isDone)
        {
            
            // This means that if the loading progress reaches to at least 90 percent
            if (asyncSceneLoading.progress >= 0.9f)
            {
                // Finally show the scene
                asyncSceneLoading.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
