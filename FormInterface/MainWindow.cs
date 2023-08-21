using Hulk;

namespace FormInterface
{
    public partial class MainWindow : Form
    {
        Kompiler Compiler;
        TitleScreen Parent;
        public MainWindow(TitleScreen titleScreen)
        {
            InitializeComponent();
            Compiler = new(PrintOutput);
            Parent = titleScreen;
            Output.Text = ">";
        }
        public void PrintOutput(object output)
        {
            Output.Text += "\n" + output.ToString();
        }
        private void Run_Click(object sender, EventArgs e)
        {
            if (Input.Text != "")
            {
                Output.Text += Input.Text;
                Compiler.Compile(Input.Text);
                Output.Text += "\n>";
                Input.Text = "";
                FunctionsList.Items.Clear();
                FunctionsList.Items.AddRange(GetFunctionItems());
                FunctionsList.Refresh();
            }
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
        private void CleanButton_Click(object sender, EventArgs e)
        {
            Compiler.Clear();
            FunctionsList.Items.Clear();
            FunctionsList.Items.AddRange(GetFunctionItems());
            FunctionsList.Refresh();
            Output.Text = ">";
        }
        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                Run_Click(sender, e);
                e.Handled = true;
            }
        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.D)
            {
                CleanButton_Click(sender, e);
                e.Handled = true;
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Parent.Dispose();
        }
    }
}