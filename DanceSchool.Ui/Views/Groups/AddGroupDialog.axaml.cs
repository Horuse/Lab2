using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DanceSchool.Ui.Views.Groups
{
    public partial class AddGroupDialog : UserControl
    {
        public AddGroupDialog()
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}