using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using DanceSchool.Ui.Services;
using DanceSchool.Ui.ViewModels.Students;
using DanceSchool.Ui.ViewModels.Groups;
using DanceSchool.Ui.ViewModels.Classes;
using DanceSchool.Ui.ViewModels.Instructors;
using DanceSchool.Ui.ViewModels.Attendances;
using DanceSchool.Ui.ViewModels.Studios;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reactive;
using ShadUI;

namespace DanceSchool.Ui.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;

    [Reactive]
    private string _title = "Танцювальна Школа";

    [Reactive]
    private string _selectedView = "Dashboard";

    public ObservableCollection<string> MenuItems { get; }
    public DialogManager DialogManager { get; }
    public ToastManager ToastManager { get; }

    public StudentsViewModel StudentsViewModel { get; }
    public GroupsViewModel GroupsViewModel { get; }
    public ClassesViewModel ClassesViewModel { get; }
    public InstructorsViewModel InstructorsViewModel { get; }
    public AttendancesViewModel AttendancesViewModel { get; }
    public StudiosViewModel StudiosViewModel { get; }

    public MainWindowViewModel(IServiceProvider serviceProvider, DialogManager dialogManager, ToastManager toastManager)
    {
        _serviceProvider = serviceProvider;
        DialogManager = dialogManager;
        ToastManager = toastManager;
        
        // Initialize child ViewModels
        StudentsViewModel = _serviceProvider.GetRequiredService<StudentsViewModel>();
        GroupsViewModel = _serviceProvider.GetRequiredService<GroupsViewModel>();
        ClassesViewModel = _serviceProvider.GetRequiredService<ClassesViewModel>();
        InstructorsViewModel = _serviceProvider.GetRequiredService<InstructorsViewModel>();
        AttendancesViewModel = _serviceProvider.GetRequiredService<AttendancesViewModel>();
        StudiosViewModel = _serviceProvider.GetRequiredService<StudiosViewModel>();
        
        // Setup navigation from classes to attendance
        ClassesViewModel.NavigateToGroupAttendance = NavigateToGroupAttendance;
        
        MenuItems = new ObservableCollection<string>
        {
            "Головна",
            "Студенти",
            "Групи", 
            "Інструктори",
            "Заняття",
            "Відвідування",
            "Студії"
        };
    }

    [ReactiveCommand]
    private void NavigateTo(string view)
    {
        SelectedView = view;
        
        // Initialize data when navigating to a view
        switch (view)
        {
            case "Students":
                StudentsViewModel.LoadStudentsCommand.Execute(Unit.Default);
                break;
            case "Groups":
                GroupsViewModel.LoadGroupsCommand.Execute(Unit.Default);
                break;
            case "Classes":
                ClassesViewModel.LoadClassesCommand.Execute(Unit.Default);
                break;
            case "Instructors":
                InstructorsViewModel.LoadInstructorsCommand.Execute(Unit.Default);
                break;
            case "Attendance":
                AttendancesViewModel.LoadGroupsCommand.Execute(Unit.Default);
                break;
            case "Studios":
                StudiosViewModel.LoadStudiosCommand.Execute(Unit.Default);
                break;
        }
    }
    
    private void NavigateToGroupAttendance(int groupId)
    {
        SelectedView = "Attendance";
        AttendancesViewModel.LoadGroupsCommand.Execute(Unit.Default);
        AttendancesViewModel.SelectGroupCommand.Execute(groupId);
    }
}
