using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsCanvas : MonoBehaviour
{   public void OnStartGame()
    {
        if(WinParams.Instance)
            WinParams.Instance.ReadAllSettings();

        SceneManager.LoadScene("Main");
    }
}
