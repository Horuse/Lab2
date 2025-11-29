using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using DanceSchool.Data.Entities;
using DanceSchool.Data.Enums;
using DanceSchool.Ui.Services;
using ShadUI;

namespace DanceSchool.Ui.ViewModels.Groups
{
    public sealed partial class AddGroupViewModel : ViewModelBase
    {
        private readonly GroupService _groupService;
        private readonly InstructorService _instructorService;
        private readonly DialogManager _dialogManager;
        private int? _groupId; // null для додавання, id для редагування

        [Reactive]
        [Required(ErrorMessage = "Назва групи є обов'язковою")]
        private string _name = string.Empty;

        [Reactive]
        private AgeCategory _ageCategory = AgeCategory.Kids4_5;

        [Reactive]
        private SkillLevel _skillLevel = SkillLevel.Starter;

        [Reactive]
        private int _maxCapacity = 10;

        [Reactive]
        private string _schedule = string.Empty;

        [Reactive]
        private List<Instructor> _availableInstructors = new();

        [Reactive]
        private string _title = "Додати групу";

        [Reactive]
        private string _submitText = "Додати";

        public static List<AgeCategory> AgeCategories => Enum.GetValues<AgeCategory>().ToList();
        public static List<SkillLevel> SkillLevels => Enum.GetValues<SkillLevel>().ToList();

        public AddGroupViewModel(GroupService groupService, InstructorService instructorService, DialogManager dialogManager)
        {
            _groupService = groupService;
            _instructorService = instructorService;
            _dialogManager = dialogManager;
        }

        [ReactiveCommand]
        private async Task Submit()
        {
            ClearAllErrors();
            ValidateAllProperties();
            
            if (HasErrors) return;
            
            if (_groupId.HasValue)
            {
                // Редагування існуючої групи
                var group = await _groupService.GetGroupByIdAsync(_groupId.Value);
                if (group != null)
                {
                    group.Name = Name;
                    group.AgeCategory = AgeCategory;
                    group.SkillLevel = SkillLevel;
                    group.MaxCapacity = MaxCapacity;
                    group.Schedule = Schedule;

                    await _groupService.UpdateGroupAsync(group);
                }
            }
            else
            {
                // Додавання нової групи
                var group = new Group
                {
                    Name = Name,
                    AgeCategory = AgeCategory,
                    SkillLevel = SkillLevel,
                    MaxCapacity = MaxCapacity,
                    Schedule = Schedule
                };

                await _groupService.CreateGroupAsync(group);
            }
            
            _dialogManager.Close(this, new CloseDialogOptions { Success = true });
        }

        [ReactiveCommand]
        private void Cancel()
        {
            _dialogManager.Close(this);
        }

        public async void Initialize()
        {
            _groupId = null;
            Name = string.Empty;
            AgeCategory = AgeCategory.Kids4_5;
            SkillLevel = SkillLevel.Starter;
            MaxCapacity = 10;
            Schedule = string.Empty;
            Title = "Додати групу";
            SubmitText = "Додати";
            
            // Load available instructors
            var instructors = await _instructorService.GetAllInstructorsAsync();
            AvailableInstructors = instructors.ToList();
        }

        public async Task InitializeForEdit(int groupId)
        {
            _groupId = groupId;
            Title = "Редагувати групу";
            SubmitText = "Зберегти";
            
            // Load available instructors
            var instructors = await _instructorService.GetAllInstructorsAsync();
            AvailableInstructors = instructors.ToList();
            
            var group = await _groupService.GetGroupByIdAsync(groupId);
            if (group != null)
            {
                Name = group.Name;
                AgeCategory = group.AgeCategory;
                SkillLevel = group.SkillLevel;
                MaxCapacity = group.MaxCapacity;
                Schedule = group.Schedule ?? string.Empty;
            }
        }
    }
}