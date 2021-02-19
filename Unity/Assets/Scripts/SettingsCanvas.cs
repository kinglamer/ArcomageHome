using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsCanvas : MonoBehaviour
{   public void OnStartGame()
    {
        Debug.Log("Perehod");
        StartSettings.Instance.ReadAllSettings();

        if(WinParams.Instance)
            WinParams.Instance.ReadAllSettings();

        SceneManager.LoadScene("Main");
    }
}
