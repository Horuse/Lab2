using System.Threading.Tasks;
using ReactiveUI.SourceGenerators;
using DanceSchool.Data.Entities;
using DanceSchool.Ui.Services;
using ShadUI;

namespace DanceSchool.Ui.ViewModels.Studios
{
    public partial class AddStudioViewModel : ViewModelBase
    {
        private readonly StudioService _studioService;
        private readonly DialogManager _dialogManager;

        [Reactive]
        private string _name = string.Empty;

        [Reactive]
        private int _capacity = 1;

        [Reactive]
        private string _floorType = string.Empty;

        [Reactive]
        private string _equipment = string.Empty;

        [Reactive]
        private bool _isAvailable = true;

        [Reactive]
        private string _notes = string.Empty;

        [Reactive]
        private bool _isBusy;

        public string SubmitText => IsEdit ? "Оновити" : "Додати";
        public string Title => IsEdit ? "Редагувати студію" : "Додати студію";
        public bool IsEdit { get; private set; }
        private int _studioId;

        public AddStudioViewModel(StudioService studioService, DialogManager dialogManager)
        {
            _studioService = studioService;
            _dialogManager = dialogManager;
        }

        public void Initialize()
        {
            IsEdit = false;
            _studioId = 0;
            Name = string.Empty;
            Capacity = 1;
            FloorType = string.Empty;
            Equipment = string.Empty;
            IsAvailable = true;
            Notes = string.Empty;
        }

        public async Task Initialize(int studioId)
        {
            IsEdit = true;
            _studioId = studioId;
            
            var studio = await _studioService.GetStudioByIdAsync(studioId);
            if (studio != null)
            {
                Name = studio.Name;
                Capacity = studio.Capacity;
                FloorType = studio.FloorType ?? string.Empty;
                Equipment = studio.Equipment ?? string.Empty;
                IsAvailable = studio.IsAvailable;
                Notes = studio.Notes ?? string.Empty;
            }
        }

        [ReactiveCommand]
        private async Task SubmitAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return;

            IsBusy = true;
            try
            {
                if (IsEdit)
                {
                    var studio = await _studioService.GetStudioByIdAsync(_studioId);
                    if (studio != null)
                    {
                        studio.Name = Name.Trim();
                        studio.Capacity = Capacity;
                        studio.FloorType = string.IsNullOrWhiteSpace(FloorType) ? null : FloorType.Trim();
                        studio.Equipment = string.IsNullOrWhiteSpace(Equipment) ? null : Equipment.Trim();
                        studio.IsAvailable = IsAvailable;
                        studio.Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim();

                        await _studioService.UpdateStudioAsync(studio);
                    }
                }
                else
                {
                    var studio = new Studio
                    {
                        Name = Name.Trim(),
                        Capacity = Capacity,
                        FloorType = string.IsNullOrWhiteSpace(FloorType) ? null : FloorType.Trim(),
                        Equipment = string.IsNullOrWhiteSpace(Equipment) ? null : Equipment.Trim(),
                        IsAvailable = IsAvailable,
                        Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim()
                    };

                    await _studioService.CreateStudioAsync(studio);
                }
                
                _dialogManager.Close(this, new CloseDialogOptions { Success = true });
            }
            catch
            {
                // Handle any errors
            }
            finally
            {
                IsBusy = false;
            }
        }

        [ReactiveCommand]
        private void Cancel()
        {
            _dialogManager.Close(this);
        }
    }
}