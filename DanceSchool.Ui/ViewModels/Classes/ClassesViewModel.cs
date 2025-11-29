using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using DanceSchool.Data.Entities;
using DanceSchool.Ui.Services;
using ShadUI;

namespace DanceSchool.Ui.ViewModels.Classes
{
    public partial class ClassesViewModel : ViewModelBase
    {
        private readonly ClassService _classService;
        private readonly DialogManager _dialogManager;
        private readonly IServiceProvider _serviceProvider;

        public ObservableCollection<ClassItemViewModel> Classes { get; } = new();

        [Reactive]
        private bool _isLoading;

        [Reactive]
        private DateTime _selectedDate = DateTime.Today;

        public ClassesViewModel(ClassService classService, DialogManager dialogManager, IServiceProvider serviceProvider)
        {
            _classService = classService;
            _dialogManager = dialogManager;
            _serviceProvider = serviceProvider;
        }

        [ReactiveCommand]
        private void AddClass()
        {
            var addClassViewModel = _serviceProvider.GetRequiredService<AddClassViewModel>();
            addClassViewModel.Initialize();
            
            _dialogManager.CreateDialog(addClassViewModel)
                .Dismissible()
                .WithSuccessCallback(async vm =>
                {
                    await LoadClassesAsync();
                })
                .Show();
        }

        [ReactiveCommand]
        public async Task LoadClassesAsync()
        {
            IsLoading = true;
            try
            {
                var classes = await _classService.GetAllClassesAsync();
                Classes.Clear();
                
                foreach (var classItem in classes)
                {
                    Classes.Add(new ClassItemViewModel
                    {
                        Id = classItem.Id,
                        Date = classItem.Date,
                        StartTime = classItem.StartTime,
                        EndTime = classItem.EndTime,
                        ClassType = classItem.ClassType,
                        Topic = classItem.Topic,
                        GroupName = classItem.Group?.Name ?? "Невідома група",
                        InstructorName = classItem.Instructor != null ? 
                            $"{classItem.Instructor.FirstName} {classItem.Instructor.LastName}" : 
                            "Невідомий інструктор",
                        StudioName = classItem.Studio?.Name ?? "Невідома студія",
                        AttendanceCount = classItem.Attendances?.Count ?? 0
                    });
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        private async Task EditClass(int id)
        {
            var addClassViewModel = _serviceProvider.GetRequiredService<AddClassViewModel>();
            await addClassViewModel.InitializeForEdit(id);
            
            _dialogManager.CreateDialog(addClassViewModel)
                .Dismissible()
                .WithSuccessCallback(async vm =>
                {
                    await LoadClassesAsync();
                })
                .Show();
        }

        [ReactiveCommand]
        private async Task DeleteClassAsync(int id)
        {
            await _classService.DeleteClassAsync(id);
            await LoadClassesAsync();
        }
    }
}