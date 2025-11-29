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

namespace DanceSchool.Ui.ViewModels.Instructors
{
    public partial class InstructorsViewModel : ViewModelBase
    {
        private readonly InstructorService _instructorService;
        private readonly DialogManager _dialogManager;
        private readonly IServiceProvider _serviceProvider;

        public ObservableCollection<InstructorItemViewModel> Instructors { get; } = new();

        [Reactive]
        private ObservableCollection<InstructorItemViewModel> _filteredInstructors = new();

        [Reactive]
        private bool _isLoading;

        [Reactive]
        private string _searchText = string.Empty;

        public InstructorsViewModel(InstructorService instructorService, DialogManager dialogManager, IServiceProvider serviceProvider)
        {
            _instructorService = instructorService;
            _dialogManager = dialogManager;
            _serviceProvider = serviceProvider;

            this.WhenAnyValue(x => x.SearchText)
                .Subscribe(_ => FilterInstructors());
        }

        [ReactiveCommand]
        private void AddInstructor()
        {
            var addInstructorViewModel = _serviceProvider.GetRequiredService<AddInstructorViewModel>();
            addInstructorViewModel.Initialize();
            
            _dialogManager.CreateDialog(addInstructorViewModel)
                .Dismissible()
                .WithSuccessCallback(async vm =>
                {
                    await LoadInstructors();
                })
                .Show();
        }

        [ReactiveCommand]
        public async Task LoadInstructors()
        {
            IsLoading = true;
            try
            {
                var instructors = await _instructorService.GetAllInstructorsAsync();
                Instructors.Clear();
                
                foreach (var instructor in instructors)
                {
                    Instructors.Add(new InstructorItemViewModel
                    {
                        Id = instructor.Id,
                        FirstName = instructor.FirstName,
                        LastName = instructor.LastName,
                        PhoneNumber = instructor.PhoneNumber,
                        Email = instructor.Email,
                        Specialization = instructor.Specialization,
                        HireDate = instructor.HireDate,
                        GroupsCount = instructor.GroupInstructors?.Count ?? 0,
                        ClassesCount = instructor.Classes?.Count ?? 0
                    });
                }
                FilterInstructors();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void FilterInstructors()
        {
            FilteredInstructors.Clear();
            
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                foreach (var instructor in Instructors)
                {
                    FilteredInstructors.Add(instructor);
                }
            }
            else
            {
                var searchLower = SearchText.ToLowerInvariant();
                var filtered = Instructors.Where(i => 
                    (i.FirstName?.ToLowerInvariant().Contains(searchLower) ?? false) ||
                    (i.LastName?.ToLowerInvariant().Contains(searchLower) ?? false) ||
                    (i.Email?.ToLowerInvariant().Contains(searchLower) ?? false) ||
                    (i.PhoneNumber?.Contains(SearchText) ?? false) ||
                    (i.Specialization?.ToLowerInvariant().Contains(searchLower) ?? false));
                
                foreach (var instructor in filtered)
                {
                    FilteredInstructors.Add(instructor);
                }
            }
        }

        [ReactiveCommand]
        private async Task EditInstructor(int id)
        {
            var addInstructorViewModel = _serviceProvider.GetRequiredService<AddInstructorViewModel>();
            await addInstructorViewModel.InitializeForEdit(id);
            
            _dialogManager.CreateDialog(addInstructorViewModel)
                .Dismissible()
                .WithSuccessCallback(async vm =>
                {
                    await LoadInstructors();
                })
                .Show();
        }

        [ReactiveCommand]
        private async Task DeleteInstructorAsync(int id)
        {
            await _instructorService.DeleteInstructorAsync(id);
            await LoadInstructors();
        }
    }
}