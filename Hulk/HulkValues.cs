using Hulk;
using System;

public abstract class HulkValue : HulkExpression
{
	public override HulkValue GetValue(bool execute)
	{ 
		return this;
	}
	public object? Value { get; set; }
	public override string ToString()
	{
		return Value == null? null: Value.ToString();
	}
}
public class HulkNumber : HulkValue
{
	public HulkNumber(double val)
	{
		Value = val;
	}
}
public class HulkBoolean : HulkValue
{
	public HulkBoolean(bool val)
	{
		Value = val;
	}
}
public class HulkString : HulkValue
{
	public HulkString(string val)
	{
		Value = val;
	}
}
public class AbstractValue : HulkValue
{
	public AbstractValue()
	{
		Value = null;
	}
}
public class EmptyReturn : HulkValue
{
    public EmptyReturn() { }
}
