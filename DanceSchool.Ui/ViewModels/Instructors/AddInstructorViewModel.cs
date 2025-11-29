using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using DanceSchool.Data.Entities;
using DanceSchool.Ui.Services;
using ShadUI;

namespace DanceSchool.Ui.ViewModels.Instructors
{
    public sealed partial class AddInstructorViewModel : ViewModelBase
    {
        private readonly InstructorService _instructorService;
        private readonly DialogManager _dialogManager;

        [Reactive]
        [Required(ErrorMessage = "Ім'я є обов'язковим")]
        private string _firstName = string.Empty;

        [Reactive]
        [Required(ErrorMessage = "Прізвище є обов'язковим")]
        private string _lastName = string.Empty;

        [Reactive]
        private string _phoneNumber = string.Empty;

        [Reactive]
        [Required(ErrorMessage = "Пошта є обов'язковою")]
        [EmailAddress(ErrorMessage = "Некоректний формат пошти")]
        private string _email = string.Empty;

        [Reactive]
        private string _specialization = string.Empty;

        [Reactive]
        private DateTime _hireDate = DateTime.Now;

        public AddInstructorViewModel(InstructorService instructorService, DialogManager dialogManager)
        {
            _instructorService = instructorService;
            _dialogManager = dialogManager;
        }

        [ReactiveCommand]
        private async Task Submit()
        {
            ClearAllErrors();
            ValidateAllProperties();
            
            if (HasErrors) return;
            
            var instructor = new Instructor
            {
                FirstName = FirstName,
                LastName = LastName,
                PhoneNumber = PhoneNumber,
                Email = Email,
                Specialization = Specialization,
                HireDate = HireDate
            };

            await _instructorService.CreateInstructorAsync(instructor);
            
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
            PhoneNumber = string.Empty;
            Email = string.Empty;
            Specialization = string.Empty;
            HireDate = DateTime.Now;
        }
    }
}