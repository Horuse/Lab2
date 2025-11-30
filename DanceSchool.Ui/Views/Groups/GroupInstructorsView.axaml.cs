using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DanceSchool.Ui.Views.Groups;

public partial class GroupInstructorsView : UserControl
{
    public GroupInstructorsView()
    {
        InitializeComponent();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}