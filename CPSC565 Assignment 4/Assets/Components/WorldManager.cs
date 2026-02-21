using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WorldManager : MonoBehaviour
{

    #region Fields


    /// <summary>
    /// List of L systems to use
    /// </summary>
    public List<LSystem> system;


    #endregion

    #region Initialization

    /// <summary>
    /// Awake is called before any start method is called.
    /// </summary>
    void Awake()
    {
        Symbol grow = new Symbol('G');
        Symbol bloom = new Symbol('B');

        List<Symbol> intoBloomOutput = new List<Symbol> {grow, bloom};
        List<Symbol> intoBloomOutput2 = new List<Symbol> {bloom, grow};

        Rule intoBloom = new Rule(grow, intoBloomOutput);
        Rule bloomGrow = new Rule(bloom, intoBloomOutput2);

        List<Rule> ruleList = new List<Rule> {intoBloom, bloomGrow};

        LSystem sys1 = new LSystem(ruleList, new List<Symbol> {grow});

        for(int i = 0; i < 1; i++)
        {
            sys1.step();
            Debug.Log("At index " + i + ": Current L-String = " + sys1.convertToString());
        }

    }

    /// <summary>
    /// Called after every awake has been called.
    /// </summary>
    private void Start()
    {
    }

    

    #endregion

    #region Methods

    // Choose a System
    void LoadSystem()
    {
        
    }

    // After choosing a L system, create the string
    void GenerateStuct()
    {
        
    }

    // Reset the world if need be
    void ResetWorld()
    {
        
    }

    #endregion
}

