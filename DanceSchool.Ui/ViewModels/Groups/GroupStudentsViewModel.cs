using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using DanceSchool.Data.Entities;
using DanceSchool.Ui.Services;
using Microsoft.Extensions.DependencyInjection;
using ShadUI;

namespace DanceSchool.Ui.ViewModels.Groups
{
    public partial class GroupStudentsViewModel : ViewModelBase
    {
        private readonly GroupService _groupService;
        private readonly StudentService _studentService;
        private readonly DialogManager _dialogManager;

        [Reactive]
        private bool _isLoading;

        [Reactive]
        private Group? _selectedGroup;

        [Reactive]
        private string _searchText = string.Empty;

        [Reactive]
        private string _availableStudentsSearchText = string.Empty;

        public ObservableCollection<StudentInGroupItem> GroupStudents { get; } = new();
        public ObservableCollection<StudentInGroupItem> FilteredGroupStudents { get; } = new();
        public ObservableCollection<StudentInGroupItem> AvailableStudents { get; } = new();
        public ObservableCollection<StudentInGroupItem> FilteredAvailableStudents { get; } = new();

        public GroupStudentsViewModel(GroupService groupService, StudentService studentService, DialogManager dialogManager)
        {
            _groupService = groupService;
            _studentService = studentService;
            _dialogManager = dialogManager;

            this.WhenAnyValue(x => x.SearchText)
                .Subscribe(_ => FilterGroupStudents());

            this.WhenAnyValue(x => x.AvailableStudentsSearchText)
                .Subscribe(_ => FilterAvailableStudents());
        }

        public async Task InitializeAsync(int groupId)
        {
            IsLoading = true;
            try
            {
                // Load group details with students
                SelectedGroup = await _groupService.GetGroupWithDetailsAsync(groupId);

                await LoadGroupStudentsAsync();
                await LoadAvailableStudentsAsync();
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        public async Task LoadGroupStudentsAsync()
        {
            if (SelectedGroup == null) return;

            IsLoading = true;
            try
            {
                var group = await _groupService.GetGroupWithDetailsAsync(SelectedGroup.Id);
                
                GroupStudents.Clear();
                if (group?.StudentGroups != null)
                {
                    foreach (var studentGroup in group.StudentGroups)
                    {
                        if (studentGroup.Student != null)
                        {
                            GroupStudents.Add(new StudentInGroupItem
                            {
                                StudentId = studentGroup.Student.Id,
                                FirstName = studentGroup.Student.FirstName ?? string.Empty,
                                LastName = studentGroup.Student.LastName ?? string.Empty,
                                Email = studentGroup.Student.Email ?? string.Empty,
                                PhoneNumber = studentGroup.Student.PhoneNumber ?? string.Empty,
                                JoinedDate = studentGroup.EnrollmentDate
                            });
                        }
                    }
                }
                FilterGroupStudents();
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        public async Task LoadAvailableStudentsAsync()
        {
            if (SelectedGroup == null) return;

            IsLoading = true;
            try
            {
                var allStudents = await _studentService.GetAllStudentsAsync();
                var groupStudentIds = GroupStudents.Select(gs => gs.StudentId).ToHashSet();

                AvailableStudents.Clear();
                foreach (var student in allStudents.Where(s => !groupStudentIds.Contains(s.Id)))
                {
                    AvailableStudents.Add(new StudentInGroupItem
                    {
                        StudentId = student.Id,
                        FirstName = student.FirstName ?? string.Empty,
                        LastName = student.LastName ?? string.Empty,
                        Email = student.Email ?? string.Empty,
                        PhoneNumber = student.PhoneNumber ?? string.Empty
                    });
                }
                FilterAvailableStudents();
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        public async Task AddStudentToGroupAsync(int studentId)
        {
            if (SelectedGroup == null || IsLoading) return;

            IsLoading = true;
            try
            {
                await _groupService.AddStudentToGroupAsync(SelectedGroup.Id, studentId);
                await LoadGroupStudentsAsync();
                await LoadAvailableStudentsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding student to group: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        public async Task RemoveStudentFromGroupAsync(int studentId)
        {
            if (SelectedGroup == null || IsLoading) return;

            IsLoading = true;
            try
            {
                await _groupService.RemoveStudentFromGroupAsync(SelectedGroup.Id, studentId);
                await LoadGroupStudentsAsync();
                await LoadAvailableStudentsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing student from group: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void FilterGroupStudents()
        {
            FilteredGroupStudents.Clear();
            
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                foreach (var student in GroupStudents)
                {
                    FilteredGroupStudents.Add(student);
                }
            }
            else
            {
                var searchLower = SearchText.ToLowerInvariant();
                var filtered = GroupStudents.Where(s => 
                    (s.FirstName?.ToLowerInvariant().Contains(searchLower) ?? false) ||
                    (s.LastName?.ToLowerInvariant().Contains(searchLower) ?? false) ||
                    (s.Email?.ToLowerInvariant().Contains(searchLower) ?? false));
                
                foreach (var student in filtered)
                {
                    FilteredGroupStudents.Add(student);
                }
            }
        }

        private void FilterAvailableStudents()
        {
            FilteredAvailableStudents.Clear();
            
            if (string.IsNullOrWhiteSpace(AvailableStudentsSearchText))
            {
                foreach (var student in AvailableStudents)
                {
                    FilteredAvailableStudents.Add(student);
                }
            }
            else
            {
                var searchLower = AvailableStudentsSearchText.ToLowerInvariant();
                var filtered = AvailableStudents.Where(s => 
                    (s.FirstName?.ToLowerInvariant().Contains(searchLower) ?? false) ||
                    (s.LastName?.ToLowerInvariant().Contains(searchLower) ?? false) ||
                    (s.Email?.ToLowerInvariant().Contains(searchLower) ?? false));
                
                foreach (var student in filtered)
                {
                    FilteredAvailableStudents.Add(student);
                }
            }
        }
    }

    public class StudentInGroupItem
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime? JoinedDate { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}