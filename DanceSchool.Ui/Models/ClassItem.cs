using System;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using DanceSchool.Data.Enums;

namespace DanceSchool.Ui.ViewModels.Classes
{
    public partial class ClassItemViewModel : ReactiveObject
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Duration => $"{(EndTime - StartTime).TotalHours:F1}г";
        public ClassType ClassType { get; set; }
        public string? Topic { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public string StudioName { get; set; } = string.Empty;
        public int AttendanceCount { get; set; }
        
        [Reactive]
        private bool _isSelected;
    }
}