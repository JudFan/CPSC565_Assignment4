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
        // Used by 1st L-System
        Symbol grow = new Symbol('G');
        Symbol bloom = new Symbol('B');
        Symbol addIntobloom = new Symbol('A');
        Symbol makePetalBody = new Symbol('P');
        Symbol makePetalEdge = new Symbol('p');

        // Used by 2nd L-System
        Symbol fruit = new Symbol('F');
        Symbol growStrightUp = new Symbol('U');
        Symbol growNorthUp = new Symbol('n');
        Symbol growSouthUp = new Symbol('s');
        Symbol growEastUp = new Symbol('e');
        Symbol growWestUp = new Symbol('w');

        // Used by both L Systems
        Symbol save = new Symbol('[');
        Symbol load = new Symbol(']');
        
        Symbol end = new Symbol('E');

        // For first L-System
        List<Symbol> growing = new List<Symbol> {grow, grow};
        List<Symbol> makePetals = new List<Symbol> {grow, save, save, makePetalBody, addIntobloom, makePetalEdge, addIntobloom, load, save, makePetalBody, addIntobloom, makePetalEdge, addIntobloom, load, save, makePetalBody, addIntobloom, makePetalEdge, addIntobloom, load, save, makePetalBody, addIntobloom, makePetalEdge, addIntobloom, load, save, makePetalBody, addIntobloom, makePetalEdge, addIntobloom, load, load, end};
        List<Symbol> blooming = new List<Symbol> {bloom, grow, end};

        List<Symbol> justEnd = new List<Symbol> {end};
        List<Symbol> enoughHeight = new List<Symbol> {grow, grow, grow, grow, grow};
        List<Symbol> petal = new List<Symbol> {load, load};

        // For 2nd L-System
        /**
        List<Symbol> fruitOutN = new List<Symbol> {growNorthUp, save, fruit, load};
        List<Symbol> fruitOutS = new List<Symbol> {growSouthUp, save, fruit, load};
        List<Symbol> fruitOutE = new List<Symbol> {growEastUp, save, fruit, load};
        List<Symbol> fruitOutW = new List<Symbol> {growWestUp, save, fruit, load};

        List<Symbol> straightOut = new List<Symbol> {growStrightUp, growStrightUp};
        List<Symbol> straightOutN = new List<Symbol> {growNorthUp, growStrightUp};
        List<Symbol> straightOutS = new List<Symbol> {growSouthUp, growStrightUp};
        List<Symbol> straightOutE = new List<Symbol> {growEastUp, growStrightUp};
        List<Symbol> straightOutW = new List<Symbol> {growWestUp, growStrightUp};
        List<Symbol> straightOutEnd = new List<Symbol> {end, growStrightUp};

        List<Symbol> straightOut = new List<Symbol> {growStrightUp, growNorthUp};
        List<Symbol> straightOutN = new List<Symbol> {growNorthUp, growNorthUp};
        List<Symbol> straightOutS = new List<Symbol> {growSouthUp, growNorthUp};
        List<Symbol> straightOutE = new List<Symbol> {growEastUp, growNorthUp};
        List<Symbol> straightOutW = new List<Symbol> {growWestUp, growNorthUp};
        List<Symbol> straightOutEnd = new List<Symbol> {end, growNorthUp};

        List<Symbol> straightOut = new List<Symbol> {growStrightUp, growSouthUp};
        List<Symbol> straightOutN = new List<Symbol> {growNorthUp, growSouthUp};
        List<Symbol> straightOutS = new List<Symbol> {growSouthUp, growSouthUp};
        List<Symbol> straightOutE = new List<Symbol> {growEastUp, growSouthUp};
        List<Symbol> straightOutW = new List<Symbol> {growWestUp, growSouthUp};
        List<Symbol> straightOutEnd = new List<Symbol> {end, growSouthUp};

        List<Symbol> straightOut = new List<Symbol> {growStrightUp, growEastUp};
        List<Symbol> straightOutN = new List<Symbol> {growNorthUp, growEastUp};
        List<Symbol> straightOutS = new List<Symbol> {growSouthUp, growEastUp};
        List<Symbol> straightOutE = new List<Symbol> {growEastUp, growEastUp};
        List<Symbol> straightOutW = new List<Symbol> {growWestUp, growEastUp};
        List<Symbol> straightOutEnd = new List<Symbol> {end, growEastUp};

        List<Symbol> straightOut = new List<Symbol> {growStrightUp, growWestUp};
        List<Symbol> straightOutN = new List<Symbol> {growNorthUp, growWestUp};
        List<Symbol> straightOutS = new List<Symbol> {growSouthUp, growWestUp};
        List<Symbol> straightOutE = new List<Symbol> {growEastUp, growWestUp};
        List<Symbol> straightOutW = new List<Symbol> {growWestUp, growWestUp};
        List<Symbol> straightOutEnd = new List<Symbol> {end, growWestUp};
        **/

        // Used by both
        List<Symbol> empty = new List<Symbol> {};

        // TO DO ENFORCE CONTEXT SENSITIVE RULES
        Rule toGrow = new Rule(grow, growing, empty, justEnd);
        Rule petalMake = new Rule(end, makePetals, enoughHeight, empty);
        Rule toBloom = new Rule(end, blooming, petal, empty);

        List<Rule> ruleList = new List<Rule> {toGrow, petalMake, toBloom};

        // Unsure how to implement probability distribution like this
        List<Rule> ruleList2 = new List<Rule> {toGrow};


        LSystem sys1 = new LSystem(ruleList, new List<Symbol> {grow, end}, ui);

        LSystem sys2 = new LSystem(ruleList2, new List<Symbol> {growStrightUp, end}, ui);

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
            
            if(ui.selectedRule == 1)
            {
                chosenSystem.step();
            }
            else if (ui.selectedRule == 2)
            {
                chosenSystem.stepRandom();
            }
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

