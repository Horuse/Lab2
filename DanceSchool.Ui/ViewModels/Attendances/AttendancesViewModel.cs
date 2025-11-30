using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using DanceSchool.Data.Entities;
using DanceSchool.Data.Enums;
using DanceSchool.Ui.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DanceSchool.Ui.ViewModels.Attendances
{
    public partial class AttendancesViewModel : ViewModelBase
    {
        private readonly AttendanceService _attendanceService;

        [Reactive]
        private bool _isLoading;

        [Reactive]
        private bool _showGroupList = true;

        [Reactive]
        private bool _showAttendanceGrid = false;

        [Reactive]
        private Group? _selectedGroup;

        [Reactive]
        private string _searchText = string.Empty;

        public ObservableCollection<GroupAttendanceItem> Groups { get; } = new();
        public ObservableCollection<GroupAttendanceItem> FilteredGroups { get; } = new();
        public ObservableCollection<Student> Students { get; } = new();
        public ObservableCollection<Class> Classes { get; } = new();
        public ObservableCollection<AttendanceGridItem> AttendanceGrid { get; } = new();

        public AttendancesViewModel(AttendanceService attendanceService)
        {
            _attendanceService = attendanceService;

            this.WhenAnyValue(x => x.SearchText)
                .Subscribe(_ => FilterGroups());
        }

        [ReactiveCommand]
        public async Task LoadGroupsAsync()
        {
            IsLoading = true;
            try
            {
                var groups = await _attendanceService.GetGroupsForAttendanceAsync();
                Groups.Clear();
                
                foreach (var group in groups)
                {
                    Groups.Add(new GroupAttendanceItem
                    {
                        Id = group.Id,
                        Name = group.Name,
                        StudentsCount = group.StudentGroups?.Count ?? 0,
                        AgeCategory = group.AgeCategory,
                        SkillLevel = group.SkillLevel
                    });
                }
                
                FilterGroups();
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        public async Task SelectGroupAsync(int groupId)
        {
            IsLoading = true;
            try
            {
                var students = await _attendanceService.GetStudentsByGroupAsync(groupId);
                var classes = await _attendanceService.GetClassesByGroupAsync(groupId);

                Students.Clear();
                Classes.Clear();
                AttendanceGrid.Clear();

                foreach (var student in students)
                {
                    Students.Add(student);
                }

                foreach (var classItem in classes.OrderBy(c => c.Date))
                {
                    Classes.Add(classItem);
                }

                await BuildAttendanceGridAsync();

                ShowGroupList = false;
                ShowAttendanceGrid = true;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        public void BackToGroupList()
        {
            ShowAttendanceGrid = false;
            ShowGroupList = true;
            SelectedGroup = null;
        }

        [ReactiveCommand]
        public async Task ToggleAttendanceAsync(AttendanceItem attendanceItem)
        {
            try
            {
                var studentGridItem = AttendanceGrid.FirstOrDefault(g => g.Attendances.Contains(attendanceItem));
                if (studentGridItem == null) return;

                var newStatus = attendanceItem.IsPresent ? AttendanceStatus.Present : AttendanceStatus.Absent;

                await _attendanceService.MarkAttendanceAsync(studentGridItem.StudentId, attendanceItem.ClassId, newStatus);
                
                attendanceItem.Status = newStatus;
                attendanceItem.IsPresent = newStatus == AttendanceStatus.Present;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error toggling attendance: {ex.Message}");
                attendanceItem.IsPresent = !attendanceItem.IsPresent;
            }
        }

        private async Task BuildAttendanceGridAsync()
        {
            foreach (var student in Students)
            {
                var gridItem = new AttendanceGridItem
                {
                    StudentId = student.Id,
                    StudentName = $"{student.FirstName} {student.LastName}",
                    Attendances = new ObservableCollection<AttendanceItem>()
                };

                var classIds = Classes.Select(c => c.Id).ToList();
                var attendanceStatuses = await _attendanceService.GetAttendanceStatusForStudentAndClassesAsync(student.Id, classIds);

                foreach (var classItem in Classes)
                {
                    var status = attendanceStatuses.TryGetValue(classItem.Id, out var existingStatus) 
                        ? existingStatus 
                        : AttendanceStatus.Absent;

                    var attendanceItem = new AttendanceItem
                    {
                        ClassId = classItem.Id,
                        Date = classItem.Date
                    };
                    attendanceItem.Status = status;
                    attendanceItem.IsPresent = status == AttendanceStatus.Present;
                    gridItem.Attendances.Add(attendanceItem);
                }

                AttendanceGrid.Add(gridItem);
            }
        }

        private void FilterGroups()
        {
            FilteredGroups.Clear();
            
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                foreach (var group in Groups)
                {
                    FilteredGroups.Add(group);
                }
            }
            else
            {
                var searchLower = SearchText.ToLowerInvariant();
                var filtered = Groups.Where(g => 
                    (g.Name?.ToLowerInvariant().Contains(searchLower) ?? false) ||
                    g.AgeCategory.ToString().ToLowerInvariant().Contains(searchLower) ||
                    g.SkillLevel.ToString().ToLowerInvariant().Contains(searchLower));
                
                foreach (var group in filtered)
                {
                    FilteredGroups.Add(group);
                }
            }
        }
    }

    public class GroupAttendanceItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int StudentsCount { get; set; }
        public AgeCategory AgeCategory { get; set; }
        public SkillLevel SkillLevel { get; set; }
    }

    public partial class AttendanceGridItem : ReactiveObject
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public ObservableCollection<AttendanceItem> Attendances { get; set; } = new();
    }

    public partial class AttendanceItem : ReactiveObject
    {
        public int ClassId { get; set; }
        public DateTime Date { get; set; }
        
        [Reactive]
        private AttendanceStatus _status;
        
        [Reactive]
        private bool _isPresent;
    }
}