namespace Hulk
{
	public class HulkMemory
	{
		public HulkMemory()
		{
			FunctionsStorage = new Dictionary<string, FunctionDeclaration>();
		}
		public void AddNewFunction(string key, FunctionDeclaration Val)
		{
			if (!FunctionsStorage.TryAdd(key, Val))
				throw new DefaultError($"Function {key} already exist");
		}
		public Dictionary<string, FunctionDeclaration> FunctionsStorage { get; }
	}
}