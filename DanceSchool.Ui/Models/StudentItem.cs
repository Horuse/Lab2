using System;
using DanceSchool.Data.Entities;
using DanceSchool.Data.Enums;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace DanceSchool.Ui.Models
{
    public partial class StudentItem : ReactiveObject
    {
        [Reactive]
        private bool _isSelected;

        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? ParentContact { get; set; }
        public SkillLevel SkillLevel { get; set; }
        public string? HealthNotes { get; set; }
        public DateTime RegistrationDate { get; set; }

        public static StudentItem FromStudent(Student student)
        {
            return new StudentItem
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                DateOfBirth = student.DateOfBirth,
                PhoneNumber = student.PhoneNumber,
                Email = student.Email,
                ParentContact = student.ParentContact,
                SkillLevel = student.SkillLevel,
                HealthNotes = student.HealthNotes,
                RegistrationDate = student.RegistrationDate
            };
        }

        public Student ToStudent()
        {
            return new Student
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                DateOfBirth = DateOfBirth,
                PhoneNumber = PhoneNumber,
                Email = Email,
                ParentContact = ParentContact,
                SkillLevel = SkillLevel,
                HealthNotes = HealthNotes,
                RegistrationDate = RegistrationDate
            };
        }
    }
}