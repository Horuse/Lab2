using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DanceSchool.Ui.Views.Students
{
    public partial class AddStudentDialog : UserControl
    {
        public AddStudentDialog()
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}