using System;
using System.Collections;
using System.Collections.Generic;
using Arcomage.Entity;
using Arcomage.Entity.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

public class StartSettings : MonoBehaviour, IStartParams
{
    Dictionary<Attributes, int> Params = new Dictionary<Attributes, int>();

    /// <summary>
    /// 
    /// </summary>
    internal void Randomize()
    {
        Params.Clear();
        SetAttribute(Attributes.Tower, Random.Range(10, 15));
        SetAttribute(Attributes.Wall, Random.Range(5,10));
        SetAttribute(Attributes.DiamondMines, Random.Range(1,5));
        SetAttribute(Attributes.Menagerie, Random.Range(1,5));
        SetAttribute(Attributes.Colliery, Random.Range(1,5));
        SetAttribute(Attributes.Diamonds, Random.Range(5,10));
        SetAttribute(Attributes.Animals, Random.Range(5,10));
        SetAttribute(Attributes.Rocks, Random.Range(5,10));
        //SetAttribute(Attributes.DirectDamage, 100);
    }

    int MaxCards = 6;

    /// <summary>
    /// 
    /// </summary>
    public Dictionary<Attributes, int> DefaultParams
    {
        get
        {
            return Params;
        }
        set
        {
            Params = value;
        }
    }

    public static StartSettings Instance { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    int IStartParams.MaxPlayerCard
    {
        get
        {
            return MaxCards;
        }

        set
        {
            MaxCards = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
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
    private void Start()
    {
        SetDefault();
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetDefault()
    {
        Params.Clear();
        SetAttribute(Attributes.Tower, 10);
        SetAttribute(Attributes.Wall, 5);
        SetAttribute(Attributes.DiamondMines, 1);
        SetAttribute(Attributes.Menagerie,1);
        SetAttribute(Attributes.Colliery, 1);
        SetAttribute(Attributes.Diamonds, 5);
        SetAttribute(Attributes.Animals, 5);
        SetAttribute(Attributes.Rocks, 5);
        //SetAttribute(Attributes.DirectDamage, 100);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directDamage"></param>
    /// <param name="v"></param>
    private void SetAttribute(Attributes attributes, int value)
    {
        if (Params.ContainsKey(attributes))
        {
            Debug.Log("PSA");
            Params[attributes] = value;
        }
        else
        {
            Debug.Log("Set");
            Params.Add(attributes, value);
        }
    }
}
