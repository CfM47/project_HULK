namespace Hulk
{
    public abstract class HulkDeclaration : HulkExpression
    {
        public abstract void AddToMemory(Memory Mem);
        public override object GetValue(bool execute)
        {
            return new EmptyReturn();
        }
    }
}