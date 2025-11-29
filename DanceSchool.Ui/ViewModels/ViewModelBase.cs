using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using ReactiveUI;

namespace DanceSchool.Ui.ViewModels;

public abstract class ViewModelBase : ReactiveObject, INotifyDataErrorInfo
{
    private readonly Dictionary<string, List<string>> _errors = new();
    
    public bool HasErrors => _errors.Count > 0;
    
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    
    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrEmpty(propertyName))
        {
            return _errors.Values.SelectMany(v => v);
        }
        
        return _errors.TryGetValue(propertyName, out var errors) ? errors : Enumerable.Empty<string>();
    }
    
    protected void ClearErrors(string propertyName)
    {
        if (_errors.Remove(propertyName)) 
        {
            OnErrorsChanged(propertyName);
            this.RaisePropertyChanged(nameof(HasErrors));
        }
    }
    
    protected void ValidateProperty<T>(T value, string propertyName)
    {
        ClearErrors(propertyName);

        var validationContext = new ValidationContext(this)
        {
            MemberName = propertyName
        };
        var validationResults = new List<ValidationResult>();

        if (Validator.TryValidateProperty(value, validationContext, validationResults)) return;

        foreach (var validationResult in validationResults)
        {
            AddError(propertyName, validationResult.ErrorMessage ?? string.Empty);
        }
    }
    
    protected void AddError(string propertyName, string error)
    {
        if (!_errors.ContainsKey(propertyName))
        {
            _errors[propertyName] = new List<string>();
        }

        if (_errors[propertyName].Contains(error)) return;

        _errors[propertyName].Add(error);
        OnErrorsChanged(propertyName);
        this.RaisePropertyChanged(nameof(HasErrors));
    }
    
    protected void ValidateAllProperties()
    {
        var properties = GetType().GetProperties()
            .Where(prop => prop.GetCustomAttributes(typeof(ValidationAttribute), true).Length != 0);

        foreach (var property in properties)
        {
            var value = property.GetValue(this);
            ValidateProperty(value, property.Name);
        }
    }

    protected void ClearAllErrors()
    {
        var properties = _errors.Keys.ToList();
        _errors.Clear();
        foreach (var property in properties) 
        {
            OnErrorsChanged(property);
        }
        this.RaisePropertyChanged(nameof(HasErrors));
    }

    private void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
}
