using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using CommunityToolkit.Mvvm.Input;
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
        private readonly ToastManager _toastManager;
        private int? _classId; // null для додавання, id для редагування

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

        [Reactive]
        private bool _isBusy;

        [Reactive]
        private string _title = "Додати заняття";

        [Reactive]
        private string _submitText = "Додати";

        private string _warningToastCode = string.Empty;

        public ObservableCollection<Group> Groups { get; } = new();
        public ObservableCollection<Instructor> Instructors { get; } = new();
        public ObservableCollection<Studio> Studios { get; } = new();
        public ObservableCollection<ClassType> ClassTypes { get; } = new();

        public AddClassViewModel(ClassService classService, GroupService groupService, 
                               InstructorService instructorService, StudioService studioService, 
                               DialogManager dialogManager, ToastManager toastManager)
        {
            _classService = classService;
            _groupService = groupService;
            _instructorService = instructorService;
            _studioService = studioService;
            _dialogManager = dialogManager;
            _toastManager = toastManager;

            // Initialize ClassTypes
            ClassTypes.Clear();
            foreach (ClassType classType in Enum.GetValues<ClassType>())
            {
                ClassTypes.Add(classType);
            }
        }

        public async void Initialize()
        {
            _classId = null;
            Title = "Додати заняття";
            SubmitText = "Додати";
            await LoadDataAsync();
        }

        public async Task InitializeForEdit(int classId)
        {
            _classId = classId;
            Title = "Редагувати заняття";
            SubmitText = "Зберегти";
            
            await LoadDataAsync();
            
            var classItem = await _classService.GetClassByIdAsync(classId);
            if (classItem != null)
            {
                Date = classItem.Date;
                StartTime = classItem.StartTime;
                EndTime = classItem.EndTime;
                ClassType = classItem.ClassType;
                Topic = classItem.Topic;
                SelectedGroup = Groups.FirstOrDefault(g => g.Id == classItem.GroupId);
                SelectedInstructor = Instructors.FirstOrDefault(i => i.Id == classItem.InstructorId);
                SelectedStudio = Studios.FirstOrDefault(s => s.Id == classItem.StudioId);
            }
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

            IsBusy = true;
            try
            {
                if (_classId.HasValue)
                {
                    // Редагування існуючого заняття
                    var classItem = await _classService.GetClassByIdAsync(_classId.Value);
                    if (classItem != null)
                    {
                        classItem.Date = Date;
                        classItem.StartTime = StartTime;
                        classItem.EndTime = EndTime;
                        classItem.ClassType = ClassType;
                        classItem.Topic = Topic;
                        classItem.GroupId = SelectedGroup!.Id;
                        classItem.InstructorId = SelectedInstructor!.Id;
                        classItem.StudioId = SelectedStudio!.Id;

                        var success = await _classService.UpdateClassAsync(classItem);
                        if (success)
                        {
                            _dialogManager.Close(this, new CloseDialogOptions { Success = true });
                        }
                    }
                }
                else
                {
                    // Додавання нового заняття
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
                }
            }
            catch (Exception ex)
            {
                ShowErrorToast(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [ReactiveCommand]
        public void Cancel()
        {
            _dialogManager.Close(this, new CloseDialogOptions { Success = false });
        }

        [RelayCommand]
        private void ShowErrorToast(string message)
        {
            _toastManager.CreateToast("Помилка при створенні заняття")
                .WithContent(message)
                .Show();
        }
    }
}