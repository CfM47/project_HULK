using System.Collections.ObjectModel;
using System.ComponentModel;
using Hulk;

namespace AppInterface.ViewModel;
public class MainViewModel : INotifyPropertyChanged
{
    string inputText;
    string outputText;
    Kompiler hulkKompiler;
    ObservableCollection<string> functionsList;
    public Command RunLine { get; private set; }
    public Command Clean { get; private set; }
    public MainViewModel()
    {
        outputText = ">";
        RunLine = new Command(ExecuteRunLine);
        Clean = new Command(CleanProgram);
        hulkKompiler = new(PrintOutput);
    }
    public void PrintOutput(object output) => OutputText += "\n" + output.ToString();
    private string[] GetFunctionItems()
    {
        Dictionary<string, FunctionDeclaration> Functions = hulkKompiler.Memory.FunctionsStorage;
        string[] items = new string[Functions.Count];
        int i = 0;
        foreach (string name in Functions.Keys)
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
    private void ExecuteRunLine()
    {
        if (string.IsNullOrEmpty(InputText))
            return;
        OutputText += InputText;
        hulkKompiler.Compile(InputText);
        OutputText += "\n>";
        InputText = "";
        Functions = new(GetFunctionItems());
    }
    private void CleanProgram()
    {
        hulkKompiler.Clear();
        Functions = new(GetFunctionItems());
        OutputText = ">";
    }
    public string InputText
    {
        get => inputText;
        set
        {
            inputText = value;
            OnPropertyChanged(nameof(InputText));
        }
    }
    public string OutputText
    {
        get => outputText;
        set
        {
            outputText = value;
            OnPropertyChanged(nameof(OutputText));
        }
    }
    public ObservableCollection<string> Functions
    {
        get => functionsList;
        set
        {
            functionsList = value;
            OnPropertyChanged(nameof(Functions));
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));    
}
