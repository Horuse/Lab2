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
        private int? _instructorId; // null для додавання, id для редагування

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

        [Reactive]
        private string _title = "Додати інструктора";

        [Reactive]
        private string _submitText = "Додати";

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
            
            if (_instructorId.HasValue)
            {
                // Редагування існуючого інструктора
                var instructor = await _instructorService.GetInstructorByIdAsync(_instructorId.Value);
                if (instructor != null)
                {
                    instructor.FirstName = FirstName;
                    instructor.LastName = LastName;
                    instructor.PhoneNumber = PhoneNumber;
                    instructor.Email = Email;
                    instructor.Specialization = Specialization;
                    instructor.HireDate = HireDate;

                    await _instructorService.UpdateInstructorAsync(instructor);
                }
            }
            else
            {
                // Додавання нового інструктора
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
            }
            
            _dialogManager.Close(this, new CloseDialogOptions { Success = true });
        }

        [ReactiveCommand]
        private void Cancel()
        {
            _dialogManager.Close(this);
        }

        public void Initialize()
        {
            _instructorId = null;
            FirstName = string.Empty;
            LastName = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;
            Specialization = string.Empty;
            HireDate = DateTime.Now;
            Title = "Додати інструктора";
            SubmitText = "Додати";
        }

        public async Task InitializeForEdit(int instructorId)
        {
            _instructorId = instructorId;
            Title = "Редагувати інструктора";
            SubmitText = "Зберегти";
            
            var instructor = await _instructorService.GetInstructorByIdAsync(instructorId);
            if (instructor != null)
            {
                FirstName = instructor.FirstName;
                LastName = instructor.LastName;
                PhoneNumber = instructor.PhoneNumber ?? string.Empty;
                Email = instructor.Email ?? string.Empty;
                Specialization = instructor.Specialization ?? string.Empty;
                HireDate = instructor.HireDate;
            }
        }
    }
}