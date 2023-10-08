using Hulk;
using System.Xml.Linq;

namespace FormInterface;

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
    public void PrintOutput(object output) => Output.Text += "\n" + output.ToString();
    private void Run_Click(object sender, EventArgs e)
    {
        if (Input.Text != "")
        {
            Output.Text += Input.Text;
            Compiler.Compile(Input.Text);
            Output.Text += "\n>";
            Input.Text = "";
            if (FunctionsList.Items.Count < Compiler.Memory.FunctionsStorage.Count)
                FunctionsList.Items.Add(GetLastFunctionName());
            FunctionsList.Refresh();
        }
    }
    private string GetLastFunctionName()
    {
        Dictionary<string, FunctionDeclaration> functions = Compiler.Memory.FunctionsStorage;
        var funcNames = functions.Keys;
        var lastFunc = funcNames.ElementAt(funcNames.Count-1);
        string args = lastFunc + "(";
        if (functions[lastFunc].ArgumentNames.Count == 0)
            args += ")";
        for (int i = 0; i < functions[lastFunc].ArgumentNames.Count; i++) 
        {
            string? argument = functions[lastFunc].ArgumentNames[i];
            if (functions[lastFunc].ArgumentNames.Count == 1)
                return $"{lastFunc}({argument})";
            else if (i == functions[lastFunc].ArgumentNames.Count - 1)
            {
                args += argument + ")";
                continue;
            }
            args += argument + ",";
        }
        return args;
    }
    private void CleanButton_Click(object sender, EventArgs e)
    {
        Compiler.Clear();
        FunctionsList.Items.Clear();
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

    private void MainWindow_FormClosing(object sender, FormClosingEventArgs e) => Parent.Dispose();
}