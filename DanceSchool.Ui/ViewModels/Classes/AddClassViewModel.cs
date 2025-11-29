using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using DanceSchool.Data.Entities;
using DanceSchool.Data.Enums;
using DanceSchool.Ui.Services;
using DanceSchool.Ui.Validators;
using ShadUI;

namespace DanceSchool.Ui.ViewModels.Classes
{
    public partial class AddClassViewModel : ViewModelBase
    {
        private readonly ClassService _classService;
        private readonly GroupService _groupService;
        private readonly InstructorService _instructorService;
        private readonly StudioService _studioService;
        private readonly DialogManager _dialogManager;

        [Required(ErrorMessage = "Дата є обов'язковою")]
        [Reactive]
        private DateTime _date = DateTime.Today;

        [Required(ErrorMessage = "Час початку є обов'язковим")]
        [StartTimeValidation(nameof(EndTime), ErrorMessage = "Час початку повинен бути раніше часу завершення")]
        [Reactive]
        private TimeSpan _startTime = new TimeSpan(9, 0, 0);

        [Required(ErrorMessage = "Час завершення є обов'язковим")]
        [EndTimeValidation(nameof(StartTime), ErrorMessage = "Час завершення повинен бути пізніше часу початку")]
        [Reactive]
        private TimeSpan _endTime = new TimeSpan(10, 0, 0);

        [Required(ErrorMessage = "Тип заняття є обов'язковим")]
        [Reactive]
        private ClassType _classType = ClassType.Regular;

        [MaxLength(200, ErrorMessage = "Тема не може перевищувати 200 символів")]
        [Reactive]
        private string? _topic;

        [Required(ErrorMessage = "Група є обов'язковою")]
        [Reactive]
        private Group? _selectedGroup;

        [Required(ErrorMessage = "Інструктор є обов'язковим")]
        [Reactive]
        private Instructor? _selectedInstructor;

        [Required(ErrorMessage = "Студія є обов'язковою")]
        [Reactive]
        private Studio? _selectedStudio;

        [Reactive]
        private bool _isLoading;

        public ObservableCollection<Group> Groups { get; } = new();
        public ObservableCollection<Instructor> Instructors { get; } = new();
        public ObservableCollection<Studio> Studios { get; } = new();
        public ObservableCollection<ClassType> ClassTypes { get; } = new();

        public AddClassViewModel(ClassService classService, GroupService groupService, 
                               InstructorService instructorService, StudioService studioService, 
                               DialogManager dialogManager)
        {
            _classService = classService;
            _groupService = groupService;
            _instructorService = instructorService;
            _studioService = studioService;
            _dialogManager = dialogManager;

            // Initialize ClassTypes
            ClassTypes.Clear();
            foreach (ClassType classType in Enum.GetValues<ClassType>())
            {
                ClassTypes.Add(classType);
            }
        }

        public async void Initialize()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            IsLoading = true;
            try
            {
                var groups = await _groupService.GetAllGroupsAsync();
                var instructors = await _instructorService.GetAllInstructorsAsync();
                var studios = await _studioService.GetAllStudiosAsync();

                Groups.Clear();
                foreach (var group in groups)
                {
                    Groups.Add(group);
                }

                Instructors.Clear();
                foreach (var instructor in instructors)
                {
                    Instructors.Add(instructor);
                }

                Studios.Clear();
                foreach (var studio in studios)
                {
                    Studios.Add(studio);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [ReactiveCommand]
        private async Task Submit()
        {
            ClearAllErrors();
            ValidateAllProperties();
            
            if (HasErrors) return;

            try
            {
                var newClass = new Class
                {
                    Date = Date,
                    StartTime = StartTime,
                    EndTime = EndTime,
                    ClassType = ClassType,
                    Topic = Topic,
                    GroupId = SelectedGroup!.Id,
                    InstructorId = SelectedInstructor!.Id,
                    StudioId = SelectedStudio!.Id
                };

                var success = await _classService.CreateClassAsync(newClass);
                if (success)
                {
                    _dialogManager.Close(this, new CloseDialogOptions { Success = true });
                }
                else
                {
                    // SetError(nameof(Date), "Конфлікт розкладу: студія або інструктор зайняті в цей час");
                }
            }
            catch (Exception ex)
            {
                // SetError(nameof(Date), $"Помилка створення заняття: {ex.Message}");
            }
        }

        [ReactiveCommand]
        private void Cancel()
        {
            _dialogManager.Close(this, new CloseDialogOptions { Success = false });
        }
    }
}