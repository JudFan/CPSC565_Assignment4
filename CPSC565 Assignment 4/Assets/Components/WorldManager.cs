using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assign4.UI;



public class WorldManager : MonoBehaviour
{

    #region Fields


    /// <summary>
    /// List of L systems to use
    /// </summary>
    public List<LSystem> systems;

    /// <summary>
    /// The L System chosen by the user
    /// </summary>
    public LSystem chosenSystem;

    /// <summary>
    /// The UI it grabs info from
    /// </summary>
    public UI_for_Setup ui;

    /// <summary>
    /// The Turtle the manager will pass the final L system mutated string to
    /// </summary>
    public Turtle turtle;


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

        List<Rule> ruleList = new List<Rule> {intoBloom};

        List<Rule> ruleList2 = new List<Rule> {bloomGrow};

        LSystem sys1 = new LSystem(ruleList, new List<Symbol> {grow});

        LSystem sys2 = new LSystem(ruleList2, new List<Symbol> {grow});

        systems = new List<LSystem>();

        systems.Add(sys1);
        systems.Add(sys2);
        

    }

    /// <summary>
    /// Called when start button is pressed
    /// </summary>
    public void Execute()
    {
        LoadSystem();
        GenerateStuct();
    }

    

    #endregion

    #region Methods

    // Choose a System
    void LoadSystem()
    {
        Debug.Log(ui.selectedRule);

        if(ui.selectedRule == 1)
        {
            chosenSystem = systems[0];
        }
        else if (ui.selectedRule == 2)
        {
            chosenSystem = systems[1];
        }
    }

    // After choosing a L system, create the string
    void GenerateStuct()
    {
        for(int i = 0; i < 2; i++)
        {
            chosenSystem.step();
            Debug.Log("At index " + i + ": Current L-String = " + chosenSystem.convertToString());
        }

        turtle.LSystemCommand = chosenSystem.convertToString();
    }

    // Reset the world if need be
    void ResetWorld()
    {
        
    }

    #endregion
}

