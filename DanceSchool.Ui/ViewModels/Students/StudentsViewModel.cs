using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using DanceSchool.Data.Entities;
using DanceSchool.Ui.Models;
using DanceSchool.Ui.Services;
using ShadUI;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace DanceSchool.Ui.ViewModels.Students
{
    public partial class StudentsViewModel : ViewModelBase
    {
        private readonly StudentService _studentService;
        private readonly DialogManager _dialogManager;
        private readonly IServiceProvider _serviceProvider;

        [Reactive]
        private ObservableCollection<StudentItem> _students = new();

        [Reactive]
        private ObservableCollection<StudentItem> _filteredStudents = new();

        [Reactive]
        private StudentItem? _selectedStudent;

        [Reactive]
        private bool _isLoading;

        [Reactive]
        private string _searchText = string.Empty;

        public StudentsViewModel(StudentService studentService, DialogManager dialogManager, IServiceProvider serviceProvider)
        {
            _studentService = studentService;
            _dialogManager = dialogManager;
            _serviceProvider = serviceProvider;

            this.WhenAnyValue(x => x.SearchText)
                .Subscribe(_ => FilterStudents());
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
                FilterStudents();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void FilterStudents()
        {
            FilteredStudents.Clear();
            
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                foreach (var student in Students)
                {
                    FilteredStudents.Add(student);
                }
            }
            else
            {
                var searchLower = SearchText.ToLowerInvariant();
                var filtered = Students.Where(s => 
                    (s.FirstName?.ToLowerInvariant().Contains(searchLower) ?? false) ||
                    (s.LastName?.ToLowerInvariant().Contains(searchLower) ?? false) ||
                    (s.Email?.ToLowerInvariant().Contains(searchLower) ?? false) ||
                    (s.PhoneNumber?.Contains(SearchText) ?? false));
                
                foreach (var student in filtered)
                {
                    FilteredStudents.Add(student);
                }
            }
        }

        [ReactiveCommand]
        private void AddStudent()
        {
            var addStudentViewModel = _serviceProvider.GetRequiredService<AddStudentViewModel>();
            addStudentViewModel.Initialize();
            
            _dialogManager.CreateDialog(addStudentViewModel)
                .Dismissible()
                .WithSuccessCallback(async vm =>
                {
                    await LoadStudents();
                })
                .Show();
        }

        [ReactiveCommand]
        private async Task EditStudent(int id)
        {
            var addStudentViewModel = _serviceProvider.GetRequiredService<AddStudentViewModel>();
            await addStudentViewModel.InitializeForEdit(id);
            
            _dialogManager.CreateDialog(addStudentViewModel)
                .Dismissible()
                .WithSuccessCallback(async vm =>
                {
                    await LoadStudents();
                })
                .Show();
        }

        [ReactiveCommand]
        private async Task DeleteStudent(int id)
        {
            await _studentService.DeleteStudentAsync(id);
            await LoadStudents();
        }
    }
}