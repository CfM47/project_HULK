namespace FormInterface;

public partial class TitleScreen : Form
{
    public TitleScreen()
    {
        InitializeComponent();
    }

    private void pictureBox1_Click(object sender, EventArgs e)
    {
        MainWindow mainWindow = new(this);
        mainWindow.Show();
        Hide();
    }
}
