using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using DanceSchool.Data.Entities;
using DanceSchool.Ui.Services;

namespace DanceSchool.Ui.ViewModels.Instructors
{
    public partial class InstructorsViewModel : ViewModelBase
    {
        private readonly InstructorService _instructorService;

        [Reactive]
        private ObservableCollection<Instructor> _instructors = new();

        [Reactive]
        private Instructor? _selectedInstructor;

        [Reactive]
        private bool _isLoading;

        public InstructorsViewModel(InstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [ReactiveCommand]
        private async Task LoadInstructorsAsync()
        {
            IsLoading = true;
            try
            {
                var instructors = await _instructorService.GetAllInstructorsAsync();
                Instructors.Clear();
                foreach (var instructor in instructors)
                {
                    Instructors.Add(instructor);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        private async Task AddInstructorAsync()
        {
            var newInstructor = new Instructor
            {
                FirstName = "Новий",
                LastName = "Інструктор",
                HireDate = DateTime.Now,
                Specialization = "Танці",
                Email = "instructor@danceschool.com"
            };

            await _instructorService.CreateInstructorAsync(newInstructor);
            await LoadInstructorsAsync();
        }

        [ReactiveCommand]
        private async Task DeleteInstructorAsync(int id)
        {
            await _instructorService.DeleteInstructorAsync(id);
            await LoadInstructorsAsync();
        }
    }
}