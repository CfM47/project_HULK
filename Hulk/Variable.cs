using Hulk;
using System;

public abstract class Variable
{
}
public class HulkNumber: Variable
{
    public HulkNumber(double value)
    {
        Value = value;
    }
    public double Value { get; set;}
}
public class HulkBoolean: Variable
{
    public HulkBoolean(bool value)
    {
        Value = value;
    }
    public bool Value { get; set;}
}
public class HulkString : Variable
{
    public HulkString(string value)
    {
        Value = value;
    }
    public string Value { get; set;}
}