using System.Collections.ObjectModel;
using System.ComponentModel;
using AppInterface.Views.Mobile;
using Hulk;

namespace AppInterface.ViewModel;
public class MobileMainViewModel : INotifyPropertyChanged
{
    string inputText;
    string outputText;
    Kompiler hulkKompiler;
    ObservableCollection<string> functionsList;
    public Command RunLine { get; private set; }
    public Command Clean { get; private set; }
    public MobileMainViewModel()
    {
        outputText = ">";
        RunLine = new Command(ExecuteRunLine);
        Clean = new Command(CleanProgram);
        hulkKompiler = new(PrintOutput);
    }
    public void PrintOutput(object output) => OutputText += "\n" + output.ToString();
    private string GetLastFunctionName()
    {
        Dictionary<string, FunctionDeclaration> functions = hulkKompiler.Memory.FunctionsStorage;
        var funcNames = functions.Keys;
        var lastFunc = funcNames.ElementAt(funcNames.Count - 1);
        string args = lastFunc + "(";
        if (functions[lastFunc].ArgumentNames.Count == 0)
            args += ")";
        for (int i = 0; i < functions[lastFunc].ArgumentNames.Count; i++)
        {
            string argument = functions[lastFunc].ArgumentNames[i];
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
    private void ExecuteRunLine()
    {
        if (string.IsNullOrEmpty(InputText))
            return;
        OutputText += InputText;
        hulkKompiler.Compile(InputText);
        OutputText += "\n>";
        InputText = "";
        Functions ??= new();
        if (Functions.Count < hulkKompiler.Memory.FunctionsStorage.Count)
            Functions.Add(GetLastFunctionName());
    }
    private void CleanProgram()
    {
        hulkKompiler.Clear();
        Functions = new();
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
