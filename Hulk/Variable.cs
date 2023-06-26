using Hulk;
using System;

public abstract class Variable
{
}
public class Numbr: Variable
{
    public Numbr(double value)
    {
        Value = value;
    }
    public double Value { get; set;}
}
public class Booln: Variable
{
    public Booln(bool value)
    {
        Value = value;
    }
    public bool Value { get; set;}
}
public class Strg : Variable
{
    public Strg(string value)
    {
        Value = value;
    }
    public string Value { get; set;}
}