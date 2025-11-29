using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using DanceSchool.Ui.ViewModels;
using DanceSchool.Ui.Views;
using Microsoft.Extensions.DependencyInjection;
using DanceSchool.Data;
using DanceSchool.Ui.Repositories;
using DanceSchool.Ui.Services;
using Microsoft.EntityFrameworkCore;
using Splat;
using ShadUI;
using DanceSchool.Ui.ViewModels.Students;
using DanceSchool.Ui.Views.Students;

namespace DanceSchool.Ui;

public partial class App : Application
{
    public IServiceProvider? ServiceProvider { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
        Locator.CurrentMutable.RegisterConstant(new AvaloniaActivationForViewFetcher(), typeof(IActivationForViewFetcher));
        Locator.CurrentMutable.RegisterConstant(new AutoDataTemplateBindingHook(), typeof(IPropertyBindingHook));
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();
        
        RegisterDialogs(ServiceProvider);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            
            var mainViewModel = ServiceProvider.GetRequiredService<MainWindowViewModel>();
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices(ServiceCollection services)
    {
        services.AddDbContext<DanceSchoolDbContext>(options =>
            options.UseSqlite("Data Source=danceschool.db"));

        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IInstructorRepository, InstructorRepository>();
        services.AddScoped<IClassRepository, ClassRepository>();
        services.AddScoped<IStudioRepository, StudioRepository>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();

        services.AddScoped<StudentService>();
        services.AddScoped<GroupService>();
        services.AddScoped<InstructorService>();
        services.AddScoped<ClassService>();
        services.AddScoped<StudioService>();

        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ViewModels.Students.StudentsViewModel>();
        services.AddTransient<ViewModels.Groups.GroupsViewModel>();
        services.AddTransient<ViewModels.Classes.ClassesViewModel>();
        services.AddTransient<ViewModels.Instructors.InstructorsViewModel>();
        services.AddTransient<AddStudentViewModel>();
        
        
        services.AddSingleton<DialogManager>();
        
        services.AddTransient<IServiceProvider>(sp => sp);
        
        using var scope = services.BuildServiceProvider().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DanceSchoolDbContext>();
        DanceSchool.Ui.Services.DbInitializer.Seed(context);
    }

    private void RegisterDialogs(IServiceProvider serviceProvider)
    {
        var dialogManager = serviceProvider.GetRequiredService<DialogManager>();
        dialogManager.Register<AddStudentDialog, AddStudentViewModel>();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}