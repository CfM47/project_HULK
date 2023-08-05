namespace Hulk
{
    public class IfElseStatement : HulkExpression
    {
        public IfElseStatement(HulkExpression Cond, HulkExpression IfExp, HulkExpression ElseExp)
        {
            if (Cond.IsDependent || IfExp.IsDependent || ElseExp.IsDependent)
                IsDependent = true;
            Condition = Cond;
            IfExpression = IfExp;
            ElseExpression = ElseExp;

        }
        #region Methods
        public override object GetValue(bool execute)
        {
            if (!execute)
                return CheckValue();
            return Result(Condition, IfExpression, ElseExpression, execute);
        }
        private object CheckValue()
        {
            if(Condition is Variable && Condition.GetValue(false) == null) 
            {
                var ifValue = IfExpression.GetValue(false);
                var elseValue = ElseExpression.GetValue(false);
                return ifValue;
            }
            else
                return Result(Condition, IfExpression, ElseExpression, false);

        }
        private object Result(HulkExpression Cond, HulkExpression IfExp, HulkExpression ElseExp, bool execute)
        {
            if (Cond.GetValue(execute) is not bool)
                throw new SemanticError("if-else condition", "boolean", Cond.GetValue(execute).GetHulkTypeAsString());
            else
            {
                var condition = (bool)Cond.GetValue(execute);
                if (condition)
                    return IfExp.GetValue(execute);
                if (IfExp != null)
                    if (ElseExp == null)
                        return null;
                return ElseExp.GetValue(execute);
            }
        }
        #endregion
        #region Properties
        public HulkExpression Condition { get; protected set; }
        public HulkExpression IfExpression { get; protected set; }
        public HulkExpression ElseExpression { get; protected set; }
        #endregion
    }
    public class LetInStatement : HulkExpression
    {
        public LetInStatement(Dictionary<string, Variable> Variables)
        {
            StoredVariables = Variables;
        }
        #region Methods
        public override object GetValue(bool execute)
        {
            CheckValues();
            return Body.GetValue(execute);
        }
        private void CheckValues()
        {
            foreach (var V in StoredVariables.Values)
            {
                bool isOK = false;
                object val = null;
                if (V.IsDependent)
                {
                    var Exp = V.Value as HulkExpression;
                    val = Exp.GetValue(false);
                    if (val != null)
                    {
                        if (V.Type == Types.boolean && val is bool)
                            isOK = true;
                        else if (V.Type == Types.hstring && val is string)
                            isOK = true;
                        else if (V.Type == Types.number && val is double)
                            isOK = true;
                        else if (V.Type == Types.dynamic)
                            isOK = true;
                    }
                    else
                        isOK = true;
                }
                else
                    isOK = true;
                if (!isOK)
                {
                    string expectedType = "";
                    switch (V.Type)
                    {
                        case Types.number:
                            expectedType = "number";
                            break;
                        case Types.boolean:
                            expectedType = "boolean";
                            break;
                        case Types.hstring:
                            expectedType = "string";
                            break;
                    }
                    throw new SemanticError("Let-in expression", expectedType, val.GetHulkTypeAsString());
                }
            }
        }
        public void Define(HulkExpression Definition)
        {
            Body = Definition;
        }
        #endregion
        #region Properties
        public Dictionary<string, Variable> StoredVariables { get; private set; }
        public HulkExpression Body { get; private set; }
        #endregion
    }
}