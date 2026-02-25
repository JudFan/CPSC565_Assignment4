using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rule
{
    public Symbol Predecessor;
    public List<Symbol> Successor;

    public List<Symbol> leftContext;
    public List<Symbol> rightContext;
    private bool leftContextHasBrackets;
    private bool rightContextHasBrackets;

    public Rule(Symbol PreIn, List<Symbol> SucIn)
    {
        Predecessor = PreIn;
        Successor = SucIn;
    }

    // Alternative constructor 
    public Rule(Symbol PreIn, List<Symbol> SucIn, List<Symbol> leftIn, List<Symbol> rightIn)
    {
        Predecessor = PreIn;
        Successor = SucIn;
        leftContext = leftIn;
        rightContext = rightIn;

        // Checking if there are brackets in the contexts
        Symbol open = new Symbol('[');
        Symbol close = new Symbol(']');

        leftContextHasBrackets = leftContext.Contains(open) || leftContext.Contains(close);
        rightContextHasBrackets = rightContext.Contains(open) || rightContext.Contains(close);
    }

    public List<Symbol> RuleChange(Symbol input)
    {
        if (input.Equals(Predecessor))
        {
            return Successor;
        }
        else
        {
            return null;
        }
    }

    public List<Symbol> contextSensitiveRuleChange(Symbol input, int index, List<Symbol> LSystemString)
    {
        // bools to check if left/right contexts match
        bool leftMatch = false;
        bool rightMatch = false;

        // int metric here to account for bracketing: [ adds 1, ] subtracts 1
        // 0 means no bracketing is occuring. non 0 means there is bracketing
        int bracketingMetric = 0;

        // Need to memorise index of the closing bracket and the index of the symbol after it if any
        int closeBracketIndex = 0;
        int preClosedBracketIndex = leftContext.Count - 1;

        // Checking for context for left context
        int exploreIndex = index - 1;
        int neighbourIndex = leftContext.Count - 1;


        if(leftContext.Count > 0)
        {
            Debug.Log("Checking left context");
            while(exploreIndex >= 0)
            {
                // Scenario 1: No bracketing in left context 
                if(!leftContextHasBrackets)
                {
                    if(bracketingMetric == 0)
                    {
                        if(LSystemString[exploreIndex].symbol == leftContext[neighbourIndex].symbol)
                        {
                            preClosedBracketIndex = neighbourIndex - 1;
                            neighbourIndex--;
                        }
                        else if(LSystemString[exploreIndex].symbol == ']')
                        {
                            bracketingMetric--;
                        } 
                        else if(LSystemString[exploreIndex].symbol != '[') // Harmless to see a raw open bracket 
                        {
                            // match not found
                            return null; 
                        }
                    }
                    else  
                    {
                        if(LSystemString[exploreIndex].symbol == leftContext[neighbourIndex].symbol)
                        {
                            neighbourIndex++;
                        }
                        else if(LSystemString[exploreIndex].symbol == '[')
                        {
                            // match not found: restart search from preClosedBracketIndex
                            neighbourIndex = preClosedBracketIndex;
                            bracketingMetric++;
                        }
                    }
                }
                // Scenario 2: The left context has brackets -> Now we need to be more careful
                else
                {
                    // Finds immediate neighbour to be a match -> Progress made in search
                    if(LSystemString[exploreIndex].symbol == leftContext[neighbourIndex].symbol)
                    {
                        if(LSystemString[exploreIndex].symbol == ']')
                        {
                            bracketingMetric--;
                            closeBracketIndex = neighbourIndex;
                        } 
                        else if(LSystemString[exploreIndex].symbol == '[' && bracketingMetric != 0)
                        {
                            bracketingMetric++;
                        } 
                        else if(bracketingMetric == 0)
                        {
                            preClosedBracketIndex = neighbourIndex - 1;
                        }
                        neighbourIndex--;
                    }
                    // Finds raw opening bracket that is not a match with the left context -> Reset search parameters to latest closed bracket
                    else if(LSystemString[exploreIndex].symbol == '[')
                    {
                        neighbourIndex = closeBracketIndex;
                        bracketingMetric++;
                    }
                    else if(bracketingMetric == 0)
                    {
                        // match not found: Incorrect neighboring symbol from root
                        return null; 
                    }
                    else
                    {
                        // Match not found in the bracket, must reset search within the bracket
                        neighbourIndex = closeBracketIndex - 1;
                    }
                } 

                exploreIndex--;
                if(neighbourIndex < 0)
                {
                    leftMatch = true;
                    break;
                }
                if(exploreIndex < 0)
                {
                    return null;
                }
            }
        }
        else
        {
            leftMatch = true;
        }

        // Checking for context for right neighbour (reset local variables here)
        exploreIndex = index + 1;
        neighbourIndex = 0;
        bracketingMetric = 0;

        // Need to memorise index of the right context if an open bracket is discovered and index of the open bracket
        int preOpenBracketIndex = 0;
        int openBracketIndex = 0;

        if(rightContext.Count > 0) {
            Debug.Log("Checking right context");
            while (exploreIndex < LSystemString.Count)
            {
                // Scenario 1: No brackets in right context
                if(!rightContextHasBrackets)
                {
                    if(bracketingMetric == 0)
                    {
                        if(LSystemString[exploreIndex].symbol == rightContext[neighbourIndex].symbol)
                        {
                            preOpenBracketIndex = neighbourIndex + 1;
                            neighbourIndex++;
                        }
                        else if(LSystemString[exploreIndex].symbol == ']')
                        {
                            // match not found: raw closed bracket means no more left neighbours
                            return null;
                        } 
                        else if(LSystemString[exploreIndex].symbol != '[') // Harmless to see a raw open bracket 
                        {
                            bracketingMetric++;
                        }
                    }
                    else 
                    {
                        if(LSystemString[exploreIndex].symbol == rightContext[neighbourIndex].symbol)
                        {
                            neighbourIndex++;
                        }
                        else if(LSystemString[exploreIndex].symbol == ']')
                        {
                            // match not found: restart search from preOpenBracketIndex
                            neighbourIndex = preOpenBracketIndex;
                            bracketingMetric--;
                        }
                    }
                }

                
                // Scenario 2: brackets in the right context
                else
                {
                    // Finds immediate neighbour to be a match -> Progress made in search
                    if(LSystemString[exploreIndex].symbol == rightContext[neighbourIndex].symbol)
                    {
                        if(LSystemString[exploreIndex].symbol == '[')
                        {
                            bracketingMetric++;
                            openBracketIndex = neighbourIndex;
                        } 
                        else if(bracketingMetric == 0)
                        {
                            preOpenBracketIndex = neighbourIndex + 1;
                        }
                        neighbourIndex++;
                    }
                    // Finds closing bracket that is not a match with the left context -> Reset search parameters to latest open bracket
                    else if(LSystemString[exploreIndex].symbol == ']' && bracketingMetric != 0)
                    {
                        neighbourIndex = openBracketIndex;
                        bracketingMetric--;
                    }
                    else if(LSystemString[exploreIndex].symbol != '[' && bracketingMetric == 0)
                    {
                        // match not found: Incorrect neighboring symbol from root
                        return null; 
                    }
                }
                

                exploreIndex++;
                if(neighbourIndex >= rightContext.Count)
                {
                    rightMatch = true;
                    break;
                }
                if(exploreIndex >= LSystemString.Count)
                {
                    return null;
                }
            }
        }
        else
        {
            rightMatch = true;
        }

        if(leftMatch && rightMatch)
        {
            return Successor;
        }
        return null;
    }
}
