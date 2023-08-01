﻿using Hulk;

namespace Interface
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Memory Memoria = new Memory();
            Tokenizer tokenizer = new Tokenizer(Memoria);
            while (true)
            {
                Console.Write(">");
                var input = Console.ReadLine();
                string[] s = tokenizer.GetTokens(input);
                if (input != null)
                {
                    try
                    {
                        HulkExpression exp = tokenizer.Parse(s);
                        if (exp is VariableDeclaration)
                        {
                            VariableDeclaration Vars = (VariableDeclaration)exp;
                            foreach (string name in Vars.Names)
                            {
                                var options = Variable.VariableOptions.InitializedVariable;
                                Variable newVar;
                                if (Vars.ValueExpression == null)
                                {
                                    options = Variable.VariableOptions.NonInitialized;
                                    newVar = new Variable(name, null, Vars.Type, options);

                                }
                                else
                                    newVar = new Variable(name, Vars.ValueExpression.GetValue(false), Vars.Type, options);
                                Memoria.AddNewVariable(name, newVar);
                            }
                        }
                        else if (exp is FunctionDeclaration)
                        {
                            FunctionDeclaration Function = (FunctionDeclaration)exp;
                            Memoria.AddNewFunction(Function.FunctionName, Function);
                        }
                        else
                        {
                            try
                            {
                                exp.GetValue(false);
                                exp.GetValue(true);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}