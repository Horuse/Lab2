using System;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace DanceSchool.Ui.ViewModels.Instructors
{
    public partial class InstructorItemViewModel : ReactiveObject
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Specialization { get; set; }
        public DateTime HireDate { get; set; }
        public int GroupsCount { get; set; }
        public int ClassesCount { get; set; }
        
        [Reactive]
        private bool _isSelected;
    }
}