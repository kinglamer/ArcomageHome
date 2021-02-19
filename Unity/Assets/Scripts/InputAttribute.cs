using Arcomage.Entity;
using UnityEngine;
using UnityEngine.UI;

public class InputAttribute : MonoBehaviour
{
    public Attributes Attribute;
    public string PlayerName;
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

    public void SetValue(int val)
    {
        GetComponent<InputField>().text = val.ToString();    
    }
}
