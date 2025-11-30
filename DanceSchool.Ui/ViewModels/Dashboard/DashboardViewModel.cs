using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using DanceSchool.Data.Entities;
using DanceSchool.Ui.Services;
using DanceSchool.Ui.Models;

namespace DanceSchool.Ui.ViewModels.Dashboard
{
    public enum PersonType
    {
        Instructor,
        Student
    }

    public partial class DashboardViewModel : ViewModelBase
    {
        private readonly ClassService _classService;
        private readonly InstructorService _instructorService;
        private readonly StudentService _studentService;

        [Reactive]
        private PersonType _selectedPersonType = PersonType.Instructor;

        [Reactive]
        private object? _selectedPerson;

        [Reactive]
        private DateTime _selectedDate = DateTime.Today;

        [Reactive]
        private bool _isLoading;

        public ObservableCollection<Instructor> Instructors { get; } = new();
        public ObservableCollection<Student> Students { get; } = new();
        public ObservableCollection<Class> TodayClasses { get; } = new();

        public DashboardViewModel(ClassService classService, InstructorService instructorService, StudentService studentService)
        {
            _classService = classService;
            _instructorService = instructorService;
            _studentService = studentService;

            // При зміні типу особи скидаємо вибір
            this.WhenAnyValue(x => x.SelectedPersonType)
                .Subscribe(_ => SelectedPerson = null);

            // При зміні особи або дати завантажуємо розклад
            this.WhenAnyValue(x => x.SelectedPerson, x => x.SelectedDate)
                .Subscribe(_ => LoadScheduleCommand.Execute(Unit.Default));
        }

        public async Task Initialize()
        {
            IsLoading = true;
            try
            {
                await LoadPersonsAsync();
                if (Instructors.Any())
                {
                    SelectedPerson = Instructors.First();
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        private async Task LoadPersons()
        {
            await LoadPersonsAsync();
        }

        private async Task LoadPersonsAsync()
        {
            var instructors = await _instructorService.GetAllInstructorsAsync();
            var students = await _studentService.GetAllStudentsAsync();

            Instructors.Clear();
            foreach (var instructor in instructors)
            {
                Instructors.Add(instructor);
            }

            Students.Clear();
            foreach (var student in students)
            {
                Students.Add(student);
            }
        }

        [ReactiveCommand]
        private async Task LoadSchedule()
        {
            if (SelectedPerson == null) return;

            IsLoading = true;
            try
            {
                TodayClasses.Clear();
                
                IEnumerable<Class> classes = new List<Class>();

                if (SelectedPersonType == PersonType.Instructor && SelectedPerson is Instructor instructor)
                {
                    classes = await _classService.GetClassesByInstructorAndDateAsync(instructor.Id, SelectedDate);
                }
                else if (SelectedPersonType == PersonType.Student && SelectedPerson is Student student)
                {
                    classes = await _classService.GetClassesByStudentAndDateAsync(student.Id, SelectedDate);
                }

                foreach (var classItem in classes.OrderBy(c => c.StartTime))
                {
                    TodayClasses.Add(classItem);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        private void ChangePersonType(PersonType personType)
        {
            SelectedPersonType = personType;
        }
    }
}