using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DanceSchool.Ui.Views.Groups;

public partial class GroupStudentsView : UserControl
{
    public GroupStudentsView()
    {
        InitializeComponent();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}