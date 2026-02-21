using UnityEngine;

public class Symbol
{
    public char symbol;

    public Symbol(char input)
    {
        symbol = input; 
    }

    public string expressAsString()
    {
        return "" + symbol;
    }
}
