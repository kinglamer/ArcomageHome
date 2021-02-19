using System;
using System.Collections;
using System.Collections.Generic;
using Arcomage.Entity;
using UnityEngine;

public class WinParams : MonoBehaviour 
{
    public static WinParams Instance { get; private set; }
    public Dictionary<Attributes, int> Params = new Dictionary<Attributes, int>();

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        if( Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attributes"></param>
    /// <param name="value"></param>
    private void SetAttribute(Attributes attributes, int value)
    {
        if (Params.ContainsKey(attributes))
            Params[attributes] = value;
        else
            Params.Add(attributes, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public void ReadAllSettings()
    {
        foreach (InputAttribute inputAttribute in FindObjectsOfType<InputAttribute>())
        {
            if (!inputAttribute.IsWinParam)
                continue;

            int val = inputAttribute.GetValue();

            if (val > 0)
                SetAttribute(inputAttribute.Attribute, val);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
