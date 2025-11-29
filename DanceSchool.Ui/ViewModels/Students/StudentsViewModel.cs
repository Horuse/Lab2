using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using DanceSchool.Data.Entities;
using DanceSchool.Ui.Models;
using DanceSchool.Ui.Services;

namespace DanceSchool.Ui.ViewModels.Students
{
    public partial class StudentsViewModel : ViewModelBase
    {
        private readonly StudentService _studentService;

        [Reactive]
        private ObservableCollection<StudentItem> _students = new();

        [Reactive]
        private StudentItem? _selectedStudent;

        [Reactive]
        private bool _isLoading;

        public StudentsViewModel(StudentService studentService)
        {
            _studentService = studentService;
        }

        [ReactiveCommand]
        private async Task LoadStudents()
        {
            IsLoading = true;
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                Students.Clear();
                foreach (var student in students)
                {
                    Students.Add(StudentItem.FromStudent(student));
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        private async Task AddStudent()
        {
            var newStudent = new Student
            {
                FirstName = "Новий",
                LastName = "Студент",
                DateOfBirth = DateTime.Now.AddYears(-10),
                RegistrationDate = DateTime.Now,
                SkillLevel = Data.Enums.SkillLevel.Starter
            };

            await _studentService.CreateStudentAsync(newStudent);
            await LoadStudents();
        }

        [ReactiveCommand]
        private async Task DeleteStudent(int id)
        {
            await _studentService.DeleteStudentAsync(id);
            await LoadStudents();
        }
    }
}