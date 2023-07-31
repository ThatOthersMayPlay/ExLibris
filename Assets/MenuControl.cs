using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{

    public void HideUIElement(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void ShowUIElement(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    //Load next level:
    public static void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    //...with option to jump to current level +- x
    public static void NextLevel(int difIndex)
    {
        int sceneRange = SceneManager.sceneCountInBuildSettings;
        int curSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if ((curSceneIndex + difIndex) >= 0 && (curSceneIndex + difIndex) < sceneRange - 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + difIndex);
        }
        else
            Debug.Log("Level index out of range");
    }

    public void ToLevel(string sc)
    {
        SceneManager.LoadScene(sc);
    }
}
