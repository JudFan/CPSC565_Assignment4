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
        Symbol bud = new Symbol('b');
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
        List<Symbol> makePetals = new List<Symbol> {bud, save, save, makePetalBody, addIntobloom, makePetalEdge, addIntobloom, load, save, makePetalBody, addIntobloom, makePetalEdge, addIntobloom, load, save, makePetalBody, addIntobloom, makePetalEdge, addIntobloom, load, save, makePetalBody, addIntobloom, makePetalEdge, addIntobloom, load, save, makePetalBody, addIntobloom, makePetalEdge, addIntobloom, load, load, end};
        List<Symbol> blooming = new List<Symbol> {bloom, grow};

        List<Symbol> justEnd = new List<Symbol> {end};
        List<Symbol> enoughHeight = new List<Symbol> {grow, grow};
        List<Symbol> petal = new List<Symbol> {addIntobloom, load};

        // For 2nd L-System
        List<Symbol> fruitOut = new List<Symbol> {growStrightUp}; // Blank rule as fruit cannot spawn on a vertical branch
        List<Symbol> straightOut = new List<Symbol> {growStrightUp, growStrightUp};
        List<Symbol> northOut = new List<Symbol> {growStrightUp, growNorthUp};
        List<Symbol> southOut = new List<Symbol> {growStrightUp, growSouthUp};
        List<Symbol> eastOut = new List<Symbol> {growStrightUp, growEastUp};
        List<Symbol> westOut = new List<Symbol> {growStrightUp, growWestUp};
        List<Symbol> saveStr = new List<Symbol> {growStrightUp, save};
        List<Symbol> loadStr = new List<Symbol> {growStrightUp, load};

        List<Symbol> fruitOutN = new List<Symbol> {growNorthUp, save, fruit, load};
        List<Symbol> straightOutN = new List<Symbol> {growNorthUp, growStrightUp};
        List<Symbol> northOutN = new List<Symbol> {growNorthUp, growNorthUp};
        List<Symbol> southOutN = new List<Symbol> {growNorthUp, growSouthUp};
        List<Symbol> eastOutN = new List<Symbol> {growNorthUp, growEastUp};
        List<Symbol> westOutN = new List<Symbol> {growNorthUp, growWestUp};
        List<Symbol> saveN = new List<Symbol> {growNorthUp, save};
        List<Symbol> loadN = new List<Symbol> {growNorthUp, load};
        
        List<Symbol> fruitOutS = new List<Symbol> {growSouthUp, save, fruit, load};
        List<Symbol> straightOutS = new List<Symbol> {growSouthUp, growStrightUp};
        List<Symbol> northOutS = new List<Symbol> {growSouthUp, growNorthUp};
        List<Symbol> southOutS = new List<Symbol> {growSouthUp, growSouthUp};
        List<Symbol> eastOutS = new List<Symbol> {growSouthUp, growEastUp};
        List<Symbol> westOutS = new List<Symbol> {growSouthUp, growWestUp};
        List<Symbol> saveS = new List<Symbol> {growSouthUp, save};
        List<Symbol> loadS = new List<Symbol> {growSouthUp, load};

        List<Symbol> fruitOutE = new List<Symbol> {growEastUp, save, fruit, load};
        List<Symbol> straightOutE = new List<Symbol> {growEastUp, growStrightUp};
        List<Symbol> northOutE = new List<Symbol> {growEastUp, growNorthUp};
        List<Symbol> southOutE = new List<Symbol> {growEastUp, growSouthUp};
        List<Symbol> eastOutE = new List<Symbol> {growEastUp, growEastUp};
        List<Symbol> westOutE = new List<Symbol> {growEastUp, growWestUp};
        List<Symbol> saveE = new List<Symbol> {growEastUp, save};
        List<Symbol> loadE = new List<Symbol> {growEastUp, load};

        List<Symbol> fruitOutW = new List<Symbol> {growWestUp, save, fruit, load};
        List<Symbol> straightOutW = new List<Symbol> {growWestUp, growStrightUp};
        List<Symbol> northOutW = new List<Symbol> {growWestUp, growNorthUp};
        List<Symbol> southOutW = new List<Symbol> {growWestUp, growSouthUp};
        List<Symbol> eastOutW = new List<Symbol> {growWestUp, growEastUp};
        List<Symbol> westOutW = new List<Symbol> {growWestUp, growWestUp};
        List<Symbol> saveW = new List<Symbol> {growWestUp, save};
        List<Symbol> loadW = new List<Symbol> {growWestUp, load};

        // Used by both
        List<Symbol> empty = new List<Symbol> {};

        // System 1 Rules
        Rule toGrow = new Rule(grow, growing, empty, justEnd);
        Rule petalMake = new Rule(end, makePetals, enoughHeight, empty);
        Rule toBloom = new Rule(load, blooming, petal, justEnd);

        List<Rule> ruleList = new List<Rule> {toGrow, petalMake, toBloom};

        // System 2 Rules
        List<List<Symbol>> possibleOutcomes = new List<List<Symbol>> {fruitOut, straightOut, northOut, southOut, eastOut, westOut, saveStr, loadStr};
        List<List<Symbol>> possibleOutcomesN = new List<List<Symbol>> {fruitOutN, straightOutN, northOutN, southOutN, eastOutN, westOutN, saveN, loadN};
        List<List<Symbol>> possibleOutcomesS = new List<List<Symbol>> {fruitOutS, straightOutS, northOutS, southOutS, eastOutS, westOutS, saveS, loadS}; 
        List<List<Symbol>> possibleOutcomesE = new List<List<Symbol>> {fruitOutE, straightOutE, northOutE, southOutE, eastOutE, westOutE, saveE, loadE}; 
        List<List<Symbol>> possibleOutcomesW = new List<List<Symbol>> {fruitOutW, straightOutW, northOutW, southOutW, eastOutW, westOutW, saveW, loadW};

        List<float> probDist = new List<float> {0.15f, 0.06f, 0.11f, 0.11f, 0.11f, 0.11f, 0.2f, 0.15f};

        RuleRandom straightRules = new RuleRandom(growStrightUp, possibleOutcomes, probDist);
        RuleRandom northRules = new RuleRandom(growNorthUp, possibleOutcomesN, probDist);;
        RuleRandom southRules = new RuleRandom(growSouthUp, possibleOutcomesS, probDist);;
        RuleRandom eastRules = new RuleRandom(growEastUp, possibleOutcomesE, probDist);;
        RuleRandom westRules = new RuleRandom(growWestUp, possibleOutcomesW, probDist);;

        // Each predecessor will have 8 rules each
        List<Rule> ruleList2 = new List<Rule> {straightRules, northRules, southRules, eastRules, westRules};
        

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
            chosenSystem.step();

            Debug.Log("At index " + i + ": Current L-String = " + chosenSystem.convertToString());
        }

        turtle.LSystemCommand = chosenSystem.convertToString();
    }

    #endregion
}

