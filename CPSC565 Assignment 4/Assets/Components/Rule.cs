using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rule
{
    public Symbol Predecessor;
    public List<Symbol> Successor;
    public List<Symbol> LSystemString;

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
    public Rule(Symbol PreIn, List<Symbol> SucIn, List<Symbol> listIn, List<Symbol> leftIn, List<Symbol> rightIn)
    {
        Predecessor = PreIn;
        Successor = SucIn;
        LSystemString = listIn;
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

    public List<Symbol> contextSensitiveRuleChange(Symbol input, int index)
    {

        // Boolean here to account for bracketing
        bool bracketed = false;

        // Need to memorise index of the closing bracket and the index of the symbol after it if any
        int closeBracketIndex = 0;
        int preClosedBracketIndex = leftContext.Count - 1;

        // Checking for context for left context
        int exploreIndex = index - 1;
        int neighbourIndex = leftContext.Count - 1;
        while(exploreIndex >= 0)
        {
            // Scenario 1: No bracketing in left context 
            if(!leftContextHasBrackets)
            {
                if(!bracketed)
                {
                    if(LSystemString[exploreIndex].symbol == leftContext[neighbourIndex].symbol)
                    {
                        preClosedBracketIndex = neighbourIndex - 1;
                        neighbourIndex--;
                    }
                    else if(LSystemString[exploreIndex].symbol == ']')
                    {
                        bracketed = true;
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
                        bracketed = true;
                        closeBracketIndex = neighbourIndex;
                    } 
                    else if(!bracketed)
                    {
                        preClosedBracketIndex = neighbourIndex - 1;
                    }
                    neighbourIndex--;
                }
                // Finds raw opening bracket that is not a match with the left context -> Reset search parameters to before first closed bracket
                else if(LSystemString[exploreIndex].symbol == '[')
                {
                    neighbourIndex = preClosedBracketIndex;
                }
                else if(!bracketed)
                {
                    // match not found: Incorrect neighboring symbol from root
                    return null; 
                }
                else
                {
                    // Match not found in the bracket, must reset search within the bracket
                    neighbourIndex = closeBracketIndex;
                }
            } 

            exploreIndex--;
            if(neighbourIndex < 0)
            {
                return Successor;
            }
            if(exploreIndex < 0)
            {
                return null;
            }
        }

        // Checking for context for right neighbour (reset local variables here)
        exploreIndex = index + 1;
        neighbourIndex = 0;
        bracketed = false;

        // Need to memorise index of the right context if an open bracket is discovered and index of the open bracket
        int preOpenBracketIndex = 0;
        int openBracketIndex = 0;

        while (exploreIndex < LSystemString.Count)
        {
            // Scenario 1: No brackets in right context
            if(!rightContextHasBrackets)
            {
                if(!bracketed)
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
                        bracketed = true;
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
                    }
                }
            }

            /**
            // Scenario 2: brackets in the right context
            else
            {
                // Finds immediate neighbour to be a match -> Progress made in search
                if(LSystemString[exploreIndex].symbol == rightContext[neighbourIndex].symbol)
                {
                    if(LSystemString[exploreIndex].symbol == '[')
                    {
                        bracketed = true;
                        openBracketIndex = neighbourIndex;
                    } 
                    else if(!bracketed)
                    {
                        preOpenBracketIndex = neighbourIndex + 1;
                    }
                    neighbourIndex++;
                }
                // Finds closing bracket that is not a match with the left context -> Reset search parameters to before first open bracket
                else if(LSystemString[exploreIndex].symbol == ']' && bracketed)
                {
                    neighbourIndex = preOpenBracketIndex;
                }
                else if(LSystemString[exploreIndex].symbol != '[' && !bracketed)
                {
                    // match not found: Incorrect neighboring symbol from root
                    return null; 
                }
                else
                {
                    // Match not found in the bracket, must reset search within the bracket
                    neighbourIndex = openBracketIndex;
                }   
            }
            **/

            exploreIndex++;
            if(neighbourIndex >= rightContext.Count)
            {
                return Successor;
            }
            if(exploreIndex >= LSystemString.Count)
            {
                return null;
            }
        }
        return null;
    }
}
