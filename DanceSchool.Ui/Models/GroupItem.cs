using ReactiveUI;
using ReactiveUI.SourceGenerators;
using DanceSchool.Data.Enums;

namespace DanceSchool.Ui.ViewModels.Groups
{
    public partial class GroupItemViewModel : ReactiveObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string InstructorsNames { get; set; } = string.Empty;
        public int StudentsCount { get; set; }
        public AgeCategory AgeCategory { get; set; }
        public SkillLevel SkillLevel { get; set; }
        public string Schedule { get; set; } = string.Empty;
        public int MaxCapacity { get; set; }
        
        [Reactive]
        private bool _isSelected;
    }
}