using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using DanceSchool.Data.Entities;
using DanceSchool.Ui.Services;

namespace DanceSchool.Ui.ViewModels.Groups
{
    public partial class GroupsViewModel : ViewModelBase
    {
        private readonly GroupService _groupService;

        [Reactive]
        private ObservableCollection<Group> _groups = new();

        [Reactive]
        private Group? _selectedGroup;

        [Reactive]
        private bool _isLoading;

        public GroupsViewModel(GroupService groupService)
        {
            _groupService = groupService;
        }

        [ReactiveCommand]
        private async Task LoadGroupsAsync()
        {
            IsLoading = true;
            try
            {
                var groups = await _groupService.GetAllGroupsAsync();
                Groups.Clear();
                foreach (var group in groups)
                {
                    Groups.Add(group);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        private async Task AddGroupAsync()
        {
            var newGroup = new Group
            {
                Name = "Нова група",
                AgeCategory = Data.Enums.AgeCategory.Kids4_5,
                SkillLevel = Data.Enums.SkillLevel.Starter,
                MaxCapacity = 10,
                Schedule = "Пн, Ср, Пт 18:00"
            };

            await _groupService.CreateGroupAsync(newGroup);
            await LoadGroupsAsync();
        }

        [ReactiveCommand]
        private async Task DeleteGroupAsync(int id)
        {
            await _groupService.DeleteGroupAsync(id);
            await LoadGroupsAsync();
        }
    }
}