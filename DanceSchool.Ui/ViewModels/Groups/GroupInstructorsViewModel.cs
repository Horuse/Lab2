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
    public partial class GroupInstructorsViewModel : ViewModelBase
    {
        private readonly GroupService _groupService;
        private readonly InstructorService _instructorService;
        private readonly DialogManager _dialogManager;

        [Reactive]
        private bool _isLoading;

        [Reactive]
        private Group? _selectedGroup;

        [Reactive]
        private string _searchText = string.Empty;

        [Reactive]
        private string _availableInstructorsSearchText = string.Empty;

        public ObservableCollection<InstructorInGroupItem> GroupInstructors { get; } = new();
        public ObservableCollection<InstructorInGroupItem> FilteredGroupInstructors { get; } = new();
        public ObservableCollection<InstructorInGroupItem> AvailableInstructors { get; } = new();
        public ObservableCollection<InstructorInGroupItem> FilteredAvailableInstructors { get; } = new();

        public GroupInstructorsViewModel(GroupService groupService, InstructorService instructorService, DialogManager dialogManager)
        {
            _groupService = groupService;
            _instructorService = instructorService;
            _dialogManager = dialogManager;

            this.WhenAnyValue(x => x.SearchText)
                .Subscribe(_ => FilterGroupInstructors());

            this.WhenAnyValue(x => x.AvailableInstructorsSearchText)
                .Subscribe(_ => FilterAvailableInstructors());
        }

        public async Task InitializeAsync(int groupId)
        {
            IsLoading = true;
            try
            {
                // Load group details with instructors
                SelectedGroup = await _groupService.GetGroupWithDetailsAsync(groupId);

                await LoadGroupInstructorsAsync();
                await LoadAvailableInstructorsAsync();
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        public async Task LoadGroupInstructorsAsync()
        {
            if (SelectedGroup == null) return;

            IsLoading = true;
            try
            {
                var group = await _groupService.GetGroupWithDetailsAsync(SelectedGroup.Id);
                
                GroupInstructors.Clear();
                if (group?.GroupInstructors != null)
                {
                    foreach (var groupInstructor in group.GroupInstructors)
                    {
                        if (groupInstructor.Instructor != null)
                        {
                            GroupInstructors.Add(new InstructorInGroupItem
                            {
                                InstructorId = groupInstructor.Instructor.Id,
                                FirstName = groupInstructor.Instructor.FirstName ?? string.Empty,
                                LastName = groupInstructor.Instructor.LastName ?? string.Empty,
                                Email = groupInstructor.Instructor.Email ?? string.Empty,
                                PhoneNumber = groupInstructor.Instructor.PhoneNumber ?? string.Empty,
                                Specialization = groupInstructor.Instructor.Specialization ?? string.Empty,
                                AssignedDate = groupInstructor.AssignedDate
                            });
                        }
                    }
                }
                FilterGroupInstructors();
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        public async Task LoadAvailableInstructorsAsync()
        {
            if (SelectedGroup == null) return;

            IsLoading = true;
            try
            {
                var allInstructors = await _instructorService.GetAllInstructorsAsync();
                var groupInstructorIds = GroupInstructors.Select(gi => gi.InstructorId).ToHashSet();

                AvailableInstructors.Clear();
                foreach (var instructor in allInstructors.Where(i => !groupInstructorIds.Contains(i.Id)))
                {
                    AvailableInstructors.Add(new InstructorInGroupItem
                    {
                        InstructorId = instructor.Id,
                        FirstName = instructor.FirstName ?? string.Empty,
                        LastName = instructor.LastName ?? string.Empty,
                        Email = instructor.Email ?? string.Empty,
                        PhoneNumber = instructor.PhoneNumber ?? string.Empty,
                        Specialization = instructor.Specialization ?? string.Empty
                    });
                }
                FilterAvailableInstructors();
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        public async Task AssignInstructorToGroupAsync(int instructorId)
        {
            if (SelectedGroup == null || IsLoading) return;

            IsLoading = true;
            try
            {
                await _groupService.AssignInstructorToGroupAsync(SelectedGroup.Id, instructorId);
                await LoadGroupInstructorsAsync();
                await LoadAvailableInstructorsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning instructor to group: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        public async Task RemoveInstructorFromGroupAsync(int instructorId)
        {
            if (SelectedGroup == null || IsLoading) return;

            IsLoading = true;
            try
            {
                await _groupService.RemoveInstructorFromGroupAsync(SelectedGroup.Id, instructorId);
                await LoadGroupInstructorsAsync();
                await LoadAvailableInstructorsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing instructor from group: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void FilterGroupInstructors()
        {
            FilteredGroupInstructors.Clear();
            
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                foreach (var instructor in GroupInstructors)
                {
                    FilteredGroupInstructors.Add(instructor);
                }
            }
            else
            {
                var searchLower = SearchText.ToLowerInvariant();
                var filtered = GroupInstructors.Where(i => 
                    (i.FirstName?.ToLowerInvariant().Contains(searchLower) ?? false) ||
                    (i.LastName?.ToLowerInvariant().Contains(searchLower) ?? false) ||
                    (i.Email?.ToLowerInvariant().Contains(searchLower) ?? false) ||
                    (i.Specialization?.ToLowerInvariant().Contains(searchLower) ?? false));
                
                foreach (var instructor in filtered)
                {
                    FilteredGroupInstructors.Add(instructor);
                }
            }
        }

        private void FilterAvailableInstructors()
        {
            FilteredAvailableInstructors.Clear();
            
            if (string.IsNullOrWhiteSpace(AvailableInstructorsSearchText))
            {
                foreach (var instructor in AvailableInstructors)
                {
                    FilteredAvailableInstructors.Add(instructor);
                }
            }
            else
            {
                var searchLower = AvailableInstructorsSearchText.ToLowerInvariant();
                var filtered = AvailableInstructors.Where(i => 
                    (i.FirstName?.ToLowerInvariant().Contains(searchLower) ?? false) ||
                    (i.LastName?.ToLowerInvariant().Contains(searchLower) ?? false) ||
                    (i.Email?.ToLowerInvariant().Contains(searchLower) ?? false) ||
                    (i.Specialization?.ToLowerInvariant().Contains(searchLower) ?? false));
                
                foreach (var instructor in filtered)
                {
                    FilteredAvailableInstructors.Add(instructor);
                }
            }
        }
    }

    public class InstructorInGroupItem
    {
        public int InstructorId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public DateTime? AssignedDate { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}