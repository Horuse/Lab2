using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using DanceSchool.Data.Entities;
using DanceSchool.Ui.Services;
using ShadUI;
using Microsoft.Extensions.DependencyInjection;

namespace DanceSchool.Ui.ViewModels.Studios
{
    public partial class StudiosViewModel : ViewModelBase
    {
        private readonly StudioService _studioService;
        private readonly DialogManager _dialogManager;
        private readonly IServiceProvider _serviceProvider;

        public ObservableCollection<StudioItemViewModel> Studios { get; } = new();

        [Reactive]
        private ObservableCollection<StudioItemViewModel> _filteredStudios = new();

        [Reactive]
        private bool _isLoading;

        [Reactive]
        private string _searchText = string.Empty;

        public StudiosViewModel(StudioService studioService, DialogManager dialogManager, IServiceProvider serviceProvider)
        {
            _studioService = studioService;
            _dialogManager = dialogManager;
            _serviceProvider = serviceProvider;

            this.WhenAnyValue(x => x.SearchText)
                .Subscribe(_ => FilterStudios());
        }

        [ReactiveCommand]
        private void AddStudio()
        {
            var addStudioViewModel = _serviceProvider.GetRequiredService<AddStudioViewModel>();
            addStudioViewModel.Initialize();
            
            _dialogManager.CreateDialog(addStudioViewModel)
                .Dismissible()
                .WithSuccessCallback(async vm =>
                {
                    await LoadStudios();
                })
                .Show();
        }

        [ReactiveCommand]
        public async Task LoadStudios()
        {
            IsLoading = true;
            try
            {
                var studios = await _studioService.GetAllStudiosAsync();
                Studios.Clear();
                
                foreach (var studio in studios)
                {
                    Studios.Add(new StudioItemViewModel
                    {
                        Id = studio.Id,
                        Name = studio.Name,
                        Capacity = studio.Capacity,
                        FloorType = studio.FloorType ?? "Не вказано",
                        Equipment = studio.Equipment ?? "Не вказано",
                        IsAvailable = studio.IsAvailable,
                        Notes = studio.Notes ?? string.Empty
                    });
                }
                FilterStudios();
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        private async Task EditStudio(int id)
        {
            var editStudioViewModel = _serviceProvider.GetRequiredService<AddStudioViewModel>();
            await editStudioViewModel.Initialize(id);
            
            _dialogManager.CreateDialog(editStudioViewModel)
                .Dismissible()
                .WithSuccessCallback(async vm =>
                {
                    await LoadStudios();
                })
                .Show();
        }

        [ReactiveCommand]
        private async Task DeleteStudioAsync(int id)
        {
            await _studioService.DeleteStudioAsync(id);
            await LoadStudios();
        }

        private void FilterStudios()
        {
            FilteredStudios.Clear();
            
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                foreach (var studio in Studios)
                {
                    FilteredStudios.Add(studio);
                }
            }
            else
            {
                var searchLower = SearchText.ToLowerInvariant();
                var filtered = Studios.Where(s => 
                    (s.Name?.ToLowerInvariant().Contains(searchLower) ?? false) ||
                    (s.FloorType?.ToLowerInvariant().Contains(searchLower) ?? false) ||
                    (s.Equipment?.ToLowerInvariant().Contains(searchLower) ?? false));
                
                foreach (var studio in filtered)
                {
                    FilteredStudios.Add(studio);
                }
            }
        }
    }

    public class StudioItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string FloorType { get; set; } = string.Empty;
        public string Equipment { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string AvailabilityText => IsAvailable ? "Доступна" : "Недоступна";
    }
}