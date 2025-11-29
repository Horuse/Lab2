using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using DanceSchool.Data.Entities;
using DanceSchool.Ui.Services;
using ShadUI;

namespace DanceSchool.Ui.ViewModels.Groups
{
    public partial class GroupsViewModel : ViewModelBase
    {
        private readonly GroupService _groupService;
        private readonly DialogManager _dialogManager;
        private readonly IServiceProvider _serviceProvider;

        public ObservableCollection<GroupItemViewModel> Groups { get; } = new();

        [Reactive]
        private bool _isLoading;

        public GroupsViewModel(GroupService groupService, DialogManager dialogManager, IServiceProvider serviceProvider)
        {
            _groupService = groupService;
            _dialogManager = dialogManager;
            _serviceProvider = serviceProvider;
        }

        [ReactiveCommand]
        private void AddGroup()
        {
            var addGroupViewModel = _serviceProvider.GetRequiredService<AddGroupViewModel>();
            addGroupViewModel.Initialize();
            
            _dialogManager.CreateDialog(addGroupViewModel)
                .Dismissible()
                .WithSuccessCallback(async vm =>
                {
                    await LoadGroups();
                })
                .Show();
        }

        [ReactiveCommand]
        public async Task LoadGroups()
        {
            IsLoading = true;
            try
            {
                var groups = await _groupService.GetAllGroupsAsync();
                Groups.Clear();
                
                foreach (var group in groups)
                {
                    Groups.Add(new GroupItemViewModel
                    {
                        Id = group.Id,
                        Name = group.Name,
                        InstructorsNames = string.Join(", ", group.GroupInstructors.Select(gi => $"{gi.Instructor?.FirstName} {gi.Instructor?.LastName}").Where(n => !string.IsNullOrEmpty(n))),
                        StudentsCount = group.StudentGroups?.Count ?? 0,
                        AgeCategory = group.AgeCategory,
                        SkillLevel = group.SkillLevel,
                        Schedule = group.Schedule ?? string.Empty,
                        MaxCapacity = group.MaxCapacity
                    });
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        private async Task EditGroup(int id)
        {
            var addGroupViewModel = _serviceProvider.GetRequiredService<AddGroupViewModel>();
            await addGroupViewModel.InitializeForEdit(id);
            
            _dialogManager.CreateDialog(addGroupViewModel)
                .Dismissible()
                .WithSuccessCallback(async vm =>
                {
                    await LoadGroups();
                })
                .Show();
        }

        [ReactiveCommand]
        private async Task DeleteGroupAsync(int id)
        {
            await _groupService.DeleteGroupAsync(id);
            await LoadGroups();
        }
    }
}