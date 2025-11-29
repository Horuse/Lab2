using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using DanceSchool.Data.Entities;
using DanceSchool.Data.Enums;
using DanceSchool.Ui.Services;
using ShadUI;
using Window = ShadUI.Window;

namespace DanceSchool.Ui.ViewModels.Students
{
    public sealed partial class AddStudentViewModel : ViewModelBase
    {
        private readonly StudentService _studentService;
        private readonly DialogManager _dialogManager;

        [Reactive]
        [Required(ErrorMessage = "Ім'я є обов'язковим")]
        private string _firstName = string.Empty;

        [Reactive]
        [Required(ErrorMessage = "Прізвище є обов'язковим")]
        private string _lastName = string.Empty;

        [Reactive]
        [Required(ErrorMessage = "Дата народження є обов'язковою")]
        private DateTime _dateOfBirth = DateTime.Now.AddYears(-18);

        [Reactive]
        [Required(ErrorMessage = "Номер телефона є обов'язковим")]
        private string _phoneNumber = string.Empty;

        [Reactive]
        [Required(ErrorMessage = "Пошта є обов'язковою")]
        private string _email = string.Empty;

        [Reactive]
        [Required(ErrorMessage = "Рівень навичок є обов'язковим")]
        private SkillLevel _skillLevel = SkillLevel.Starter;

        public static List<SkillLevel> SkillLevels => new()
        {
            SkillLevel.Starter,
            SkillLevel.Intermediate,
            SkillLevel.PrePerformance
        };

        public AddStudentViewModel(DialogManager dialogManager, StudentService studentService)
        {
            _dialogManager = dialogManager;
            _studentService = studentService;
        }

        [ReactiveCommand]
        private async Task Submit()
        {
            ClearAllErrors();
            ValidateAllProperties();
            // if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
                
            if (HasErrors) return;
            
            var student = new Student
            {
                FirstName = FirstName,
                LastName = LastName,
                DateOfBirth = DateOfBirth,
                PhoneNumber = PhoneNumber,
                Email = Email,
                SkillLevel = SkillLevel,
                RegistrationDate = DateTime.Now
            };

            await _studentService.CreateStudentAsync(student);
            
            _dialogManager.Close(this, new CloseDialogOptions { Success = true });
        }

        [ReactiveCommand]
        private void Cancel()
        {
            _dialogManager.Close(this);
        }

        public void Initialize()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            DateOfBirth = DateTime.Now.AddYears(-18);
            PhoneNumber = string.Empty;
            Email = string.Empty;
            SkillLevel = SkillLevel.Starter;
        }
    }
}