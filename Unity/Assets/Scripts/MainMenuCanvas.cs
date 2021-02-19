using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public void OnRandomGame()
    {
        StartSettings.Instance.Randomize();
        SceneManager.LoadScene("Main");

    }

    /// <summary>
    /// 
    /// </summary>
    public void OnCustomGame()
    {
        StartSettings.Instance.SetDefault();
        SceneManager.LoadScene("Settings");
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnQuit()
    {
        Application.Quit();
    }
}
