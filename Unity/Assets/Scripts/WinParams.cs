using System;
using System.Collections;
using System.Collections.Generic;
using Arcomage.Core.Interfaces.Impl;
using Arcomage.Entity;
using UnityEngine;

public class WinParams : MonoBehaviour 
{
    public static WinParams Instance { get; private set; }
    public GameStartParams HumanParams = new GameStartParams();
    public GameStartParams AIParams = new GameStartParams();
    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        var defaultValue = new GameStartParams();
        foreach (InputAttribute inputAttribute in FindObjectsOfType<InputAttribute>())
        {
            inputAttribute.SetValue(defaultValue.DefaultParams[inputAttribute.Attribute]);
        }

        if (Instance)
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
    private void SetAttribute(Dictionary<Attributes, int> Params, Attributes attributes, int value)
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
            int val = inputAttribute.GetValue();

            if (val > 0)
            {
                if(inputAttribute.PlayerName.StartsWith("human", StringComparison.OrdinalIgnoreCase))
                    SetAttribute(HumanParams.DefaultParams, inputAttribute.Attribute, val);
                else
                    SetAttribute(AIParams.DefaultParams, inputAttribute.Attribute, val);
            }
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
