using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LSystem
{
    public List<Rule> rules;

    public List<Symbol> currentString;
    public bool canGoOn;
    private List<Symbol> axiom;

    public LSystem(List<Rule> ruleInput, List<Symbol> symbolInput)
    {
        rules = ruleInput;
        currentString = symbolInput;
        axiom = symbolInput;
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
            foreach (Rule rule in rules)
            {
                Debug.Log("a rule was checked " + currentString[i].expressAsString());
                List<Symbol> result = rule.RuleChange(currentString[i]);


                if(result is not null)
                {
                    canGoOn = true;
                    foundARuleMatch = true;
                    newString.InsertRange(newString.Count, result);
                }
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
