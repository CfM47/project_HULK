using System;
using System.Reflection.Metadata.Ecma335;

namespace Hulk
{
	public class Memory
	{
		public Memory()
		{
			VariablesStorage = new Dictionary<string, Variable>();
			FunctionsStorage = new Dictionary<string, FunctionDeclaration>();
		}
		public void AddNewVariable(string key, Variable Val)
		{
			if (!VariablesStorage.TryAdd(key, Val))
				throw new Exception();
		}
		public void AddNewFunction(string key, FunctionDeclaration Val)
		{
			if (!FunctionsStorage.TryAdd(key, Val))
				throw new Exception();
		}
		public Dictionary<string, Variable> VariablesStorage { get;}
		public Dictionary<string, FunctionDeclaration> FunctionsStorage { get;}
	}
}