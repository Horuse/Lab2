using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using DanceSchool.Data.Entities;
using DanceSchool.Ui.Services;

namespace DanceSchool.Ui.ViewModels.Classes
{
    public partial class ClassesViewModel : ViewModelBase
    {
        private readonly ClassService _classService;

        [Reactive]
        private ObservableCollection<Class> _classes = new();

        [Reactive]
        private Class? _selectedClass;

        [Reactive]
        private bool _isLoading;

        [Reactive]
        private DateTime _selectedDate = DateTime.Today;

        public ClassesViewModel(ClassService classService)
        {
            _classService = classService;
        }

        [ReactiveCommand]
        private async Task LoadClassesAsync()
        {
            IsLoading = true;
            try
            {
                var classes = await _classService.GetAllClassesAsync();
                Classes.Clear();
                foreach (var cls in classes)
                {
                    Classes.Add(cls);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        private async Task LoadClassesByDateAsync()
        {
            IsLoading = true;
            try
            {
                var classes = await _classService.GetClassesByDateAsync(SelectedDate);
                Classes.Clear();
                foreach (var cls in classes)
                {
                    Classes.Add(cls);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        private async Task LoadUpcomingClassesAsync()
        {
            IsLoading = true;
            try
            {
                var classes = await _classService.GetUpcomingClassesAsync();
                Classes.Clear();
                foreach (var cls in classes)
                {
                    Classes.Add(cls);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        private async Task DeleteClassAsync(int id)
        {
            await _classService.DeleteClassAsync(id);
            await LoadClassesAsync();
        }
    }
}