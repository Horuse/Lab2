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
            
            var group = new Group
            {
                Name = Name,
                AgeCategory = AgeCategory,
                SkillLevel = SkillLevel,
                MaxCapacity = MaxCapacity,
                Schedule = Schedule
            };

            await _groupService.CreateGroupAsync(group);
            
            _dialogManager.Close(this, new CloseDialogOptions { Success = true });
        }

        [ReactiveCommand]
        private void Cancel()
        {
            _dialogManager.Close(this);
        }

        public async void Initialize()
        {
            Name = string.Empty;
            AgeCategory = AgeCategory.Kids4_5;
            SkillLevel = SkillLevel.Starter;
            MaxCapacity = 10;
            Schedule = string.Empty;
            
            // Load available instructors
            var instructors = await _instructorService.GetAllInstructorsAsync();
            AvailableInstructors = instructors.ToList();
        }
    }
}