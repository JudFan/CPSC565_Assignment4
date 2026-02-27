using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RuleRandom : Rule
{
    public List<List<Symbol>> PossibleSuccessors;

    private List<float> probDistr; 

    public RuleRandom(Symbol PreIn, List<List<Symbol>> SucIn, List<float> probIn)
    {
        Predecessor = PreIn;
        PossibleSuccessors = SucIn;
        probDistr = probIn;
    }

    public override List<Symbol> RuleChange(Symbol input)
    {
        float random = UnityEngine.Random.value;

        if (input.Equals(Predecessor))
        {
            Debug.Log("Match at " + input.expressAsString());
            int index = 0;
            while (index < PossibleSuccessors.Count)
            {
                random -= probDistr[index];
                if(random <= 0)
                {
                    break;
                }
                index++;
            }
            
            List<Symbol> result = null;
            try {
                result = PossibleSuccessors[index];
            }
            catch (Exception ex)
            {
                Debug.Log("Index is " + index + " while PossibleSuccessors has count " + PossibleSuccessors.Count);
            }
            return result;
        }
        else
        {
            return null;
        }
    }
}
