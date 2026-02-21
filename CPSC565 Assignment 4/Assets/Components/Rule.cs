using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rule
{
    public Symbol Predecessor;
    public List<Symbol> Successor;

    public Rule(Symbol PreIn, List<Symbol> SucIn)
    {
        Predecessor = PreIn;
        Successor = SucIn;
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
}
