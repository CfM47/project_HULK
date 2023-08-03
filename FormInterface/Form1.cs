using Hulk;
using Interface;
using System.CodeDom.Compiler;

namespace FormInterface
{
    public partial class MainWindow : Form
    {
        Kompiler Compiler;
        public MainWindow()
        {
            InitializeComponent();
            Compiler = new(PrintOutput);
        }
        public void PrintOutput(object output)
        {
            Output.Text += "\n" + output.ToString() + "\n>";
        }        
        private void Run_Click(object sender, EventArgs e)
        {
            if (Input.Text != "")
            {
                Output.Text += Output.Text == "" ? Input.Text : "\n" + Input.Text;
                Compiler.Compile(Input.Text);
                Input.Text = "";

                VariablesList.Items.Clear();
                VariablesList.Items.AddRange(GetVariableItems());
                VariablesList.Refresh();
                FunctionsList.Items.Clear();
                FunctionsList.Items.AddRange(GetFunctionItems());
                FunctionsList.Refresh();
            }
        }
        private string[] GetVariableItems()
        {
            Dictionary<string, Variable> Variables = Compiler.Memory.VariablesStorage;
            string[] items = new string[Variables.Count];
            int i = 0;
            foreach (var name in Variables.Keys)
            {
                items[i] = $"{Variables[name].Type} {name} = {Variables[name].Value}";
                i++;
            }
            return items;
        }
        private string[] GetFunctionItems()
        {
            Dictionary<string, FunctionDeclaration> Functions = Compiler.Memory.FunctionsStorage;
            string[] items = new string[Functions.Count];
            int i = 0;
            foreach (var name in Functions.Keys)
            {
                string args = name + "(";
                int argumentsCount = Functions[name].ArgumentNames.Count;
                if (argumentsCount == 1)
                {
                    args = name + "(" + Functions[name].ArgumentNames[0] + ")";
                    items[i] = args;
                    i++;
                    continue;
                }
                for (int j = 0; j < argumentsCount; j++)
                {
                    string? arg = Functions[name].ArgumentNames[j];
                    args += j == argumentsCount - 1 ? arg : arg + ", ";
                }
                args += ")";
                items[i] = args;
                i++;
            }
            return items;
        }
    }
}