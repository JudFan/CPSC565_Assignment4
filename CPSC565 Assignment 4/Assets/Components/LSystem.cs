using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assign4.UI;

public class LSystem
{
    public UI_for_Setup ui;
    public List<Rule> rules;

    public List<Symbol> currentString;
    public bool canGoOn;
    private List<Symbol> axiom;
    private List<float> probDistr; 

    public LSystem(List<Rule> ruleInput, List<Symbol> symbolInput, UI_for_Setup uiIn)
    {
        rules = ruleInput;
        currentString = symbolInput;
        axiom = symbolInput;
        ui = uiIn;
    }
    public LSystem(List<Rule> ruleInput, List<Symbol> symbolInput, UI_for_Setup uiIn, List<float> probIn)
    {
        rules = ruleInput;
        currentString = symbolInput;
        axiom = symbolInput;
        ui = uiIn;
        probDistr = probIn;
    }

    public void reset()
    {
        currentString = axiom;
    }

    public void step()
    {
        canGoOn = false; 

        List<Symbol> newString = new List<Symbol>();

        for (int i = 0; i < currentString.Count; i++)
        {
            bool foundARuleMatch = false;

            int j = 0;
            foreach (Rule rule in rules)
            {
                Debug.Log("a rule was checked " + currentString[i].expressAsString());

                List<Symbol> result = null;

                result = rule.contextSensitiveRuleChange(currentString[i], i, currentString);

                if(result is not null)
                {
                    canGoOn = true;
                    foundARuleMatch = true;
                    newString.InsertRange(newString.Count, result);

                    //Debug
                    string resultStr = "";

                    foreach (Symbol sym in newString)
                    {
                        resultStr += sym.expressAsString();
                    }
                    Debug.Log("At rule " + j + ": Current L-String = " + resultStr);
                }
                j++;
            }
            if(!foundARuleMatch)
            {
                newString.Add(currentString[i]); 
            }
        }
        currentString = newString;
    }

    public void stepRandom()
    {
        canGoOn = false; 

        List<Symbol> newString = new List<Symbol>();

        for (int i = 0; i < currentString.Count; i++)
        {
            bool foundARuleMatch = false;

            Debug.Log("a rule was checked " + currentString[i].expressAsString());

            List<Symbol> result = null;

            float random = Random.value;

            int ruleIndex = 0;
            while (ruleIndex < probDistr.Count)
            {
                random -= probDistr[ruleIndex];
                if(random <= 0)
                {
                    break;
                }
                ruleIndex++;
            }
            result = rules[ruleIndex].RuleChange(currentString[i]);
            


            if(result is not null)
            {
                canGoOn = true;
                foundARuleMatch = true;
                newString.InsertRange(newString.Count, result);
            }
            
            if(!foundARuleMatch)
            {
                newString.Add(currentString[i]); 
            }
        }
        currentString = newString;
    }
    

    public string convertToString()
    {
        string result = "";

        foreach (Symbol sym in currentString)
        {
            result += sym.expressAsString();
        }

        return result;
    }
}
