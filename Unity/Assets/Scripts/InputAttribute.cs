using System.Collections;
using System.Collections.Generic;
using Arcomage.Entity;
using UnityEngine;
using UnityEngine.UI;

public class InputAttribute : MonoBehaviour
{
    public Attributes Attribute;
    public bool IsWinParam;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetValue()
    {
        string str = GetComponent<InputField>().text;

        int result;

        if ( int.TryParse( str, out result ))
        {
            return result;
        }

        return 0;
    }
}
