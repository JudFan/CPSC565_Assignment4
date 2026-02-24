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
        Symbol addIntobloom = new Symbol('A');
        Symbol makePetalBody = new Symbol('P');
        Symbol makePetalEdge = new Symbol('p');
        Symbol save = new Symbol('[');
        Symbol load = new Symbol(']');

        Symbol end = new Symbol('E');

        List<Symbol> growing = new List<Symbol> {grow, grow};
        List<Symbol> makePetals = new List<Symbol> {save, makePetalBody, addIntobloom, makePetalEdge, addIntobloom, load, save, makePetalBody, addIntobloom, makePetalEdge, addIntobloom, load, save, makePetalBody, addIntobloom, makePetalEdge, addIntobloom, load, save, makePetalBody, addIntobloom, makePetalEdge, addIntobloom, load, save, makePetalBody, addIntobloom, makePetalEdge, addIntobloom, load};
        List<Symbol> blooming = new List<Symbol> {bloom, grow, end};

        // TO DO ENFORCE CONTEXT SENSITIVE RULES
        Rule toGrow = new Rule(grow, growing);
        Rule petalMake = new Rule(bloom, makePetals);
        Rule toBloom = new Rule(bloom, blooming);

        List<Rule> ruleList = new List<Rule> {toGrow, petalMake, toBloom};

        List<Rule> ruleList2 = new List<Rule> {toGrow};

        LSystem sys1 = new LSystem(ruleList, new List<Symbol> {grow, end});

        LSystem sys2 = new LSystem(ruleList2, new List<Symbol> {grow, end});

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
        chosenSystem.reset(); 
        
        for(int i = 0; i < ui.numOfIters; i++)
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

